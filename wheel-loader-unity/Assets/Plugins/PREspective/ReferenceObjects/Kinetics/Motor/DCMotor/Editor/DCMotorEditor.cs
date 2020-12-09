#if UNITY_EDITOR || UNITY_EDITOR_BETA
using UnityEngine;
using UnityEditor;
using u040.prespective.prepair.kinematics;
using u040.prespective.utility.editor;
using System.Reflection;
using u040.prespective.prepair.inspector;

namespace u040.prespective.referenceobjects.kinetics.motor.dcmotor.editor
{
    [ObfuscationAttribute(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    [CustomEditor(typeof(DCMotor))]
    public class DCMotorEditor : PrespectiveEditor
    {
        private DCMotor component;
        private SerializedObject soTarget;
        private SerializedProperty toolbarTab;

        private void OnEnable()
        {
            component = (DCMotor)target;
            soTarget = new SerializedObject(target);
            toolbarTab = soTarget.FindProperty("toolbarTab");
        }

        public override void OnInspectorGUI()
        {
            soTarget.Update();
            //DrawDefaultInspector();


            if (component.WheelJoint == null)
            {
                EditorGUILayout.HelpBox("No Wheel Joint has been set. This component will not function properly untill all required components have been assigned. You can do this in the Properties tab.", MessageType.Warning);
            }

            EditorGUI.BeginChangeCheck();
            toolbarTab.intValue = GUILayout.Toolbar(toolbarTab.intValue, new string[] { "Live Data", "Properties", "Control Panel" });

            switch (toolbarTab.intValue)
            {
                case 0:
                    ////ReadOnly values
                    EditorGUILayout.LabelField("Target Velocity (deg/s)", component.TargetVelocity.ToString());
                    EditorGUILayout.LabelField("Velocity (deg/s)", component.Velocity.ToString());
                    break;

                case 1:
                    if (Application.isPlaying) //Make sure motor physical properties cannot be editted during playmode
                    {
                        if (component.WheelJoint == null)
                        {
                            EditorGUILayout.LabelField("No target wheel joint has been set so no properties can be shown.");
                        }
                        else
                        {
                            EditorGUILayout.LabelField("Target Wheel Joint", component.WheelJoint.ToString());
                            EditorGUILayout.LabelField("Maximum angular velocity (deg/s)", component.MaxVelocity.ToString());
                            EditorGUILayout.LabelField("Angular Acceleration (deg/s/s)", component.Acceleration.ToString());
                        }
                    }
                    else
                    {
                        component.WheelJoint = (WheelJoint)EditorGUILayout.ObjectField("Wheel Joint", component.WheelJoint, typeof(WheelJoint), true);
                        component.MaxVelocity = EditorGUILayout.DelayedFloatField("Maximum Velocity (deg/s)", component.MaxVelocity);
                        component.Acceleration = EditorGUILayout.DelayedFloatField("Angular Acceleration (deg/s/s)", component.Acceleration);
                    }

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