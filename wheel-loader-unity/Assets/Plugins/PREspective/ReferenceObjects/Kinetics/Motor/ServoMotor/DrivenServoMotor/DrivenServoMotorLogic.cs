using System;
using System.Collections.Generic;
using System.Reflection;
using u040.prespective.prelogic;
using u040.prespective.prelogic.component;
using u040.prespective.prelogic.signal;
using u040.prespective.prepair;
using u040.prespective.prepair.physics.kinetics.motor;
using UnityEngine;

namespace u040.prespective.referenceobjects.kinetics.motor.servomotor
{
    public class DrivenServoMotorLogic : PreLogicComponent
    {
#pragma warning disable 0414
        [SerializeField] [Obfuscation] private int toolbarTab;
#pragma warning restore 0414

        public DrivenServoMotor ServoMotor;

        //Inputs
        public float iVelocity = 0f;
        public float iPosition = 0f;
        public bool iError = false;

        //Outputs
        public float oPreferredVelocity = 0f;
        public float oTarget = 0f;
        public bool oContinuous = false;
        public bool oDirection = true;
        public bool oIsActive = false;
        public bool oResetError = false;

        #region <<PLC Signals>>
        #region <<Signal Definitions>>
        /// <summary>
        /// Declare the IO signals
        /// </summary>
        public override List<SignalDefinition> SignalDefinitions
        {
            get
            {
                return new List<SignalDefinition>() {
                    //Input Only
                    new SignalDefinition("iVelocity", PLCSignalDirection.INPUT, SupportedSignalType.REAL32, _xmlNote: "Velocity (deg/s)", _baseValue: 0f),
                    new SignalDefinition("iPosition", PLCSignalDirection.INPUT, SupportedSignalType.REAL32, _xmlNote: "Position (deg)", _baseValue: 0f),
                    new SignalDefinition("iError", PLCSignalDirection.INPUT, SupportedSignalType.BOOL, _xmlNote: "Error State", _baseValue: false),

                    //Outputs only
                    new SignalDefinition("oPreferredVelocity", PLCSignalDirection.OUTPUT, SupportedSignalType.REAL32, _xmlNote: "Preferred Velocity (deg/s)", _onValueChange: onSignalChanged, _baseValue: 0f),
                    new SignalDefinition("oTarget", PLCSignalDirection.OUTPUT, SupportedSignalType.REAL32, _xmlNote: "Target (deg)", _onValueChange: onSignalChanged, _baseValue: 0f),
                    new SignalDefinition("oContinuous", PLCSignalDirection.OUTPUT, SupportedSignalType.BOOL, _xmlNote: "Continuous Rotation", _onValueChange: onSignalChanged, _baseValue: false),
                    new SignalDefinition("oDirection", PLCSignalDirection.OUTPUT, SupportedSignalType.BOOL, _xmlNote: "Continuous Direction (true = CW, false = CCW)", _onValueChange: onSignalChanged, _baseValue: true),
                    new SignalDefinition("oIsActive", PLCSignalDirection.OUTPUT, SupportedSignalType.BOOL, _xmlNote: "Is Active", _onValueChange: onSignalChanged, _baseValue: false),
                    new SignalDefinition("oResetError", PLCSignalDirection.OUTPUT, SupportedSignalType.BOOL, _xmlNote: "Reset Error", _onValueChange: onSignalChanged, _baseValue: false),
                };
            }
        }
        #endregion
        #region <<PLC Outputs>>
        /// <summary>
        /// General callback for the IOs
        /// </summary>
        /// <param name="_signal">the signal that has changed</param>
        /// <param name="_newValue">the new value</param>
        /// <param name="_newValueReceived">the time of the value change</param>
        /// <param name="_oldValue">the old value</param>
        /// <param name="_oldValueReceived">the time of the old value change</param>
        void onSignalChanged(SignalInstance _signal, object _newValue, DateTime _newValueReceived, object _oldValue, DateTime _oldValueReceived)
        {
            switch (_signal.definition.defaultSignalName)
            {
                case "oPreferredVelocity":
                    oPreferredVelocity = (float)_newValue;
                    ServoMotor.PreferredVelocity = oPreferredVelocity;
                    break;

                case "oTarget":
                    oTarget = (float)_newValue;
                    ServoMotor.TargetAngle = oTarget;
                    break;

                case "oContinuous":
                    oContinuous = (bool)_newValue;
                    ServoMotor.Continuous = oContinuous;
                    break;

                case "oDirection":
                    oDirection = (bool)_newValue;
                    ServoMotor.ContinuousDirection = oDirection ? DrivenMotor.Direction.CW : DrivenMotor.Direction.CCW;
                    break;

                case "oIsActive":
                    oIsActive = (bool)_newValue;
                    ServoMotor.IsActive = oIsActive;
                    break;

                case "oResetError":
                    oResetError = (bool)_newValue;
                    if (oResetError)
                    {
                        ServoMotor.ResetError();
                    }
                    break;

                default:
                    Debug.LogWarning("Unrecognized PLC output registered");
                    break;
            }
        }
        #endregion
        #endregion

        #region <<Update>>
        /// <summary>
        /// update the simulation component
        /// </summary>
        /// <param name="_simFrame">the current frame since start</param>
        /// <param name="_deltaTime">the time since last frame</param>
        /// <param name="_totalSimRunTime">total run time of the simulation</param>
        /// <param name="_simStart">the time the simulation started</param>
        protected override void onSimulatorUpdated(
            int _simFrame,
            float _deltaTime,
            float _totalSimRunTime,
            DateTime _simStart)
        {
            if (ServoMotor != null)
            {
                readComponent();
            }
            else
            {
                Debug.LogWarning("No " + typeof(DrivenServoMotor).Name + " has been assigned.");
            }
        }


        void readComponent()
        {
            if (ServoMotor.Velocity != iVelocity)
            {
                iVelocity = ServoMotor.Velocity;
                WriteValue("iVelocity", iVelocity);
            }

            if (ServoMotor.Position != iPosition)
            {
                iPosition = ServoMotor.Position;
                WriteValue("iPosition", iPosition);
            }

            if (ServoMotor.Error != iError)
            {
                iError = ServoMotor.Error;
                WriteValue("iError", iError);
            }
        }
        #endregion
    }
}