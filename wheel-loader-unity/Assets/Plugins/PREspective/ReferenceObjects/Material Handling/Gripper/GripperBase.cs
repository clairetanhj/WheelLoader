#if UNITY_EDITOR || UNITY_EDITOR_BETA
using UnityEditor;
#endif
using System.Collections.Generic;
using System.Reflection;
using u040.prespective.prepair.inspector;
using UnityEngine;
using System;

namespace u040.prespective.referenceobjects.materialhandling.gripper
{
    public class GripperBase : MonoBehaviour, IControlPanel
    {
#pragma warning disable 0414 
        [SerializeField] [Obfuscation] private int toolbarTab; 
        #pragma warning restore 0414 


        /// <summary>
        /// A list of all Gripper Fingers assigned to this GripperBase
        /// </summary>
        public List<FingerSetting> GripperFingers = new List<FingerSetting>();


        /// <summary>
        /// Add a unique Gripper Finger
        /// </summary>
        /// <param name="_finger"></param>
        /// <returns></returns>
        public bool AddFinger(GripperFinger _finger, bool _inverted = false)
        {
            //If the list does not already contain the finger
            if (GripperFingers.Find((_entry) => _entry.Finger == _finger) == null)
            {
                //Add the finger
                FingerSetting _setting = new FingerSetting();
                _setting.Finger = _finger;
                _setting.Inverted = _inverted;
                GripperFingers.Add(_setting);
                return true;
            }

            //If the list does already contain the finger
            else
            {
                //Log warning, do not add finger
                Debug.LogWarning("Unable to add " + _finger.name + " since it has already been assigned as a finger");
                return false;
            }
        }
        public bool RemoveFinger(GripperFinger _finger)
        {
            //Find the FingerSetting for this finger
            FingerSetting _setting = GripperFingers.Find((_entry) => _entry.Finger == _finger);

            //Remove it from the list
            return GripperFingers.Remove(_setting);
        }

        private void Awake()
        {
            //Remove all null fingers from the list
            GripperFingers.RemoveAll((_entry) => _entry == null);

            //Force all fingers to the same set position
            moveFingersToPosition(currentClosePercentage);

            //Store the Gripper base's current transform position and rotation
            storeTransformValues();
        }

        private void FixedUpdate()
        {
            //Move objects along with the base if necessary
            processGrippedObjects();

            //Detect which object is currently being gripped
            detectGrippedObjects();

            //Make sure to open/close fingers if necessary
            positionFingers();

            /*
             * It is important that we move the finger into position last. This is necessary for a 
             * proper grip detection. If we move the fingers first, than detect a grip, the new 
             * actual grip will not be detected since the trigger events only fire after the loop
             * for the gripper base has been completed. This results in the gripper finger lagging 
             * a frame behind and the fingers to clip through gripped objects.
             */
        }


        #region<<Processing Gripper Objects>>

        private Vector3 storedTransformPosition;
        private Quaternion storedTransformRotation;

        private void processGrippedObjects()
        {
            /*
             * Each gripped object has to do 3 things:
             * - Cancel out gravity
             * - Inherit positional changes
             * - Inherit rotational changes
             */

            //Put drag on the rigidbodies
            //For each gripped object
            for (int _i = 0; _i < grippedObjects.Count; _i++)
            {
                //Try to get the Rigidbody
                Rigidbody _rigidbody = grippedObjects[_i].Object.GetComponent<Rigidbody>();

                //If it has a Rigidbody and Gravity is used
                if (_rigidbody)
                {
                    _rigidbody.velocity *= (0.5f * Time.fixedDeltaTime);
                    _rigidbody.angularVelocity *= (0.5f * Time.fixedDeltaTime);
                }
            }


            //Second: Inherit positional changes
            //Get current position
            Vector3 _currentTransformPosition = this.transform.position;

            //Check if there has been a change in position
            if (_currentTransformPosition != storedTransformPosition)
            {
                //Calculate the difference in position
                Vector3 _deltaTransformPosition = _currentTransformPosition - storedTransformPosition;

                //For each gripped object
                for (int _i = 0; _i < grippedObjects.Count; _i++)
                {
                    //Buffer the GameObject
                    GrippedObject _grippedObject = grippedObjects[_i];

                    //Apply the same change in position to the gripped object
                    _grippedObject.Object.transform.position += _deltaTransformPosition;
                }
            }


            //Third: Inherit rotational changes
            //Get current rotation
            Quaternion _currentTransformRotation = this.transform.rotation;

            //Check if there has been a change in rotation
            if (_currentTransformRotation != storedTransformRotation)
            {
                //Calculate the difference in rotation
                Quaternion _deltaTransformRotation = _currentTransformRotation * Quaternion.Inverse(storedTransformRotation);

                //For each gripped object
                for (int _i = 0; _i < grippedObjects.Count; _i++)
                {
                    //Buffer the GameObject
                    GrippedObject _grippedObject = grippedObjects[_i];

                    //Apply the same change in rotation to the gripped object's rotation
                    _grippedObject.Object.transform.rotation = _deltaTransformRotation * _grippedObject.Object.transform.rotation;

                    //Calculate the new relative position by applying the difference in rotation to the relative vector
                    Vector3 _rotatedRelativePosition = _deltaTransformRotation * _grippedObject.RelativePosition;

                    //Calculate the difference in relative position
                    Vector3 _deltaRelativePosition = _rotatedRelativePosition - _grippedObject.RelativePosition;

                    //Apply difference in relative position to the gripped object
                    _grippedObject.Object.transform.position += _deltaRelativePosition;
                }
            }

            //Store the current transform values for next frame
            storeTransformValues();
        }

