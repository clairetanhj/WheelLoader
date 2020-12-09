using System.Collections.Generic;
using System.Reflection;
using u040.prespective.prelogic;
using u040.prespective.prelogic.component;
using u040.prespective.prelogic.signal;
using u040.prespective.prepair.ui.buttons;
using UnityEngine;

namespace u040.prespective.referenceobjects.userinterface.buttons.switches
{
    public class SlideSwitchLogic : PreLogicComponent
    {
#pragma warning disable 0414
        [SerializeField] [Obfuscation] private int toolbarTab;
#pragma warning restore 0414

        public SlideSwitch SlideSwitch;

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
                return new List<SignalDefinition>()
                {
                    //Inputs
                    //new SignalDefinition("iSelectedState", PLCSignalDirection.INPUT, SupportedSignalType.STRING, "", "Selected State", null, null, "N/A"),
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
            if (SlideSwitch.SelectedState.Id != this.iSelectedId) // || SlideSwitch.SelectedState.Name != this.iSelectedState)
            {
                //this.iSelectedState = SlideSwitch.SelectedState.Name;
                this.iSelectedId = SlideSwitch.SelectedState.Id;
                //WriteValue("iSelectedState", this.iSelectedState);
                WriteValue("iSelectedID", this.iSelectedId);
            }
        }
    }
}