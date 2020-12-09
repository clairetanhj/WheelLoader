#if UNITY_EDITOR || UNITY_EDITOR_BETA
using UnityEngine;
using UnityEditor;
using u040.prespective.utility.editor;
using u040.prespective.prepair.inspector;
using System.Reflection;
using System.Collections.Generic;
using static u040.prespective.referenceobjects.materialhandling.gripper.GripperBase;

namespace u040.prespective.referenceobjects.materialhandling.gripper.editor
{
    [ObfuscationAttribute(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    [CustomEditor(typeof(GripperBase))]
    public class GripperBaseEditor : PrespectiveEditor
    {
        private GripperBase component;
        private SerializedObject soTarget;
        private SerializedProperty toolbarTab;

        private void OnEnable()
        {
            component = (GripperBase)target;
            soTarget = new SerializedObject(target);
            toolbarTab = soTarget.FindProperty("toolbarTab");
        }

        public override void OnInspectorGUI()
        {
            soTarget.Update();
            //DrawDefaultInspector();

            EditorGUI.BeginChangeCheck();
            toolbarTab.intValue = GUILayout.Toolbar(toolbarTab.intValue, new string[] { "Live Data", "Properties", "Control Panel" });

            switch (toolbarTab.intValue)
            {
                case 0:
                    GripperState _state = component.State;
                    string _label = _state.ToString();
                    Color _labelColor;
                    switch (_state)
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

                    //Draw State field
                    EditorGUILayout.LabelField("State", _label, _labelStyle);

                    EditorGUILayout.LabelField("Current Close percentage", component.ClosePercentage.ToString());
                    EditorGUILayout.Space();

                    List<GameObject> _grippedObjects = component.GrippedGameObjects;
                    int _objectCount = _grippedObjects.Count;
                    EditorGUILayout.LabelField("Gripped Objects (" + _objectCount + ")", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;

                    if (_objectCount == 0)
                    {
                        EditorGUILayout.LabelField("No gripped objects", new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Italic });
                    }
                    else
                    {
                        for (int _i = 0; _i < _objectCount; _i++)
                        {
                            //readonly objectfield for object
                            EditorGUILayout.ObjectField(_grippedObjects[_i], typeof(GameObject), false);
                        }
                    }
                    EditorGUI.indentLevel--;
                    break;

                case 1:
                    component.CloseTime = Mathf.Max(EditorGUILayout.FloatField("Close Time (s)", component.CloseTime), 0.01f);
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Fingers (" + component.GripperFingers.Count + ")", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;

                    for (int _i = 0; _i < component.GripperFingers.Count; _i++)
                    {
                        FingerSetting _setting = component.GripperFingers[_i];
                        EditorGUILayout.BeginHorizontal();
                        //readonly objectfield for collider
                        EditorGUILayout.ObjectField(_setting.Finger, typeof(GripperFinger), false);
                        EditorGUIUtility.labelWidth = 50f;
                        _setting.Inverted = EditorGUILayout.Toggle("Invert", _setting.Inverted, GUILayout.ExpandWidth(false));
                        EditorGUIUtility.labelWidth = 0f;
                        if (GUILayout.Button("Delete", new GUIStyle(GUI.skin.button) { stretchWidth = false }))
                        {
                            Undo.RecordObject(component, "Delete finger");
                            component.RemoveFinger(_setting.Finger);
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel++;

                        EditorGUI.indentLevel--;
                    }

                    EditorGUILayout.LabelField("Add Finger", new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Italic });

                    //Add a field to add new Triggers
                    GripperFinger _newFinger = (GripperFinger)EditorGUILayout.ObjectField(null, typeof(GripperFinger), true);
                    if (_newFinger)
                    {
                        component.AddFinger(_newFinger);
                    }
                    EditorGUI.indentLevel--;
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
    }
}
#endif