        private void storeTransformValues()
        {
            storedTransformPosition = this.transform.position;
            storedTransformRotation = this.transform.rotation;
        }

        #endregion

        #region<< Gripping Objects >>
        /// <summary>
        /// A list of all gripped objects and their relative positions
        /// </summary>
        private List<GrippedObject> grippedObjects = new List<GrippedObject>();

        /// <summary>
        /// A list containing all gripped objects
        /// </summary>
        public List<GameObject> GrippedGameObjects
        {
            get
            {
                return this.grippedObjects.ConvertAll<GameObject>((_entry) => { return _entry.Object; });
            }
        }

        /// <summary>
        /// A list of all objects detected by fingers and the number of fingers that detect them
        /// </summary>
        [SerializeField] [Obfuscation(Exclude = true)] private Dictionary<GameObject, int> detectedObjects = new Dictionary<GameObject, int>();

        protected virtual void detectGrippedObjects()
        {
            //Clear the entire lists each frame for the new update
            clearGrippedObjectsList();
            detectedObjects.Clear();
            fingersCollide = false;

            //For each gripper finger
            for (int _i = 0; _i < GripperFingers.Count; _i++)
            {
                //Get all the objects the finger detects
                List<GameObject> _objects = GripperFingers[_i].Finger.DetectedObjects;

                

                //For each detected object
                for (int _j = 0; _j < _objects.Count; _j++)
                {
                    GameObject _object = _objects[_j];
                    int _count = 0;

                    //Check whether the detected object is another finger
                    if (GripperFingers.Find((_entry) => _entry.Finger.Trigger.gameObject == _object) != null)
                    {
                        fingersCollide = true;
                        continue;
                    }

                    //Check whether this object was already detected by other fingers
                    if (detectedObjects.TryGetValue(_object, out _count))
                    {
                        //Add 1 to its current value
                        detectedObjects[_object] = ++_count;
                    }

                    //If not already detected
                    else
                    {
                        //Add it as a detected object
                        detectedObjects.Add(_object, 1);
                        _count = 1;
                    }

                    //If the objects is detected by all fingers
                    if (_count == GripperFingers.Count)
                    {
                        //Check whether the object's rigidbody uses gravity
                        bool _usedGravity = false;
                        Rigidbody _rigidbody = _object.GetComponent<Rigidbody>();
                        if (_rigidbody)
                        {
                            _usedGravity = _rigidbody.useGravity;
                            _rigidbody.useGravity = false;
                        }

                        //Create the new datastruct
                        GrippedObject _grippedObject = new GrippedObject()
                        {
                            Object = _object,
                            RelativePosition = _object.transform.position - this.transform.position,
                            UsedGravity = _usedGravity
                        };

                        //Add it as a gripped object
                        grippedObjects.Add(_grippedObject);
                    }
                }
            }

            /*
             * If all grippers need a detection per definition, change system to set al detected object from finger 1 in dictionary, 
             * then for every other finger compare which dectected objects are in the dictionary, ignore the others an clear the 
             * dictionary entries that do not match. In the end only all matching objects remain or nothing is detected by every 
             * finger and the method can be stopped early.
             */
        }

        private void clearGrippedObjectsList()
        {
            for (int _i = 0; _i < grippedObjects.Count; _i++)
            {
                GrippedObject _grippedObject = grippedObjects[_i];
                Rigidbody _rigidbody = _grippedObject.Object.GetComponent<Rigidbody>();
                if (_rigidbody)
                {
                    _rigidbody.useGravity = _grippedObject.UsedGravity;
                }
            }
            grippedObjects.Clear();
        }

        /// <summary>
        /// A data struct to save information on a gripped object
        /// </summary>
        private struct GrippedObject
        {
            public GameObject Object;
            public Vector3 RelativePosition;
            public bool UsedGravity;
        }
        #endregion

