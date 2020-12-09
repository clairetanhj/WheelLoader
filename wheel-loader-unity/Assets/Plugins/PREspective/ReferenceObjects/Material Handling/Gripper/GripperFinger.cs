#if UNITY_EDITOR || UNITY_EDITOR_BETA
using UnityEditor;
#endif
using System.Collections.Generic;
using System.Reflection;
using u040.prespective.core;
using UnityEngine;
using u040.prespective.utility;
using u040.prespective.prepair.kinematics;


namespace u040.prespective.referenceobjects.materialhandling.gripper
{
    public abstract class GripperFinger : MonoBehaviour
    {
        public abstract KinematicTransform KinematicTransform
        {
            get; set;
        }

        protected List<GameObject> detectedObjects = new List<GameObject>();
        public List<GameObject> DetectedObjects
        {
            get
            {
                //Remove all null values from the list, if any
                detectedObjects.RemoveAll((_entry) => _entry == null);

                //Return the list 
                return detectedObjects;
            }
        }

        [SerializeField] [Obfuscation(Exclude = true)] private Collider trigger;
        public Collider Trigger
        {
            get { return this.trigger; }
            set
            {
                if (value != this.trigger)
                {
                    if (!value || value.isTrigger)
                    {
                        this.trigger = value;
                    }
                    else
                    {
                        Debug.LogWarning("Cannot assign " + value.name + " as trigger since it is not setup as a trigger.");
                    }
                }
            }
        }

        /// <summary>
        /// Add a Rigidbody to Trigger object upon starting playmode
        /// </summary>
        public bool GenerateRigidbody = false;

        private void Awake()
        {
            //Make sure colliders are setup properly
            Trigger.isTrigger = true;
            //Collider.isTrigger = false;

            //Create a local event link to pass on the OntriggerStay if the trigger is not on the same gameobject
            LocalEventLink _link = LocalEventLink.Create(Trigger.gameObject, this);
            _link.TriggerEnter = onObjectDetected;
            _link.TriggerExit = onObjectLost;

            //Generate a rigidbody on the trigger object if necessary
            if (GenerateRigidbody)
            {
                Rigidbody _rigidbody = Trigger.RequireComponent<Rigidbody>(false);
                _rigidbody.useGravity = false;
                _rigidbody.isKinematic = true;
            }
        }

        protected virtual void onObjectDetected(Collider _collider)
        {
            //If the list does not already contain the GameObject
            if (!detectedObjects.Contains(_collider.gameObject))
            {
                //Add it to the list
                detectedObjects.Add(_collider.gameObject);
            }
        }

        protected virtual void onObjectLost(Collider _collider)
        {
            detectedObjects.Remove(_collider.gameObject);
        }

        protected void fingerJammedCallback(float _percentage)
        {
            //To prevent FPE, callback percentage is allowed to divert a tiny bit from 1f
            if (_percentage < 0.999f || _percentage > 1.001f)
            {
                Debug.LogError(this.name + " cannot move to intended position. Moved " + (_percentage * 100f) + "% of intended move.");
            }
        }

        public abstract void SetPosition(float _percentage);

        public void ShowBaseInspector()
        {
#if UNITY_EDITOR || UNITY_EDITOR_BETA
            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            Trigger = (Collider)EditorGUILayout.ObjectField("Trigger", this.Trigger, typeof(Collider), true);
            GenerateRigidbody = EditorGUILayout.Toggle("Generate Trigger Rigidbody", GenerateRigidbody);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.Space();

            List<GameObject> _detectedObjectsList = this.DetectedObjects;
            int _objectCount = _detectedObjectsList.Count;
            EditorGUILayout.LabelField("Detected Objects (" + _objectCount + ")", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            if (_objectCount == 0)
            {
                EditorGUILayout.LabelField("No detected objects", new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Italic });
            }
            else
            {
                for (int _i = 0; _i < _objectCount; _i++)
                {
                    //Readonly field for object
                    EditorGUILayout.ObjectField(_detectedObjectsList[_i], typeof(GameObject), false);
                }
            }
            EditorGUI.indentLevel--;
#endif
        }
    }
}