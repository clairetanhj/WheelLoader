#if UNITY_EDITOR || UNITY_EDITOR_BETA
using UnityEngine;
using UnityEditor;
using u040.prespective.utility.editor;
using u040.prespective.prepair.inspector;
using System.Reflection;
using u040.prespective.prepair.kinematics;

namespace u040.prespective.referenceobjects.kinetics.motor.steppermotor.editor
{
    [ObfuscationAttribute(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    [CustomEditor(typeof(DrivenStepperMotor))]
    public class DrivenStepperMotorEditor : PrespectiveEditor
    {
        private DrivenStepperMotor component;
        private SerializedObject soTarget;
        private SerializedProperty toolbarTab;

        private void OnEnable()
        {
            component = (DrivenStepperMotor)target;
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
                    EditorGUILayout.LabelField("Preferred Velocity (deg/s)", component.PreferredVelocity.ToString());
                    EditorGUILayout.LabelField("Target Step", component.TargetStep.ToString());
                    EditorGUILayout.LabelField("Continuous", component.Continuous.ToString());
                    EditorGUILayout.LabelField("Continuous Direction", component.ContinuousDirection.ToString());

                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("Velocity (deg/s)", component.Velocity.ToString());
                    EditorGUILayout.LabelField("Position (deg)", component.Position.ToString());
                    EditorGUILayout.LabelField("Position (step)", component.PositionSteps.ToString());
                    GUIStyle _labelStyle = new GUIStyle(GUI.skin.label);
                    _labelStyle.normal.textColor = (component.IsActive && !component.Error) ? new Color(0f, 0.5f, 0f) : Color.red;
                    _labelStyle.fontStyle = FontStyle.Bold;
                    EditorGUILayout.LabelField("State", component.Error ? "Error" : (component.IsActive ? "Active" : "Inactive"), _labelStyle);
                    break;

                case 1:
                    EditorGUI.BeginDisabledGroup(Application.isPlaying);
                    component.WheelJoint = (WheelJoint)EditorGUILayout.ObjectField("Wheel Joint", component.WheelJoint, typeof(WheelJoint), true);
                    component.StepsPerCycle = EditorGUILayout.DelayedIntField("Step Count", component.StepsPerCycle);
                    component.MaxVelocity = EditorGUILayout.DelayedFloatField("Max Velocity (deg/s)", component.MaxVelocity);
                    component.Acceleration = EditorGUILayout.DelayedFloatField("Acceleration (deg/s/s)", component.Acceleration);
                    component.ZeroOffset = EditorGUILayout.DelayedFloatField("Zero Offset (deg)", component.ZeroOffset);
                    if (GUILayout.Button("Set current position as zero offset"))
                    {
                        component.ZeroOffset = component.WheelJoint.CurrentRevolutionDegrees;
                    }

                    EditorGUI.EndDisabledGroup();
                    break;

                case 2:
                    EditorGUILayout.Space();
                    ControlPanelInterface.ShowGenerationButtonForComponent(component);
                    EditorGUILayout.Space();
                    break;
            }

            if (EditorGUI.EndChangeCheck())
            {
                soTarget.ApplyModifiedProperties(); 
            }
            base.OnInspectorGUI();
        }
    }
}
#endif