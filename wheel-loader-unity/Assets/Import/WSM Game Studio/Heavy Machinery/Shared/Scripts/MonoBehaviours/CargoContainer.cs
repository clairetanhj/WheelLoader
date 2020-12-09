using UnityEngine;

namespace WSMGameStudio.HeavyMachinery
{
    public class CargoContainer : MonoBehaviour
    {
        private Transform _transform;
        private bool _isMoving = false;
        private Vector3 _lastPosition;
        private Vector3 _movement;

        public bool IsMoving
        {
            get { return _isMoving; }
            set { _isMoving = value; }
        }

        public Vector3 Movement
        {
            get { return _movement; }
            set { _movement = value; }
        }

        private void Start()
        {
            _transform = GetComponent<Transform>();
            _lastPosition = _transform.position;
        }

        private void Update()
        {
            _isMoving = _lastPosition != _transform.position;

            _movement = _isMoving ? _transform.position - _lastPosition : Vector3.zero;

            _lastPosition = _transform.position;
        }
    } 
}
