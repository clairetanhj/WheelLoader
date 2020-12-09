using System.Collections.Generic;
using System.Reflection;
using u040.prespective.prelogic;
using u040.prespective.prelogic.component;
using u040.prespective.prelogic.signal;
using UnityEngine;

namespace u040.prespective.referenceobjects.userinterface.buttons.switches
{
    public class RotarySwitchLogic : PreLogicComponent
    {
#pragma warning disable 0414
        [SerializeField] [Obfuscation] private int toolbarTab;
#pragma warning restore 0414

        public RotarySwitch RotarySwitch;
        
        //public string iSelectedState = "N/A";
        public int iSelectedId = -1;


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
                new SignalDefinition("iSelectedID", PLCSignalDirection.INPUT, SupportedSignalType.INT32, "", "Selected ID", null, null, -1),
            };
            }
        }
        #endregion
        #endregion

        #region <<Update>>
        private void FixedUpdate()
        {
            readComponent();
        }
        #endregion


        void readComponent()
        {
            if (RotarySwitch.SelectedState.Id != this.iSelectedId) // || RotarySwitch.SelectedState.Name != this.iSelectedState)
            {
                //this.iSelectedState = RotarySwitch.SelectedState.Name;
                this.iSelectedId = RotarySwitch.SelectedState.Id;
                //WriteValue("iSelectedState", this.iSelectedState);
                WriteValue("iSelectedID", this.iSelectedId);
                
            }
        }
    }
}