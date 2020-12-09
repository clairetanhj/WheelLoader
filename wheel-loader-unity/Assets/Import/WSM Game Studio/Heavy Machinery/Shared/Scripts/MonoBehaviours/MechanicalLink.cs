using UnityEngine;

namespace WSMGameStudio.HeavyMachinery
{
    public class MechanicalLink : MonoBehaviour
    {
        public bool linkRotation = true;
        public bool linkPosition = false;
        public Transform linkTarget;
        public Transform link;
        public bool xPosConstraint;
        public bool yPosConstraint;
        public bool zPosConstraint;

        private Transform _transform;
        private float _linkDistance = 0f;
        private Vector3 _originalLocalPos;
        private Vector3 _newLocalPos = Vector3.zero;

        private void Start()
        {
            _transform = GetComponent<Transform>();
            _originalLocalPos = _transform.localPosition;

            if (link != null)
                _linkDistance = Vector3.Distance(_transform.position, link.position);
        }

        // Late Update executes after all Update were called this frame
        void LateUpdate()
        {
            LinkPosition();
            LinkRotation();
        }

        /// <summary>
        /// Ensure object is always facing the target
        /// MUST BE CALLED ON LATEUPDATE
        /// </summary>
        private void LinkRotation()
        {
            if (linkRotation && linkTarget != null)
            {
                _transform.LookAt(linkTarget, _transform.up);
            }
        }

        /// <summary>
        /// Ensure object is linked to target
        /// MUST BE CALLED ON LATEUPDATE
        /// </summary>
        private void LinkPosition()
        {
            if (linkPosition && linkTarget != null && link != null)
            {
                // Calculate position
                _transform.position = linkTarget.position - (transform.forward * _linkDistance);
                // Apply constraints
                _newLocalPos = _transform.localPosition;
                _newLocalPos.x = xPosConstraint ? _originalLocalPos.x : _newLocalPos.x;
                _newLocalPos.y = yPosConstraint ? _originalLocalPos.y : _newLocalPos.y;
                _newLocalPos.z = zPosConstraint ? _originalLocalPos.z : _newLocalPos.z;
                _transform.localPosition = _newLocalPos;
            }
        }
    }
}
