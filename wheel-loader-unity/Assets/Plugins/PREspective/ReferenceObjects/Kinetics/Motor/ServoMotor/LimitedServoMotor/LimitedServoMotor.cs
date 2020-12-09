#if UNITY_EDITOR || UNITY_EDITOR_BETA
using UnityEditor;
#endif
using u040.prespective.prepair.inspector;
using UnityEngine;

namespace u040.prespective.referenceobjects.kinetics.motor.servomotor
{
    public class LimitedServoMotor : ContinuousServoMotor, IControlPanel
    {
        public float Damping = 0.1f;

        private float target
        {
            get
            {
                //Return angle to rotate to, minus the neutral angle which is half the max angle.
                return this.Target - (rangeValue * 0.5f);
            }
        }

        protected override void Reset()
        {
            base.Reset();
            this.SecondsPer60Degrees = 0.1f;
        }


        protected override void updatePreferredVelocity()
        {
            this.TargetVelocity = (target - Position) * (1f / Damping); //FIXME: This *50f is random... Needs to be changed to something with more ground to it.

            //Prevent updates within the deadband
            if (Mathf.Abs(target - Position) <= DeadAngle)
            {
                this.TargetVelocity = 0f;
            }
        }

        public override void ShowControlPanel()
        {
#if UNITY_EDITOR || UNITY_EDITOR_BETA
            Target = EditorGUILayout.DelayedFloatField("Target (deg)", Target);
            if (EnablePWM)
            {
                PulseWidth = EditorGUILayout.DelayedFloatField("Pulse Width (ms)", PulseWidth);
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Position (deg)", Position.ToString());
#endif
        }
    }
}