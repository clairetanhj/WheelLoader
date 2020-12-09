using u040.prespective.prepair.kinematics;
using u040.prespective.prepair.ui.buttons;
using u040.prespective.utility;
using UnityEngine;

namespace u040.prespective.referenceobjects.userinterface.buttons.encoders
{
    public class LinearEncoder : BaseEncoder
    {
        public PrismaticJoint PrismaticJoint;

        private void Reset()
        {
            PrismaticJoint = this.RequireComponent<PrismaticJoint>();
        }

        protected override void FixedUpdate()
        {
            if (PrismaticJoint == null)
            {
                Debug.LogError("Cannot function without a PrismaticJoint assigned.");
                return;
            }

            updateValue(this.PrismaticJoint.CurrentPerc);
        }
    }
}
