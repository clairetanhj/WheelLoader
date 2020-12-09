using u040.prespective.prepair.components.sensors;
using u040.prespective.utility;
using UnityEngine;

namespace u040.prespective.referenceobjects.sensors.beamsensor
{
    public class PerfectBeamReflector : BaseBeamReflector
    {
        public void Reset()
        {
            Collider _collider = this.GetComponent<Collider>();

            if(!_collider)
            {
                Debug.LogWarning("The " + this.GetType().Name + " component on " + this.gameObject.name + " GameObject requires a Collider component in order to function.");
            }
        }

        public override BeamPathRedirectionPoint resolveHit(Vector3 _hitVector, RaycastHit hit)
        {
            return new BeamPathRedirectionPoint(this, hit.point, hit.point, Vector3.Reflect(_hitVector, hit.normal));
        }

        public override void lostHit()
        {
            //Do Nothing
        }
    }
}
