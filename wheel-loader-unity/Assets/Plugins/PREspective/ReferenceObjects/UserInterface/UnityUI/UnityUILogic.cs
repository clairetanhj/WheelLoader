using System;
using System.Collections.Generic;
using System.Reflection;
using u040.prespective.prelogic;
using u040.prespective.prelogic.component;
using u040.prespective.prelogic.signal;
using u040.prespective.prepair;
using UnityEngine;

namespace u040.prespective.referenceobjects.userinterface.unityui
{
    public class UnityUILogic : PreLogicComponent
    {
#pragma warning disable 0414
        [SerializeField] [Obfuscation] private int toolbarTab;
#pragma warning restore 0414

        /// <summary>
        /// Print debug logs to console
        /// </summary>
        [SerializeField] [Obfuscation(Exclude = true)] private List<string> signalNames = new List<string>();
        public List<string> SignalNames
        {
            get { return signalNames; }
            set
            {
                List<string> _tempNameList = new List<string>();
                List<string> _tempDateList = new List<string>();
                for (int _i = 0; _i < value.Count; _i++)
                {
                    string _name = value[_i];
                    _name = _name.Replace(" ", "_");
                    _tempNameList.Add(_name);
                    _tempDateList.Add(DateTime.MinValue.ToLongTimeString());
                }
                signalNames = _tempNameList;
                lastTriggeredTime = _tempDateList;
            }
        }

        [SerializeField] [Obfuscation(Exclude = true)] private List<string> lastTriggeredTime = new List<string>();
        /// <summary>
        /// A list which holds timestamps of when individual signals were last triggered
        /// </summary>
        public List<string> LastTriggeredTime
        {
            get { return lastTriggeredTime; }
        }
        private string defaultTimestamp = DateTime.MinValue.ToLongTimeString();
        public string DefaultTimestamp { get; }

        private string inputPrefix = "i";
        private string outputPrefix = "o";

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
                List<SignalDefinition> _signalDefList = new List<SignalDefinition>();

                for (int _i = 0; _i < SignalNames.Count; _i++)
                {
                    SignalDefinition _newInputSignalDef = new SignalDefinition(inputPrefix + SignalNames[_i], PLCSignalDirection.INPUT, SupportedSignalType.BOOL, "", SignalNames[_i] + " received", null, null, false);
                    SignalDefinition _newOutputSignalDef = new SignalDefinition(outputPrefix + SignalNames[_i], PLCSignalDirection.OUTPUT, SupportedSignalType.BOOL, "", SignalNames[_i] + " confirmed", onSignalChanged, null, false);

                    if (!_signalDefList.Contains(_newInputSignalDef) && !_signalDefList.Contains(_newOutputSignalDef))
                    {
                        _signalDefList.Add(_newInputSignalDef);
                        _signalDefList.Add(_newOutputSignalDef);
                    }
                    else { Debug.LogError("Cannot add \"" + SignalNames[_i] + "\" to the list since it already exists within it."); }
                }
                return _signalDefList;
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
            if ((bool)_newValue)
            {
                string _signalName = _signal.definition.defaultSignalName.Substring(1);
                string _inputSignalname = inputPrefix + _signalName;
                WriteValue(_inputSignalname, false);
            }
        }
        #endregion

        /// <summary>
        /// Trigger a signal by its ID
        /// </summary>
        /// <param name="_signalID"></param>
        public void SendSignal(int _signalID)
        {
            if (_signalID >= 0 && _signalID < SignalNames.Count)
            {
                SendSignal(SignalNames[_signalID]);
            }
            else { Debug.LogError("Cannot send signal \"" + _signalID.ToString() + "\" since it is not defined."); }
        }

        /// <summary>
        /// Trigger a signal by its name
        /// </summary>
        /// <param name="_signalName"></param>
        public void SendSignal(string _signalName)
        {
            if (SignalNames.Contains(_signalName))
            {
                if (WriteValue(inputPrefix + _signalName, true))
                {
                    saveTimeStamp(_signalName);
                }
                else { Debug.LogError("Unable to set value " + _signalName); }
            }
            else { Debug.LogError("Cannot send signal \"" + _signalName + "\" since it is not defined."); }
        }

        private void saveTimeStamp(string _signalName)
        {
            lastTriggeredTime[signalNames.IndexOf(_signalName)] = DateTime.Now.ToLongTimeString();
        }
        private void saveTimeStamp(int _signalID)
        {
            lastTriggeredTime[_signalID] = DateTime.Now.ToLongTimeString();
        }

        #endregion
    }
}