        #region << Moving Fingers>>
        /// <summary>
        /// A bool to track whether fingers collider with each other
        /// </summary>
        private bool fingersCollide = false;

        [SerializeField] [Obfuscation(Exclude = true)] private float targetClosePercentage = 0f;
        /// <summary>
        /// The close percentage the gripper fingers need to move to
        /// </summary>
        public float TargetClosePercentage
        {
            get { return this.targetClosePercentage; }
            set
            {
                float _clampedValue = Mathf.Clamp(value, 0f, 1f);

                if (this.targetClosePercentage != _clampedValue)
                {
                    this.targetClosePercentage = _clampedValue;
                }
            }
        }

        [SerializeField] [Obfuscation(Exclude = true)] private float currentClosePercentage = 0f;
        /// <summary>
        /// The current close percentage of the fingers
        /// </summary>
        public float ClosePercentage
        {
            get { return this.currentClosePercentage; }
            private set
            {
                this.currentClosePercentage = value;
            }
        }

        /// <summary>
        /// The time the gripper fingers need to go from fully open to fully closed
        /// </summary>
        public float CloseTime = 1f;

        /// <summary>
        /// Close the fingers at a pace according to the set speed
        /// </summary>
        private void positionFingers()
        {
            //If we havent closed enough
            if (currentClosePercentage != TargetClosePercentage)
            {
                //Determine how many percent the gripper still needs to close
                float _percentageDifference = TargetClosePercentage - currentClosePercentage;

                //If we have an object detected or fingers collider with each other
                if (grippedObjects.Count > 0 || fingersCollide)
                {
                    //Only allow moves that open te gripper
                    _percentageDifference = Mathf.Min(_percentageDifference, 0f);

                    //If not move should occur, return
                    if (_percentageDifference == 0f) { return; }
                }

                //Clamp the percentage so that we never close more than the set speed allows over time
                float _maxAllowedMove = Time.fixedDeltaTime / this.CloseTime;
                float _clampedPercentageOverSpeed = Mathf.Clamp(_percentageDifference, -_maxAllowedMove, _maxAllowedMove);

                //Determine the position the fingers would need to move to
                currentClosePercentage = Mathf.Clamp(currentClosePercentage + _clampedPercentageOverSpeed, 0f, 1f);

                //Set all fingers to set percentage
                moveFingersToPosition(currentClosePercentage);
            }
        }

        /// <summary>
        /// Move all fingers to the same position
        /// </summary>
        /// <param name="_percentage"></param>
        private void moveFingersToPosition(float _percentage)
        {
            
            //Set all fingers to set percentage
            for (int _i = 0; _i < GripperFingers.Count; _i++)
            {
                float _positionPercentage = _percentage;

                //if percentage needs to be inverted
                if(GripperFingers[_i].Inverted)
                {
                    _positionPercentage = 1f - _percentage;
                }

                //Set position of individual finger
                GripperFingers[_i].Finger.SetPosition(_positionPercentage);
            }
        }

        public enum GripperState { Open = 0, Closed = 1, Opening = 2, Closing = 3 };

        public GripperState State
        {
            get
            {
                if (GrippedGameObjects.Count > 0 || ClosePercentage == 1f || fingersCollide)
                {
                    return GripperState.Closed;
                }
                else if (ClosePercentage == TargetClosePercentage)
                {
                    return GripperState.Open;
                }
                else
                {
                    if (ClosePercentage > TargetClosePercentage)
                    {
                        return GripperState.Opening;
                    }
                    else
                    {
                        return GripperState.Closing;
                    }
                }
            }
        }
        #endregion


        public void ShowControlPanel()
        {
#if UNITY_EDITOR || UNITY_EDITOR_BETA

            string _label = State.ToString();
            Color _labelColor;
            switch (State)
            {
                case GripperState.Open:
                    //Green
                    _labelColor = new Color(0f, 0.5f, 0f);
                    break;

                case GripperState.Closed:
                    //Red
                    _labelColor = Color.red;
                    break;

                default:
                    //Orange
                    _labelColor = new Color(0.9f, 0.5f, 0f);
                    break;
            }

            //Create style according to state
            GUIStyle _labelStyle = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold };
            _labelStyle.normal.textColor = _labelColor;

            //Draw State field with toggle button
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("State", _label, _labelStyle);
            if (GUILayout.Button(TargetClosePercentage == 1f ? "Open" : "Close"))
            {
                //Toggle 0 and 1
                TargetClosePercentage = (TargetClosePercentage + 1f) % 2f;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("Current Close percentage", ClosePercentage.ToString());
#endif
        }


        [Serializable]
        public class FingerSetting
        {
            public GripperFinger Finger;
            public bool Inverted = false;
        }

    }
}