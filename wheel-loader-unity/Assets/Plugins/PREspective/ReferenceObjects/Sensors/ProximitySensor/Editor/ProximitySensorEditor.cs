#if UNITY_EDITOR || UNITY_EDITOR_BETA
using UnityEngine;
using UnityEditor;
using u040.prespective.utility.editor;
using u040.prespective.prepair.inspector;
using System.Reflection;
using System;

namespace u040.prespective.referenceobjects.sensors.proximitysensor.editor
{
    [ObfuscationAttribute(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    [CustomEditor(typeof(ProximitySensor))]
    public class ProximitySensorEditor : PrespectiveEditor
    {
        private ProximitySensor component;
        private SerializedObject soTarget;
        private SerializedProperty toolbarTab;
        private SerializedProperty signalLow;
        private SerializedProperty signalHigh;
        private SerializedProperty detectedObjects;


        private void OnEnable()
        {
            component = (ProximitySensor)target;
            soTarget = new SerializedObject(target);
            toolbarTab = soTarget.FindProperty("toolbarTab");
            signalLow = soTarget.FindProperty("onSignalLow");
            signalHigh = soTarget.FindProperty("onSignalHigh");
            detectedObjects = soTarget.FindProperty("colliderList");
        }

        public override void OnInspectorGUI()
        {
            soTarget.Update();
            //DrawDefaultInspector();

            if (!componentHasSuitableColliders())
            {
                EditorGUILayout.HelpBox("No Trigger Colliders have been assigned yet. Without at least one Trigger Collider assigned, this component cannot function properly. You can assign these under the Properties tab.", MessageType.Error);
            }

            EditorGUI.BeginChangeCheck();
            toolbarTab.intValue = GUILayout.Toolbar(toolbarTab.intValue, new string[] { "Live Data", "Properties", "Control Panel" });
                                 
            switch (toolbarTab.intValue)
            {
                case 0:
                    component.ShowQuantitativeSensorValuesInspector(true);

                    EditorGUILayout.Space();

                    int _listCount = detectedObjects.arraySize;
                    EditorGUILayout.LabelField("Detected Objects (" + _listCount + ")", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    
                    if (_listCount == 0)
                    {
                        EditorGUILayout.LabelField("No objects detected", new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Italic });
                    }
                    else
                    {
                        for (int _i = 0; _i < _listCount; _i++)
                        {
                            //Get serialized object from serialized list
                            SerializedProperty _object = detectedObjects.GetArrayElementAtIndex(_i);
                            
                            //Get collider object from serialized object
                            Collider _collider = (Collider)_object.objectReferenceValue;
                            
                            //readonly objectfield for collider
                            EditorGUILayout.ObjectField(_collider, typeof(Collider), false);
                        }
                    }
                    EditorGUI.indentLevel--;

                    break;

                case 1:
                    EditorGUI.BeginDisabledGroup(Application.isPlaying);
                    EditorGUILayout.PropertyField(signalHigh, true);
                    EditorGUILayout.PropertyField(signalLow, true);

                    EditorGUILayout.LabelField("Triggers (" + component.TriggerList.Count + ")", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;

                    //Texture _deleteIcon = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Plugins/PREspective/Icons/Trash_Icon_20x20.png", typeof(Texture2D));

                    for (int _i = 0; _i < component.TriggerList.Count; _i++)
                    {
                        EditorGUILayout.BeginHorizontal();

                        //readonly objectfield for collider
                        EditorGUILayout.ObjectField(component.TriggerList[_i], typeof(Collider), false);

                        //if (GUILayout.Button(new GUIContent(_deleteIcon), new GUIStyle(GUI.skin.button) { stretchWidth = false }))
                        if (GUILayout.Button("Delete", new GUIStyle(GUI.skin.button) { stretchWidth = false }))
                        {
                            Undo.RecordObject(component, "Delete trigger");
                            component.RemoveTrigger(component.TriggerList[_i]);
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.LabelField("Add Trigger", new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Italic });

                    //Add a field to add new Triggers
                    Collider _newCollider = (Collider)EditorGUILayout.ObjectField(null, typeof(Collider), true) ;
                    if (_newCollider)
                    {
                        component.AddTrigger(_newCollider);
                    }
                    EditorGUI.indentLevel--;

                    EditorGUILayout.Space();

                    component.GenerateTriggerRigidbodies = EditorGUILayout.Toggle("Generate Rigidbodies", component.GenerateTriggerRigidbodies);
                    EditorGUI.EndDisabledGroup();
                    break;

                case 2:
                       ControlPanelInterface.ShowGenerationButtonForComponent(component);
                    break;
            }

            EditorUtility.SetDirty(target); //Make sure inspector updates and repaints properly 

            if (EditorGUI.EndChangeCheck())
            {
                soTarget.ApplyModifiedProperties();
            }
            base.OnInspectorGUI();
        }


        private bool componentHasSuitableColliders()
        {
            for (int _i = 0; _i < component.TriggerList.Count; _i++)
            {
                if (component.TriggerList[_i] != null && component.TriggerList[_i].isTrigger)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
#endif