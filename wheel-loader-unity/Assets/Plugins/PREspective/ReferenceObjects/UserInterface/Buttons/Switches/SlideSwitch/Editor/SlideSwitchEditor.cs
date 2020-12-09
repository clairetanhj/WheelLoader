#if UNITY_EDITOR || UNITY_EDITOR_BETA
using UnityEngine;
using UnityEditor;
using u040.prespective.utility.editor;
using u040.prespective.prepair.kinematics;
using u040.prespective.prepair.inspector;
using System.Reflection;
using u040.prespective.math;

namespace u040.prespective.referenceobjects.userinterface.buttons.switches.editor
{
    [ObfuscationAttribute(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    [CustomEditor(typeof(SlideSwitch))]
    public class SlideSwitchEditor : PrespectiveEditor
    {
        private SlideSwitch component;
        private SerializedObject soTarget;
        private SerializedProperty toolbarTab;

        private void OnEnable()
        {
            component = (SlideSwitch)target;
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
                    if (Application.isPlaying && component.SelectedState != null )
                    {
                        string _stateName = (component.SelectedState.Name != null && component.SelectedState.Name != "") ? component.SelectedState.Name : "State";
                        int _id = component.SelectedState.Id;
                        _label = "[" + _id + "] - " + _stateName;
                    }
                    EditorGUILayout.LabelField("Selected State", _label);

                    break;

                case 1:
                    EditorGUI.BeginDisabledGroup(Application.isPlaying);
                    component.PrismaticJoint = (PrismaticJoint)EditorGUILayout.ObjectField("Prismatic Joint", component.PrismaticJoint, typeof(PrismaticJoint), true);
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
                        SlideSwitch.SwitchState _switchState = component.SwitchStates[_i];

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
                            float _newPosition = PreSpectiveMath.LimitMinMax(EditorGUILayout.DelayedFloatField("Position", _switchState.Position), 0f, 1f);

                            if (component.LoopingSwitch) { _newPosition = _newPosition % 1f; }

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


                    //State save button
                    EditorGUI.indentLevel--;
                    EditorGUILayout.Space();
                    if (!Application.isPlaying && GUILayout.Button("Add new state"))
                    {
                        if (!component.SaveCurrentPositionAsState())
                        {
                            Debug.LogWarning("Cannot save current position as state." + (component.PrismaticJoint == null ? " Assign a PrismaticJoint first." : ""));
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
            if (component.UseSceneGizmo && component.PrismaticJoint && component.PrismaticJoint.ConstrainingSpline)
            {
                Handles.color = component.GizmoColor;
                float _handleSize = HandleUtility.GetHandleSize(component.PrismaticJoint.ConstrainingSpline.transform.position);


                if (component.SwitchStates.Count > 0)
                {                
                    //Draw the first previous transition point manually
                    float _firstTransition = component.SwitchStates[0].LowerTransition;
                    Vector3 _firstTransitionPoint = component.PrismaticJoint.ConstrainingSpline.GetPointAtEquidistantPerc(_firstTransition);
                    Handles.SphereHandleCap(0, _firstTransitionPoint, Quaternion.identity, _handleSize * 0.10f, EventType.Repaint);

                    for (int _i = 0; _i < component.SwitchStates.Count; _i++)
                    {
                        //Draw the position of each switch state
                        SlideSwitch.SwitchState _state = component.SwitchStates[_i];
                        float _position = _state.Position;
                        Vector3 _pointPosition = component.PrismaticJoint.ConstrainingSpline.GetPointAtEquidistantPerc(_position);
                        Handles.SphereHandleCap(0, _pointPosition, Quaternion.identity, _handleSize * 0.25f, EventType.Repaint);

                        //Draw all next transition dots for each state
                        float _transitionPoint = _state.UpperTransition;
                        Vector3 _transitionPointPosition = component.PrismaticJoint.ConstrainingSpline.GetPointAtEquidistantPerc(_transitionPoint);
                        Handles.SphereHandleCap(0, _transitionPointPosition, Quaternion.identity, _handleSize * 0.10f, EventType.Repaint);
                    }
                }


                //Draw a dot on current prismatic position
                Handles.color = new Color(1f - component.GizmoColor.r, 1f - component.GizmoColor.g, 1f - component.GizmoColor.b); //Inverted gizmo color
                float _prismaticPosition = component.PrismaticJoint.CurrentPerc;
                Vector3 _prismaticPoint = component.PrismaticJoint.ConstrainingSpline.GetPointAtEquidistantPerc(_prismaticPosition);
                Handles.SphereHandleCap(0, _prismaticPoint, Quaternion.identity, _handleSize * 0.20f, EventType.Repaint);
            }
        }
    }
}
#endif