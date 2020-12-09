﻿using System;
using System.Collections.Generic;
using System.Reflection;
using u040.prespective.prelogic;
using u040.prespective.prelogic.component;
using u040.prespective.prelogic.signal;
using UnityEngine;

namespace u040.prespective.referenceobjects.sensors.beamsensor
{
    public class BeamReceiverLogic : PreLogicComponent
    {
#pragma warning disable 0414
        [SerializeField] [Obfuscation] private int toolbarTab;
#pragma warning restore 0414

        public BeamReceiver BeamReceiver;
        public bool iSensorOutput = false;
        public bool iActive = true;
        public bool oActive = true;

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
                //Inputs
                new SignalDefinition("iSensorOutput", PLCSignalDirection.INPUT, SupportedSignalType.BOOL, "", "Sensor Output Signal", null, null, false),

                //Input / output
                new SignalDefinition("iActive", PLCSignalDirection.INPUT, SupportedSignalType.BOOL, "", "Active", null, null, true),
                new SignalDefinition("oActive", PLCSignalDirection.OUTPUT, SupportedSignalType.BOOL, "", "Active", onSignalChanged, null, true),

                //Outputs
                //None
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
                case "oActive":
                    oActive = (bool)_newValue;
                    BeamReceiver.IsActive = oActive;
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
        protected override void onSimulatorUpdated(int _simFrame, float _deltaTime, float _totalSimRunTime, DateTime _simStart)
        {
            if (BeamReceiver.IsActive != iActive)
            {
                iActive = BeamReceiver.IsActive;
                WriteValue("iActive", iActive);
            }

            if (BeamReceiver.OutputSignal != iSensorOutput)
            {
                iSensorOutput = BeamReceiver.OutputSignal;
                WriteValue("iSensorOutput", iSensorOutput);
            }
        }
        #endregion
    }
}