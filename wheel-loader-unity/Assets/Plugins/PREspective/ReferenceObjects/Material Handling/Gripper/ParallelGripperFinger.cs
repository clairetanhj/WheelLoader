using System;
using u040.prespective.prepair.kinematics;
using u040.prespective.utility;
using UnityEngine;

namespace u040.prespective.referenceobjects.materialhandling.gripper
{
    public class ParallelGripperFinger : GripperFinger
    {
        public PrismaticJoint PrismaticJoint;

        public override KinematicTransform KinematicTransform
        {
            get
            {
                return this.PrismaticJoint;
            }
            set
            {
                if (value != PrismaticJoint)
                {
                    if (value.GetType() == typeof(PrismaticJoint))
                    {
                        this.PrismaticJoint = (PrismaticJoint)value;
                    }
                    else
                    {
                        Debug.LogError("Cannot add " + value.name + " as a KinematicTransform since it is not of type " + typeof(PrismaticJoint).Name);
                    }
                }
            }
        }

        private void Reset()
        {
            this.PrismaticJoint = this.gameObject.RequireComponent<PrismaticJoint>(true);
        }

        public override void SetPosition(float _percent)
        {
            if (PrismaticJoint)
            {
                float _relativePercentageMove = _percent - PrismaticJoint.CurrentPerc;

                PrismaticJoint.KinematicTranslation(_relativePercentageMove, VectorSpace.LocalParent, new Action<float, Vector3>((float _percentage, Vector3 _translation) =>
                {

                    fingerJammedCallback(_percentage);
                }));
            }
            else
            {
                Debug.LogError("Cannot set position of " + this.name + " while no PrismaticJoint has been assigned");
            }
        }
    }
}