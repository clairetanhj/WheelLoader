using System;
using System.Reflection;
using u040.prespective.prepair.inspector;
using u040.prespective.prepair.kinematics;
using u040.prespective.utility;
using UnityEngine;
#if UNITY_EDITOR || UNITY_EDITOR_BETA
using UnityEditor;
#endif

namespace u040.prespective.referenceobjects.kinetics.motor.linearactuator
{
    public class LinearActuator : MonoBehaviour, IControlPanel
    {
#pragma warning disable 0414
        [SerializeField] [Obfuscation] private int toolbarTab;
#pragma warning restore 0414

        public PrismaticJoint PrismaticJoint;

        public float Target = 0f;
        public bool InvertPosition = false;

        [SerializeField] [Obfuscation(Exclude = true)] private float extendingCycleTime = 1f;
        public float ExtendingCycleTime
        {
            get
            {
                //get actual time by dividing spline length by move speed
                if (this.PrismaticJoint && this.PrismaticJoint.ConstrainingSpline)
                {
                    this.extendingCycleTime = this.PrismaticJoint.ConstrainingSpline.SplineLength / ExtendingMoveSpeed;
                }

                //return value
                return extendingCycleTime;
            }
            set
            {
                //If value has changed and is positive
                if (this.extendingCycleTime != value && value > 0f)
                {
                    //set value
                    this.extendingCycleTime = value;
                    
                    //If we have a spline set, set the MoveSpeed accordingly
                    if (this.PrismaticJoint && this.PrismaticJoint.ConstrainingSpline)
                    {
                        ExtendingMoveSpeed = this.PrismaticJoint.ConstrainingSpline.SplineLength / value;
                    }
                }
            }
        }


        [SerializeField] [Obfuscation(Exclude = true)] private float extendingMoveSpeed = 1f;
        public float ExtendingMoveSpeed
        {
            get
            {
                return this.extendingMoveSpeed;
            }
            set
            {
                //If value has changed and is positive
                if (this.extendingMoveSpeed != value && value > 0f)
                {
                    //Set value
                    this.extendingMoveSpeed = value;
                }
            }
        }

        [SerializeField] [Obfuscation(Exclude = true)] private float retractingCycleTime = 1f;
        public float RetractingCycleTime
        {
            get
            {
                //get actual time by dividing spline length by move speed
                if (this.PrismaticJoint && this.PrismaticJoint.ConstrainingSpline)
                {
                    this.retractingCycleTime = this.PrismaticJoint.ConstrainingSpline.SplineLength / RetractingMoveSpeed;
                }

                //return value
                return retractingCycleTime;
            }
            set
            {
                //If value has changed and is positive
                if (this.retractingCycleTime != value && value > 0f)
                {
                    //set value
                    this.retractingCycleTime = value;

                    //If we have a spline set, set the MoveSpeed accordingly
                    if (this.PrismaticJoint && this.PrismaticJoint.ConstrainingSpline)
                    {
                        RetractingMoveSpeed = this.PrismaticJoint.ConstrainingSpline.SplineLength / value;
                    }
                }
            }
        }


        [SerializeField] [Obfuscation(Exclude = true)] private float retractingMoveSpeed = 1f;
        public float RetractingMoveSpeed
        {
            get
            {
                return this.retractingMoveSpeed;
            }
            set
            {
                //If value has changed and is positive
                if (this.retractingMoveSpeed != value && value > 0f)
                {
                    //Set value
                    this.retractingMoveSpeed = value;
                }
            }
        }

        public float Position
        {
            get
            {
                if (PrismaticJoint)
                {
                    if(InvertPosition)
                    {
                        return 1f - PrismaticJoint.CurrentPerc;
                    }
                    return PrismaticJoint.CurrentPerc;
                }
                else
                {
                    return 0f;
                }
            }
            set
            {
                if (Position != value)
                {
                    setPosition(value);
                }
            }
        }
            

        private void Reset()
        {
            this.PrismaticJoint = this.RequireComponent<PrismaticJoint>(true);
        }

        private void FixedUpdate()
        {
            if (this.PrismaticJoint != null)
            {
                if (Position != Target)
                {
                    float _moveSpeed = (Target - Position > 0) ? ExtendingMoveSpeed : RetractingMoveSpeed;

                    float _speedPercentage = (_moveSpeed * Time.deltaTime) / this.PrismaticJoint.ConstrainingSpline.SplineLength;
                    float _travelDistance = Mathf.Clamp(Target - Position, -_speedPercentage, _speedPercentage);
                    setPosition(Position + _travelDistance);
                }
            }
        }

        private void setPosition(float _percentage)
        {
            float _move = _percentage - Position;
            if (InvertPosition)
            {
                _move *= -1f;
            }

            this.PrismaticJoint.KinematicTranslation(_move, VectorSpace.LocalParent, new Action<float, Vector3>((float _completedPercentage, Vector3 _completedTranslation) => { /*CALLBACK HERE*/ })); //apply step
        }

        public void ShowControlPanel()
        {
#if UNITY_EDITOR || UNITY_EDITOR_BETA
            Target = EditorGUILayout.Slider("Target (%)", Target, 0f, 1f);
            EditorGUILayout.LabelField("Position (%)", Position.ToString());

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Go to Zero"))
            {
                Target = 0f;
            }
            if (GUILayout.Button("Go To End"))
            {
                Target = 1f;
            }
            EditorGUILayout.EndHorizontal();
#endif
        }
    }
}
