using System;
using UnityEngine;

namespace WSMGameStudio.Vehicles
{
    [RequireComponent(typeof(Camera))]
    public class WSMVehicleCamera : MonoBehaviour
    {
        //Local only
        private Camera _camera;
        private Transform _transform;
        private WSMVehicleController _targetVehicleController;
        private int _selectedCameraIndex;
        private int _cameraTypesLength;

        [SerializeField] private Transform _target;
        [SerializeField] private bool _cameraToggleEnabled = true;
        [SerializeField] private WSMVehicleCameraType _cameraType;
        [SerializeField] private float _tpsHeight = 2f;
        [SerializeField] private float _tpsDistance = 5f;
        [SerializeField] private float _tpsRotationSpeed = 5f;
        [SerializeField] private float _fpsRotationSpeed = 5f;
        [SerializeField] private Vector3 _fpsDefaultLookPosition = new Vector3(0f, 1.7f, -0.4f);
        [SerializeField] private Vector3 _fpsSideLookPosition = new Vector3(0f, 1.7f, -0.1f);
        [SerializeField] private Vector3 _fpsLookBackPosition = new Vector3(0f, 1.8f, -0.1f);
        [SerializeField] private Vector3 _fpsLookDownPosition = new Vector3(0f, 1.9f, 0.8f);
        [SerializeField] private float _fpsHorizontalAngleLimit = 90f;
        [SerializeField] private float _fpsVerticalAngleLimit = 30f;
        [SerializeField] private Vector3 _topPositionOffset = new Vector3(-10, 20, -10);
        [SerializeField] private bool _topDownOrthographicCam = true;

        public Transform Target { get { return _target; } set { _target = value; ValidateTarget(); } }
        public WSMVehicleCameraType CameraType { get { return _cameraType; } set { _cameraType = value; } }
        public bool CameraToggleEnabled { get { return _cameraToggleEnabled; } set { _cameraToggleEnabled = value; } }
        public float TpsHeight { get { return _tpsHeight; } set { _tpsHeight = Mathf.Abs(value); } }
        public float TpsDistance { get { return _tpsDistance; } set { _tpsDistance = Mathf.Abs(value); } }
        public float TpsRotationSpeed { get { return _tpsRotationSpeed; } set { _tpsRotationSpeed = Mathf.Abs(value); } }
        public float FpsRotationSpeed { get { return _tpsRotationSpeed; } set { _tpsRotationSpeed = Mathf.Abs(value); } }
        public Vector3 FpsDefaultLookPosition { get { return _fpsDefaultLookPosition; } set { _fpsDefaultLookPosition = value; } }
        public Vector3 FpsSideLookPosition { get { return _fpsSideLookPosition; } set { _fpsSideLookPosition = value; } }
        public Vector3 FpsLookBackPosition { get { return _fpsLookBackPosition; } set { _fpsLookBackPosition = value; } }
        public Vector3 FpsLookDownPosition { get { return _fpsLookDownPosition; } set { _fpsLookDownPosition = value; } }
        public float FpsHorizontalAngleLimit { get { return _fpsHorizontalAngleLimit; } set { _fpsHorizontalAngleLimit = Mathf.Abs(value); } }
        public float FpsVerticalAngleLimit { get { return _fpsVerticalAngleLimit; } set { _fpsVerticalAngleLimit = Mathf.Abs(value); } }
        public Vector3 TopPositionOffset { get { return _topPositionOffset; } set { _topPositionOffset = value; } }
        public bool TopDownOrthographicCam { get { return _topDownOrthographicCam; } set { _topDownOrthographicCam = value; } }

        // Use this for initialization
        void Start()
        {
            _camera = GetComponent<Camera>();
            _transform = GetComponent<Transform>();
            _selectedCameraIndex = (int)_cameraType;
            _cameraTypesLength = Enum.GetNames(typeof(WSMVehicleCameraType)).Length;
            MoveToStartPosition();
        }

        /// <summary>
        /// FixedUpdate used instead of LateUpdate to avoid camera sttutering
        /// </summary>
        void FixedUpdate()
        {
            if (_target != null)
            {
                if (!ValidateTarget())
                    return;

                switch (_cameraType)
                {
                    case WSMVehicleCameraType.TPS:
                        TPSCameraUpdate();
                        break;
                    case WSMVehicleCameraType.FPS:
                        FPSCameraUpdate();
                        break;
                    case WSMVehicleCameraType.TopDown:
                        TopDownCameraUpdate();
                        break;
                }

                if (_targetVehicleController.CameraToggleRequested)
                    ToggleCameraType();
            }
        }

