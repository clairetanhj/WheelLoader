using System;
using UnityEngine;
using UnityEngine.Events;

namespace WSMGameStudio.HeavyMachinery
{
    [Serializable]
    public class MechanicalPart : MonoBehaviour
    {
        protected Transform _transform;

        [Range(0, 1)]
        [SerializeField]
        protected float _movementInput;
        [SerializeField] protected MovingMode _movingMode;
        [SerializeField] protected AnimationCurve _movementFunction = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        [SerializeField] protected Vector3 _min;
        [SerializeField] protected Vector3 _max;
        [SerializeField] protected Vector3 _default;
        [SerializeField] protected UnityEvent _reachedMin;
        [SerializeField] protected UnityEvent _reachedMax;
        [SerializeField] protected UnityEvent _startedMovement;
        [SerializeField] protected UnityEvent _finishedMovement;

        protected bool _lastMovingState = false;
        protected bool _firstFrame = true;
        protected bool _isMoving = false;
        protected AnimationCurve _inverseMovementFunction;

        public float MovementInput
        {
            get { return _movementInput; }
            set
            {
                if (_movementInput != value)
                {
                    _movementInput = value;
                    _isMoving = true;

                    if (_movementInput >= 1f && _reachedMax != null)
                        _reachedMax.Invoke();

                    if (_movementInput <= 0f && _reachedMin != null)
                        _reachedMin.Invoke();
                }
                else
                    _isMoving = false;
            }
        }

        public bool IsMoving { get { return _isMoving; } }
        public MovingMode MovingMode { get { return _movingMode; } set { _movingMode = value; } }
        public AnimationCurve MovementFunction { get { return _movementFunction; } set { _movementFunction = value; } }
        public Vector3 Min { get { return _min; } set { _min = value; } }
        public Vector3 Max { get { return _max; } set { _max = value; } }
        public Vector3 Default { get { return _default; } set { _default = value; } }
        public UnityEvent ReachedMin { get { return _reachedMin; } set { _reachedMin = value; } }
        public UnityEvent ReachedMax { get { return _reachedMax; } set { _reachedMax = value; } }
        public UnityEvent StartedMovement { get { return _startedMovement; } set { _startedMovement = value; } }
        public UnityEvent FinishedMovement { get { return _finishedMovement; } set { _finishedMovement = value; } }

        // Use this for initialization
        void OnEnable()
        {
            _transform = GetComponent<Transform>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_isMoving)
            {
                //Move
                switch (_movingMode)
                {
                    case MovingMode.Linear:
                        LinearMovement();
                        break;
                    case MovingMode.Function:
                        NonLinearMovement();
                        break;
                }

                if (!_firstFrame)
                {
                    if (!_lastMovingState && _startedMovement != null)
                        _startedMovement.Invoke();

                    _lastMovingState = _isMoving;
                }
            }
            else
            {
                if (_lastMovingState && _finishedMovement != null && !_firstFrame)
                    _finishedMovement.Invoke();

                _lastMovingState = _isMoving;
            }

            _firstFrame = false;
        }

        /// <summary>
        /// Set current position as min position (In-Editor Only)
        /// </summary>
        public void SetCurrentAsMin()
        {
#if UNITY_EDITOR
            ValidateTransform();
            CurrentToMin();
#endif
        }

        /// <summary>
        /// Set current position as max position (In-Editor Only)
        /// </summary>
        public void SetCurrentAsMax()
        {
#if UNITY_EDITOR
            ValidateTransform();
            CurrentToMax();
#endif
        }

        /// <summary>
        /// Set current position as default position (In-Editor Only)
        /// </summary>
        public void SetCurrentAsDefault()
        {
#if UNITY_EDITOR
            ValidateTransform();
            CurrentToDefault();
            Vector3 current = GetCurrentValue();
            RecalculateMovementInput(current);
#endif
        }

        /// <summary>
        /// Move part to min position
        /// </summary>
        public void ResetToMin()
        {
            ValidateTransform();
            MinToCurrent();
            _movementInput = 0f;
        }

        /// <summary>
        /// Move part to max position
        /// </summary>
        public void ResetToMax()
        {
            ValidateTransform();
            MaxToCurrent();
            _movementInput = 1f;
        }

        /// <summary>
        /// Move part to its default position
        /// </summary>
        public void ResetToDefault()
        {
            ValidateTransform();
            DefaultToCurrent();
            RecalculateMovementInput(_default);
        }

        /// <summary>
        /// Make sure transform is not null while using In-Editor operations
        /// </summary>
        private void ValidateTransform()
        {
            if (_transform == null) _transform = GetComponent<Transform>();
        }

        /// <summary>
        /// Recalculate movement input based on current value
        /// </summary>
        /// <param name="current"></param>
        public void RecalculateMovementInput(Vector3 current)
        {
            float totalDistance = Mathf.Abs(Vector3.Distance(_max, _min));
            float currentDistance = Mathf.Abs(Vector3.Distance(current, _min));
            _movementInput = totalDistance == 0f ? 0f : (currentDistance / totalDistance);

            if (_movingMode == MovingMode.Function)
            {
                _inverseMovementFunction = new AnimationCurve();

                for (int i = 0; i < _movementFunction.length; i++)
                {
                    Keyframe inverseKey = new Keyframe(_movementFunction.keys[i].value, _movementFunction.keys[i].time, InverseTangent(_movementFunction.keys[i].outTangent), InverseTangent(_movementFunction.keys[i].inTangent));
                    _inverseMovementFunction.AddKey(inverseKey);
                }

                _movementInput = _inverseMovementFunction.Evaluate(_movementInput);
            }
        }

        /// <summary>
        /// Calculate aproximatade value for KeyFrame tangents
        /// </summary>
        /// <param name="tangent"></param>
        /// <returns></returns>
        private float InverseTangent(float tangent)
        {
            float sign = tangent < 0f ? -1 : 1;
            float inverse = ((Mathf.PI * 0.5f) - Mathf.Abs(tangent)) * sign;
            return inverse;
        }

        #region Virtual Methods

        /// <summary>
        /// Linear interpolation between Min and Max values
        /// </summary>
        protected virtual void LinearMovement() { }
        /// <summary>
        /// Non-Linear interpolation between Min and Max values
        /// </summary>
        protected virtual void NonLinearMovement() { }
        /// <summary>
        /// Set current to Min
        /// </summary>
        protected virtual void CurrentToMin() { }
        /// <summary>
        /// Set current to Max
        /// </summary>
        protected virtual void CurrentToMax() { }
        /// <summary>
        /// Set current to default
        /// </summary>
        protected virtual void CurrentToDefault() { }
        /// <summary>
        /// Set min to current
        /// </summary>
        protected virtual void MinToCurrent() { }
        /// <summary>
        /// Set max to current
        /// </summary>
        protected virtual void MaxToCurrent() { }
        /// <summary>
        /// Set default to current
        /// </summary>
        protected virtual void DefaultToCurrent() { }
        /// <summary>
        /// Return current value
        /// </summary>
        /// <returns></returns>
        protected virtual Vector3 GetCurrentValue() { return Vector3.zero; }

        #endregion
    }
}
