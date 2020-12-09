#if UNITY_EDITOR || UNITY_EDITOR_BETA
using UnityEngine;
using UnityEditor;
using u040.prespective.utility.editor;
using u040.prespective.prepair.inspector;
using System.Reflection;
using u040.prespective.prepair.kinematics;

namespace u040.prespective.referenceobjects.kinetics.motor.servomotor.editor
{
    [ObfuscationAttribute(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    [CustomEditor(typeof(LimitedServoMotor))]
    public class LimitedServoMotorEditor : PrespectiveEditor
    {
        private LimitedServoMotor component;
        private SerializedObject soTarget;
        private SerializedProperty toolbarTab;

        private void OnEnable()
        {
            component = (LimitedServoMotor)target;
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
                    EditorGUILayout.LabelField("Target (deg)", component.Target.ToString());
                    if (component.EnablePWM)
                    {
                        EditorGUILayout.LabelField("Pulse Width (ms)", component.PulseWidth.ToString());
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Position (deg)", component.Position.ToString());
                    break;

                case 1:
                    EditorGUI.BeginDisabledGroup(Application.isPlaying);
                    this.component.WheelJoint = (WheelJoint)EditorGUILayout.ObjectField("Wheel Joint", component.WheelJoint, typeof(WheelJoint), true);
                    component.RotationRange = (ContinuousServoMotor.Range)EditorGUILayout.EnumPopup("Rotation Range", component.RotationRange);
                    component.SecondsPer60Degrees = EditorGUILayout.DelayedFloatField("Seconds per 60 degrees", component.SecondsPer60Degrees);
                    component.Damping = EditorGUILayout.DelayedFloatField("Damping", component.Damping);
                    component.DeadAngle = EditorGUILayout.DelayedFloatField("Dead Angle (deg)", component.DeadAngle);

                    EditorGUILayout.Space();

                    component.EnablePWM = EditorGUILayout.Toggle("Enable PWM", component.EnablePWM);
                    if (component.EnablePWM)
                    {
                        EditorGUI.indentLevel++;
                        component.PulseWidthDefinition.x = EditorGUILayout.DelayedFloatField("0 deg Pulse Width (ms)", component.PulseWidthDefinition.x);
                        component.PulseWidthDefinition.y = EditorGUILayout.DelayedFloatField((float)component.RotationRange + " deg Pulse Width (ms)", component.PulseWidthDefinition.y);
                        component.DeadBandWidth = EditorGUILayout.DelayedFloatField("Dead Band Width (ms)", component.DeadBandWidth);
                        EditorGUI.indentLevel--;
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