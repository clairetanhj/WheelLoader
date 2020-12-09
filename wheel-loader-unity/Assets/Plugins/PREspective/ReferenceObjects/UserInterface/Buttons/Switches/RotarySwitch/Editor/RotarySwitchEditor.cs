#if UNITY_EDITOR || UNITY_EDITOR_BETA
using UnityEngine;
using UnityEditor;
using u040.prespective.utility.editor;
using u040.prespective.prepair.kinematics;
using u040.prespective.math;
using u040.prespective.prepair.inspector;
using System.Reflection;

namespace u040.prespective.referenceobjects.userinterface.buttons.switches.editor
{
    [ObfuscationAttribute(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    [CustomEditor(typeof(RotarySwitch))]
    public class RotarySwitchEditor : PrespectiveEditor
    {
        private RotarySwitch component;
        private SerializedObject soTarget;
        private SerializedProperty toolbarTab;

        private void OnEnable()
        {
            component = (RotarySwitch)target;
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
                    // current state
                    string _label = "N/A";
                    if (Application.isPlaying && component.SelectedState != null)
                    {
                        string _stateName = (component.SelectedState.Name != null && component.SelectedState.Name != "") ? component.SelectedState.Name : "State";
                        int _id = component.SelectedState.Id;
                        _label = "[" + _id + "] - " + _stateName;
                    }
                    EditorGUILayout.LabelField("Selected State", _label);
                    break;

                case 1:
                    EditorGUI.BeginDisabledGroup(Application.isPlaying);
                    component.WheelJoint = (WheelJoint)EditorGUILayout.ObjectField("Wheel Joint", component.WheelJoint, typeof(WheelJoint), true);

                    //Get the serialized property of the switch states list
                    SerializedProperty _serializedSwitchStatesList = soTarget.FindProperty("SwitchStates");

                    EditorGUI.indentLevel++;

                    //Display message of no switch states have been assigned
                    if (component.SwitchStates.Count == 0)
                    {
                        EditorGUILayout.LabelField("No Switch States available", new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Italic });
                    }

                    //For each switch state
                    for (int _i = 0; _i < component.SwitchStates.Count; _i++)
                    {
                        //Buffer switch state
                        RotarySwitch.SwitchState _switchState = component.SwitchStates[_i];

                        //Show foldout with summary of information like ID, name and positon
                        EditorGUILayout.BeginHorizontal();
                        _switchState.Foldout = EditorGUILayout.Foldout(_switchState.Foldout, "[" + _switchState.Id + "] " + _switchState.Name + ": " + _switchState.Position, true);
                        if (GUILayout.Button("Select", new GUIStyle(GUI.skin.button) { stretchWidth = false }))
                        {
                            component.SelectState(_i);
                        }
                        EditorGUILayout.EndHorizontal();

                        //If foldout
                        if (_switchState.Foldout)
                        {
                            //Show Id and Delete buttons next to each other
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("ID", _switchState.Id.ToString());
                            if (GUILayout.Button("Delete", new GUIStyle(GUI.skin.button) { stretchWidth = false }))
                            {
                                Undo.RecordObject(component, "Switch states");
                                component.DeleteState(_i);
                            }
                            EditorGUILayout.EndHorizontal();

                            //Name field
                            _switchState.Name = EditorGUILayout.TextField("Name", _switchState.Name);

                            //Previous Weight
                            float _newPreviousWeight = Mathf.Max(EditorGUILayout.FloatField("Lower Weight", _switchState.LowerWeight), 0f);
                            if (_switchState.LowerWeight != _newPreviousWeight)
                            {
                                _switchState.LowerWeight = _newPreviousWeight;
                                component.RecalculateTransitions();
                            }

                            //Position field
                            float _newPosition = PreSpectiveMath.LimitMinMax(EditorGUILayout.DelayedFloatField("Position", _switchState.Position), 0f, 1f) % 1f;
                            
                            //If position has changed, sort list by position
                            if (_switchState.Position != _newPosition)
                            {
                                _switchState.Position = _newPosition;
                                component.RecalculateTransitions();
                            }

                            //Next Weight
                            float _newNextWeight = Mathf.Max(EditorGUILayout.FloatField("Upper Weight", _switchState.UpperWeight), 0f);
                            if (_switchState.UpperWeight != _newNextWeight)
                            {
                                _switchState.UpperWeight = _newNextWeight;
                                component.RecalculateTransitions();
                            }

                            //Get serialized property of switch state from serialized list
                            SerializedProperty _serializedSwitchState = _serializedSwitchStatesList.GetArrayElementAtIndex(_i);
                            
                            //Get serialized property of UnityEvents from serialized switch state
                            SerializedProperty _serializedOnSelectedEvent = _serializedSwitchState.FindPropertyRelative("OnSelected");
                            SerializedProperty _serializedOnUnselectedEvent = _serializedSwitchState.FindPropertyRelative("OnUnselected");

                            //Display serialized UnityEvent properties
                            EditorGUILayout.PropertyField(_serializedOnSelectedEvent, true);
                            EditorGUILayout.PropertyField(_serializedOnUnselectedEvent, true);
                            EditorGUILayout.Space();
                        }
                    }

                    EditorGUI.indentLevel--;
                    EditorGUILayout.Space();
                    if (!Application.isPlaying && GUILayout.Button("Save current position as state"))
                    {
                        if (!component.SaveCurrentPositionAsState())
                        {
                            Debug.LogWarning("Cannot save current position as state." + (component.WheelJoint == null ? " Assign a WheelJoint first." : ""));
                        }
                    }

                    //Gizmo settings
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                    component.UseSceneGizmo = EditorGUILayout.Toggle("Use Scene Gizmo", component.UseSceneGizmo);
                    component.GizmoColor = EditorGUILayout.ColorField(component.GizmoColor);
                    EditorGUILayout.EndHorizontal();

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

        private void OnSceneGUI()
        {
            if (component.UseSceneGizmo && component.WheelJoint != null)
            {
                //Draw arrows for state positions
                Handles.color = component.GizmoColor;

                //Position or origin
                Vector3 _handleOrigin = component.WheelJoint.transform.position;

                //Size
                float _size = component.WheelJoint.Radius;

                //Direction of origin
                Vector3 _parentForward = component.WheelJoint.transform.parent == null ? Vector3.forward : component.WheelJoint.transform.parent.transform.forward;
                Quaternion _parentRotation = component.WheelJoint.transform.parent == null ? Quaternion.identity : component.WheelJoint.transform.parent.rotation;

                for (int _i = 0; _i < component.SwitchStates.Count; _i++)
                {
                    float _correctionalAngle = Vector3.SignedAngle(_parentForward, component.WheelJoint.ForwardDir.GlobalVector, component.WheelJoint.AxisDir.GlobalVector);
                    Quaternion _handleDirection = Quaternion.AngleAxis((component.SwitchStates[_i].Position * 360f) + _correctionalAngle, component.WheelJoint.AxisDir.GlobalVector) * _parentRotation;

                    //Draw handle
                    Handles.ArrowHandleCap(0, _handleOrigin, _handleDirection, _size * 0.875f, EventType.Repaint);
                    Handles.Label(_handleOrigin + (_handleDirection * (_parentForward * _size)), component.SwitchStates[_i].Name);

                    //Draw transition lines
                    Vector3 _fromPosition = component.WheelJoint.transform.position;                    
                    float _angle = component.SwitchStates[_i].UpperTransition * 360f;
                    Vector3 _toVector = component.WheelJoint.ForwardDir.GlobalVector.normalized * component.WheelJoint.Radius;
                    Quaternion _rotateVector = Quaternion.AngleAxis(_angle, component.WheelJoint.AxisDir.GlobalVector);
                    _toVector = _rotateVector * _toVector;
                    Vector3 _toPosition = _fromPosition + _toVector;
                    Handles.DrawLine(_fromPosition, _toPosition);
                }
            }
        }
    }
}
#endif
