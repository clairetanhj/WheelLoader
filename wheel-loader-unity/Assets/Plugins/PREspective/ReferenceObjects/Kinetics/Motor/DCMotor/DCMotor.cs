#if UNITY_EDITOR || UNITY_EDITOR_BETA
using UnityEditor;
#endif
using UnityEngine;
using u040.prespective.prepair.inspector;
using u040.prespective.prepair.physics.kinetics.motor;

namespace u040.prespective.referenceobjects.kinetics.motor.dcmotor
{
    public class DCMotor : BaseMotor, IControlPanel
    {
        public void StartRotation()
        {
            if (this.TargetVelocity == 0f)
            {
                this.TargetVelocity = this.MaxVelocity;
            }
        }

        public void StopRotation()
        {
            if (this.TargetVelocity != 0f)
            {
                this.TargetVelocity = 0f;
            }
        }


        public void ShowControlPanel()
        {
#if UNITY_EDITOR || UNITY_EDITOR_BETA
            //Editable values
            TargetVelocity = EditorGUILayout.Slider("Preferred Velocity (deg/s)", TargetVelocity, -MaxVelocity, MaxVelocity);

            //ReadOnly values
            EditorGUILayout.LabelField("Velocity (deg/s)", Velocity.ToString());

            //Buttons
            EditorGUILayout.Space();
            EditorGUI.BeginDisabledGroup(!Application.isPlaying);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Start"))
            {
                StartRotation();
            }
            if (GUILayout.Button("Stop"))
            {
                StopRotation();
            }
            GUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();
            #endif
        }

    }
}