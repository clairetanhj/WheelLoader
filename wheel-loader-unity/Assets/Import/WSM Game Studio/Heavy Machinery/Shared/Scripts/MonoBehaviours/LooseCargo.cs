using UnityEngine;

namespace WSMGameStudio.HeavyMachinery
{
    public class LooseCargo : MonoBehaviour
    {
        private Transform _transform;
        private CargoContainer _parentCargoContainer;

        private void Start()
        {
            _transform = GetComponent<Transform>();
        }

        private void OnTriggerEnter(Collider other)
        {
            CargoContainer cargoContainer = other.GetComponent<CargoContainer>();

            if (cargoContainer != null)
            {
                _parentCargoContainer = cargoContainer;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            CargoContainer cargoContainer = other.GetComponent<CargoContainer>();

            if (cargoContainer != null && _parentCargoContainer != null)
            {
                if (_parentCargoContainer.gameObject.GetInstanceID() == cargoContainer.gameObject.GetInstanceID())
                    _parentCargoContainer = null;
            }
        }

        private void Update()
        {
            if (_parentCargoContainer != null && _parentCargoContainer.IsMoving)
            {
                _transform.position += _parentCargoContainer.Movement;
            }
        }
    }
}
