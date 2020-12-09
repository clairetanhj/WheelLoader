#if UNITY_EDITOR || UNITY_EDITOR_BETA
using UnityEditor;
#endif
using u040.prespective.prepair.inspector;
using UnityEngine;
using System.Reflection;
using u040.prespective.prepair.physics.kinetics.motor;

namespace u040.prespective.referenceobjects.kinetics.motor.steppermotor
{
    public class DrivenStepperMotor : DrivenMotor, IControlPanel
    {
        public int StepsPerCycle = 200;
        private float stepAngle { get { return 360f / StepsPerCycle; } }

        [SerializeField] [Obfuscation(Exclude = true)] private int targetStep = 0;
        public int TargetStep
        {
            get { return Mathf.RoundToInt(this.TargetAngle / stepAngle); }
            set
            {
                this.targetStep = value;
                this.TargetAngle = this.targetStep * stepAngle;
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            TargetStep = TargetStep; //Round target to steps
        }

        public float PositionDegrees { get { return this.Position; } }
        public int PositionSteps { get { return Mathf.RoundToInt(this.Position / stepAngle); } }

        protected override void goToError()
        {
            base.goToError();
            this.TargetStep = this.PositionSteps;
        }

        public void ShowControlPanel()
        {
#if UNITY_EDITOR || UNITY_EDITOR_BETA
            PreferredVelocity = EditorGUILayout.Slider("Preferred Velocity (deg/s)", PreferredVelocity, 0f, MaxVelocity);
            TargetStep = EditorGUILayout.DelayedIntField("Target Step", TargetStep);
            Continuous = EditorGUILayout.Toggle("Continuous", Continuous);
            ContinuousDirection = (Direction)EditorGUILayout.EnumPopup("Continuous Direction", ContinuousDirection);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Velocity (deg/s)", Velocity.ToString());
            EditorGUILayout.LabelField("Position (deg)", PositionDegrees.ToString());
            EditorGUILayout.LabelField("Position (step)", PositionSteps.ToString());
            GUIStyle _labelStyle = new GUIStyle(GUI.skin.label);
            _labelStyle.normal.textColor = (IsActive && !Error) ? new Color(0f, 0.5f, 0f) : Color.red;
            _labelStyle.fontStyle = FontStyle.Bold;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("State", Error ? "Error" : (IsActive ? "Active" : "Inactive"), _labelStyle);
            if (GUILayout.Button(Error ? "Reset Error" : (IsActive ? "Stop" : "Start")))
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