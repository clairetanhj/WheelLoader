using u040.prespective.prepair.kinematics;
using u040.prespective.prepair.ui.buttons;
using u040.prespective.utility;
using UnityEngine;

namespace u040.prespective.referenceobjects.userinterface.buttons.encoders
{
    public class RotaryEncoder : BaseEncoder
    {
        public WheelJoint WheelJoint;

        private void Reset()
        {
            WheelJoint = this.RequireComponent<WheelJoint>();
        }

        protected override void FixedUpdate()
        {
            if (WheelJoint == null)
            {
                Debug.LogError("Cannot function without a WheelJoint assigned.");
                return;
            }

            updateValue(this.WheelJoint.CurrentRevolutionPercentage);
        }
    }
}