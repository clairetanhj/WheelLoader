using System;
using System.Collections.Generic;
using System.Reflection;
using u040.prespective.prelogic;
using u040.prespective.prelogic.component;
using u040.prespective.prelogic.signal;
using UnityEngine;

namespace u040.prespective.referenceobjects.kinetics.motor.servomotor
{
    public class LimitedServoMotorLogic : PreLogicComponent
    {
#pragma warning disable 0414
        [SerializeField] [Obfuscation] private int toolbarTab;
#pragma warning restore 0414

        public LimitedServoMotor ServoMotor;

        //Inputs
        public float iPosition = 0f;

        //Outputs
        public float oTarget = 0f;
        public float oPulseWidth = 1f;


        private void Reset()
        {
            this.implicitNamingRule.instanceNameRule = "GVLs." + this.GetType().Name + "[{{INDEX_IN_PARENT}}]";
        }


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
                    ////Input Only
                    new SignalDefinition("iPosition", PLCSignalDirection.INPUT, SupportedSignalType.REAL32, "", "Position (deg)", null, null, 0f),

                    ////Outputs only
                    new SignalDefinition("oTarget", PLCSignalDirection.OUTPUT, SupportedSignalType.REAL32, "", "Target (deg)", onSignalChanged, null, 0f),
                    new SignalDefinition("oPulseWidth", PLCSignalDirection.OUTPUT, SupportedSignalType.REAL32, "", "Pulse Width (ms)", onSignalChanged, null, 1f),
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
                case "oTarget":
                    oTarget = (float)_newValue;
                    ServoMotor.Target = oTarget;
                    break;

                case "oPulseWidth":
                    oPulseWidth = (float)_newValue;
                    ServoMotor.PulseWidth = oPulseWidth;
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
                Debug.LogWarning("No " + typeof(LimitedServoMotor).Name + " has been assigned.");
            }
        }


        void readComponent()
        {
            if (ServoMotor.Position != iPosition)
            {
                iPosition = ServoMotor.Position;
                WriteValue("iPosition", iPosition);
            }
        }
        #endregion
    }
}