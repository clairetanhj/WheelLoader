using UnityEngine;

namespace WSMGameStudio.HeavyMachinery
{
    public class MechanicalPartSync : MonoBehaviour
    {
        public GameObject master;
        public GameObject slave;
        private IMechanicalPart _masterPart;
        private IMechanicalPart _slavePart;

        private void Start()
        {
            if (master != null) _masterPart = master.GetComponent<IMechanicalPart>();
            if(slave != null) _slavePart = slave.GetComponent<IMechanicalPart>();
        }

        private void Update()
        {
            if (_masterPart != null && _slavePart != null)
                _slavePart.MovementInput = _masterPart.MovementInput;
        }
    } 
}
