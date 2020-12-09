#if UNITY_EDITOR || UNITY_EDITOR_BETA
using UnityEditor;
using u040.prespective.utility.editor;
#endif
using System.Collections.Generic;
using System.Reflection;
using u040.prespective.prepair.inspector;
using UnityEngine;
using u040.prespective.utility;
using u040.prespective.core;
using u040.prespective.prepair.components.sensors;


namespace u040.prespective.referenceobjects.sensors.proximitysensor
{
    public class ProximitySensor : QuantitativeSensor, IControlPanel
    {
#pragma warning disable 0414
        [SerializeField] [Obfuscation] private int toolbarTab;
#pragma warning restore 0414
        [SerializeField] [Obfuscation(Exclude = true)] private List<Collider> colliderList = new List<Collider>();
        [SerializeField] [Obfuscation(Exclude = true)] private List<Collider> triggerList = new List<Collider>();
        public List<Collider> TriggerList
        {
            get { return this.triggerList; }
            private set { this.triggerList = value; }
        }
        public bool GenerateTriggerRigidbodies = true;

        private void Awake()
        {
            //Create trigger linkage with each collider
            for (int _i = TriggerList.Count - 1; _i >= 0; _i--)
            {
                if (TriggerList[_i] != null && TriggerList[_i].isTrigger)
                {
                    if (this.GenerateTriggerRigidbodies)
                    {
                        Rigidbody _rb = TriggerList[_i].gameObject.GetComponent<Rigidbody>();
                        if (_rb == null)
                        {
                            _rb = TriggerList[_i].gameObject.AddComponent<Rigidbody>();
                        }
                        _rb.useGravity = false;
                        _rb.isKinematic = true;
                    }

                    LocalEventLink _link = TriggerList[_i].gameObject.AddComponent<LocalEventLink>();
                    _link.Listener = this;
                    _link.TriggerEnter = addCollider;
                    _link.TriggerExit = removeCollider;
                } 
                else
                { 
                    TriggerList.RemoveAt(_i);
                }
            }
        }

        private void addCollider(Collider _collider)
        {
            List<Transform> _transformList = TypedList.GetAllTypedComponents<Transform>(this.transform);
            if (!_transformList.Contains(_collider.transform))
            {
                this.colliderList.Add(_collider);
            }
            updateFlag();
        }

        private void removeCollider(Collider _collider)
        {
            this.colliderList.Remove(_collider);
            updateFlag();
        }

        private void updateFlag()
        {
            Flagged = colliderList.Count > 0;
        }

        private void FixedUpdate()
        {
            if (colliderList.RemoveAll(_entry => _entry == null) > 0)
            {
                updateFlag();
            }
        }

        /// <summary>
        /// Remove a certain Trigger collider from the ProximitySensor
        /// </summary>
        /// <param name="_collider"></param>
        /// <returns></returns>
        public bool RemoveTrigger(Collider _collider)
        {
            return TriggerList.Remove(_collider);
        }

        /// <summary>
        /// Add a certain Trigger collider to the ProximitySensor. Collider must be a trigger unique to the list.
        /// </summary>
        /// <param name="_collider"></param>
        /// <returns></returns>
        public bool AddTrigger(Collider _collider)
        {
            if (!_collider.isTrigger)
            {
                Debug.LogWarning("Cannot add " + _collider.name + " because it is not a trigger");
                return false;
            }
            else if (TriggerList.Contains(_collider))
            {
                Debug.LogWarning("Cannot add " + _collider.name + " because it has already been assigned as a trigger");
                return false;
            }
            else
            {
                TriggerList.Add(_collider);
                return true;
            }
        }

        public void ShowControlPanel()
        {
#if UNITY_EDITOR || UNITY_EDITOR_BETA
            //Create a style for Active or Inactive
            GUIStyle _labelStyle = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold };
            _labelStyle.normal.textColor = IsActive ? new Color(0f, 0.5f, 0f) : Color.red;

            //Draw Active field with toggle button
            EditorGUILayout.BeginHorizontal();
            PrespectiveEditor.BoolLabel("State", IsActive, "Active", "Inactive", _labelStyle);
            if (GUILayout.Button(IsActive ? "Disable" : "Enable"))
            {
                IsActive = !IsActive;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.LabelField("Output Signal", OutputSignal ? "High" : "Low");
#endif
        }

    }
}