        /// <summary>
        /// Validate if target is a vehicle
        /// </summary>
        /// <returns>True if vehicle</returns>
        private bool ValidateTarget()
        {
            if (_targetVehicleController == null)
            {
                _targetVehicleController = _target.GetComponent<WSMVehicleController>();

                if (_targetVehicleController == null)
                {
                    Debug.LogWarning("WSMVehicleCamera - Target is not a vehicle");
                    _target = null;
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Handles TPS Camera
        /// </summary>
        private void TPSCameraUpdate()
        {
            float currentAngleY = _transform.eulerAngles.y;
            float currentHeight = _transform.position.y;

            float newAngleY = _target.eulerAngles.y;
            float newHeight = _target.position.y + _tpsHeight;

            switch (_targetVehicleController.CamLookDirection)
            {
                case WSMVehicleCameraLookDirection.Backwards: newAngleY += 180; break;
                case WSMVehicleCameraLookDirection.Right: newAngleY += 90; break;
                case WSMVehicleCameraLookDirection.Left: newAngleY -= 90; break;
            }

            currentAngleY = Mathf.LerpAngle(currentAngleY, newAngleY, _tpsRotationSpeed * Time.deltaTime);
            currentHeight = Mathf.Lerp(currentHeight, newHeight, _tpsRotationSpeed * Time.deltaTime);

            Quaternion newRotation = Quaternion.Euler(0, currentAngleY, 0);

            _transform.position = _target.position; // Reset position
            _transform.position -= newRotation * Vector3.forward * _tpsDistance; // Apply distance
            _transform.position = new Vector3(_transform.position.x, currentHeight, _transform.position.z); // Apply Rotation

            _transform.LookAt(new Vector3(_target.position.x, _target.position.y + _tpsHeight, _target.position.z)); // Look at vehicle
        }

        /// <summary>
        /// Handles FPS Camera
        /// </summary>
        private void FPSCameraUpdate()
        {
            float targetHorizontalRotation = 0f;
            float targetVerticalRotation = 0f;

            switch (_targetVehicleController.CamLookDirection)
            {
                case WSMVehicleCameraLookDirection.Backwards:
                    targetHorizontalRotation = 180;
                    _transform.localPosition = Vector3.MoveTowards(_transform.localPosition, _fpsLookBackPosition, _fpsRotationSpeed * Time.deltaTime);
                    break;
                case WSMVehicleCameraLookDirection.Right:
                    targetHorizontalRotation = _fpsHorizontalAngleLimit;
                    _transform.localPosition = Vector3.MoveTowards(_transform.localPosition, _fpsSideLookPosition, _fpsRotationSpeed * Time.deltaTime);
                    break;
                case WSMVehicleCameraLookDirection.Left:
                    targetHorizontalRotation = -_fpsHorizontalAngleLimit;
                    _transform.localPosition = Vector3.MoveTowards(_transform.localPosition, _fpsSideLookPosition, _fpsRotationSpeed * Time.deltaTime);
                    break;
                case WSMVehicleCameraLookDirection.Up:
                    targetVerticalRotation = -_fpsVerticalAngleLimit;
                    _transform.localPosition = Vector3.MoveTowards(_transform.localPosition, _fpsDefaultLookPosition, _fpsRotationSpeed * Time.deltaTime);
                    break;
                case WSMVehicleCameraLookDirection.Down:
                    targetVerticalRotation = _fpsVerticalAngleLimit;
                    _transform.localPosition = Vector3.MoveTowards(_transform.localPosition, _fpsLookDownPosition, _fpsRotationSpeed * Time.deltaTime);
                    break;
                default:
                    targetHorizontalRotation = 0f;
                    targetVerticalRotation = 0f;
                    _transform.localPosition = Vector3.MoveTowards(_transform.localPosition, _fpsDefaultLookPosition, _fpsRotationSpeed * Time.deltaTime);
                    break;
            }

            _transform.localRotation = Quaternion.Lerp(_transform.localRotation, Quaternion.Euler(targetVerticalRotation, targetHorizontalRotation, 0f), _fpsRotationSpeed * Time.deltaTime);
        }

        /// <summary>
        /// Handles Top Down Camera
        /// </summary>
        private void TopDownCameraUpdate()
        {
            _transform.position = _target.position + _topPositionOffset;
            _transform.LookAt(_target);
        }

        /// <summary>
        /// Reset camera to start position
        /// </summary>
        public void MoveToStartPosition()
        {
            if (_target == null)
            {
                Debug.LogWarning("WSMVehicleCamera - Target cannot be null");
                return;
            }

            if (_camera == null)
                _camera = GetComponent<Camera>();

            if (_transform == null)
                _transform = GetComponent<Transform>();

            switch (_cameraType)
            {
                case WSMVehicleCameraType.TPS:
                    ClearTargetParenting();
                    _transform.position = _target.position + (_target.forward * -_tpsDistance);
                    _transform.position = new Vector3(_transform.position.x, _tpsHeight, _transform.position.z);
                    _transform.rotation = _target.rotation;
                    _camera.orthographic = false;
                    break;
                case WSMVehicleCameraType.FPS:
                    EnforceTargetParenting();
                    _transform.position = _target.position + _fpsDefaultLookPosition;
                    _transform.rotation = _target.rotation;
                    _camera.orthographic = false;
                    break;
                case WSMVehicleCameraType.TopDown:
                    ClearTargetParenting();
                    _transform.position = _target.position + _topPositionOffset;
                    _transform.LookAt(_target);
                    _camera.orthographic = _topDownOrthographicCam;
                    break;
            }
        }

        /// <summary>
        /// Toggle camera
        /// </summary>
        public void ToggleCameraType()
        {
            if (_cameraToggleEnabled)
            {
                _selectedCameraIndex++;
                _selectedCameraIndex = _selectedCameraIndex >= _cameraTypesLength ? 0 : _selectedCameraIndex;
                _cameraType = (WSMVehicleCameraType)_selectedCameraIndex;
                MoveToStartPosition();
            }
        }

        /// <summary>
        /// Make sure the camera is parented to the target
        /// </summary>
        private void EnforceTargetParenting()
        {
            if (_transform.parent != _target)
                _transform.SetParent(_target);
        }

        /// <summary>
        /// Make sure the camera is NOT parented to the target
        /// </summary>
        private void ClearTargetParenting()
        {
            if (_transform.parent == _target)
                _transform.SetParent(null);
        }
    }
}
