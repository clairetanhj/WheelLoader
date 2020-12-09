#if UNITY_EDITOR || UNITY_EDITOR_BETA
using UnityEditor;
#endif
using u040.prespective.prepair.inspector;
using UnityEngine;
using u040.prespective.prepair.physics.kinetics.motor;

namespace u040.prespective.referenceobjects.kinetics.motor.servomotor
{
    public class DrivenServoMotor : DrivenMotor, IControlPanel
    {
        public void ShowControlPanel()
        {
#if UNITY_EDITOR || UNITY_EDITOR_BETA
            PreferredVelocity = EditorGUILayout.Slider("Preferred Velocity (deg/s)", PreferredVelocity, 0f, MaxVelocity);
            TargetAngle = EditorGUILayout.DelayedFloatField("Target Angle (deg)", TargetAngle);
            Continuous = EditorGUILayout.Toggle("Continuous", Continuous);
            ContinuousDirection = (Direction)EditorGUILayout.EnumPopup("Continuous Direction", ContinuousDirection);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Velocity (deg/s)", Velocity.ToString());
            EditorGUILayout.LabelField("Position (deg)", Position.ToString());
            GUIStyle _labelStyle = new GUIStyle(GUI.skin.label);
            _labelStyle.normal.textColor = (IsActive && !Error) ? new Color(0f, 0.5f, 0f) : Color.red;
            _labelStyle.fontStyle = FontStyle.Bold;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("State", Error ? "Error" : (IsActive ? "Active" : "Inactive"), _labelStyle);
            if (GUILayout.Button(Error? "Reset Error" : (IsActive ? "Stop" : "Start")))
            {
                if (Error)
                {
                    ResetError();
                }
                else
                {
                    IsActive = !IsActive;
                }
                GUI.FocusControl(null);
            }
            EditorGUILayout.EndHorizontal();
#endif
        }
    }
}