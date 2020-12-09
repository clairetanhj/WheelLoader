using System;
using u040.prespective.prepair.kinematics;
using u040.prespective.utility;
using UnityEngine;

namespace u040.prespective.referenceobjects.materialhandling.gripper
{
    public class AngularGripperFinger : GripperFinger
    {
        public WheelJoint WheelJoint;

        public override KinematicTransform KinematicTransform
        {
            get
            {
                return this.WheelJoint;
            }
            set
            {
                if (value != WheelJoint)
                {
                    if (value.GetType() == typeof(WheelJoint))
                    {
                        this.WheelJoint = (WheelJoint)value;
                    }
                    else
                    {
                        Debug.LogError("Cannot add " + value.name + " as a KinematicTransform since it is not of type " + typeof(WheelJoint).Name);
                    }
                }
            }
        }

        public float LowerLimit
        {
            get
            {
                if (WheelJoint)
                {
                    return this.WheelJoint.RotationLimitMinMaxDeg.x;
                }
                return 0f;
            }
            set
            {
                if (WheelJoint && LowerLimit != value)
                {
                    Vector2 _newLimits = new Vector2(value, this.WheelJoint.RotationLimitMinMaxDeg.y);
                    this.WheelJoint.RotationLimitMinMaxDeg = _newLimits;
                }
            }
        }
        public float UpperLimit
        {
            get
            {
                if (WheelJoint)
                {
                    return this.WheelJoint.RotationLimitMinMaxDeg.y;
                }
                return 0f;
            }
            set
            {
                if (WheelJoint && UpperLimit != value)
                {
                    Vector2 _newLimits = new Vector2(this.WheelJoint.RotationLimitMinMaxDeg.x, value);
                    this.WheelJoint.RotationLimitMinMaxDeg = _newLimits;
                }
            }
        }

        private float angleOfFreedom
        {
            get
            {
                if (WheelJoint)
                {
                    Vector2 _limits = WheelJoint.RotationLimitMinMaxDeg;
                    return _limits.y - _limits.x;
                }
                return 0f;
            }
        }

        private void Reset()
        {
            this.WheelJoint = this.gameObject.RequireComponent<WheelJoint>(true);
        }

        public override void SetPosition(float _percent)
        {
            if (WheelJoint)
            {
                float _relativeAngle = this.angleOfFreedom * _percent;
                float _absoluteAngle = WheelJoint.RotationLimitMinMaxDeg.x + _relativeAngle;
                float _moveIntent = _absoluteAngle - this.WheelJoint.CurrentRevolutionDegrees;

                WheelJoint.KinematicRotation(_moveIntent, VectorSpace.LocalParent, new Action<float, Quaternion>((float _percentage, Quaternion _rotation) =>
                {
                    fingerJammedCallback(_percentage);
                }));
            }
            else
            {
                Debug.LogError("Cannot set position of " + this.name + " while no WheelJoint has been assigned");
            }
        }
    }
}