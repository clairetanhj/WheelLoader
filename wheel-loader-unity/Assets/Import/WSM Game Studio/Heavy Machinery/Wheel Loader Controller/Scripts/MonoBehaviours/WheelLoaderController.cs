using UnityEngine;

namespace WSMGameStudio.HeavyMachinery
{
    [System.Serializable]
    public class WheelLoaderController : MonoBehaviour
    {
        #region VARIABLES
        [SerializeField]
        public LevelingMode levelingMode;
        public float loaderFrameSpeed = 0.5f;
        public float bucketSpeed = 0.5f;
        public float selfLevelingSpeed = 0.5f;

        private float _frameLeverAngle = 0f;
        private float _bucketLeverAngle = 0f;

        [SerializeField] private bool _isEngineOn = true;

        [SerializeField] public RotatingMechanicalPart loaderFrame;
        [SerializeField] public RotatingMechanicalPart bucket;
        [SerializeField] public RotatingMechanicalPart bellCrank;
        [SerializeField] public Transform loaderFrameLever;
        [SerializeField] public Transform bucketLever;
        [SerializeField] public AudioSource partsMovingSFX;
        [SerializeField] public AudioSource partsStartMovingSFX;
        [SerializeField] public AudioSource partsStopMovingSFX;

        [Range(0f, 1f)] private float _loaderFrameTilt;
        [Range(0f, 1f)] private float _bucketTilt;
        [Range(0f, 1f)] private float _bellCrankTilt;
        #endregion

        #region PROPERTIES
        public float LoaderFrameTilt { get { return _loaderFrameTilt; } set { _loaderFrameTilt = value; } }
        public float BucketTilt { get { return _bucketTilt; } set { _bucketTilt = value; } }

        public bool IsEngineOn
        {
            get { return _isEngineOn; }
            set
            {
                if (!_isEngineOn && value)
                    StartEngine();
                else if (_isEngineOn && !value)
                    StopEngine();
            }
        }
        #endregion

        #region UNITY METHODS
        /// <summary>
        /// Initialize wheel loader
        /// </summary>
        private void Start()
        {
            if (loaderFrame != null) _loaderFrameTilt = loaderFrame.MovementInput;
            if (bucket != null) _bucketTilt = bucket.MovementInput;
            if (bellCrank != null) _bellCrankTilt = bellCrank.MovementInput;

            loaderFrameSpeed = Mathf.Abs(loaderFrameSpeed);
            bucketSpeed = Mathf.Abs(bucketSpeed);
        }

        /// <summary>
        /// Late Update
        /// </summary>
        private void LateUpdate()
        {
            if (_isEngineOn)
            {
                bool isMoving = loaderFrame.IsMoving || bucket.IsMoving;
                FrameMovementSFX(isMoving); //Should be called on late update to track SFX correctly
            }
        }
        #endregion

        #region METHODS
        /// <summary>
        /// Starts vehicle engine
        /// </summary>
        public void StartEngine()
        {
            _isEngineOn = true;
        }

        /// <summary>
        /// Stop vehicle engine
        /// </summary>
        public void StopEngine()
        {
            _isEngineOn = false;
        }

        /// <summary>
        /// Handles loader frame movement
        /// </summary>
        /// <param name="horizontalInput">-1 = down | 0 = none | 1 = up</param>
        public void MoveLoaderFrame(int frameInput)
        {
            if (_isEngineOn && loaderFrame != null)
            {
                _loaderFrameTilt += (frameInput * Time.deltaTime * loaderFrameSpeed);
                _loaderFrameTilt = Mathf.Clamp01(_loaderFrameTilt);
                loaderFrame.MovementInput = _loaderFrameTilt;
            }
        }

        /// <summary>
        /// Handles loader frame movement
        /// </summary>
        /// <param name="horizontalInput">-1 = down | 0 = none | 1 = up</param>
        public void MoveBucket(int bucketInput, int frameInput)
        {
            if (_isEngineOn && bucket != null)
            {
                float speed = bucketSpeed;

                if (levelingMode == LevelingMode.SelfLeveling && bucketInput == 0)
                {
                    if (loaderFrame.IsMoving)
                    {
                        bucketInput = -frameInput;
                        speed = selfLevelingSpeed;
                    }
                }

                _bucketTilt += (bucketInput * Time.deltaTime * speed);
                _bucketTilt = Mathf.Clamp01(_bucketTilt);
                bucket.MovementInput = _bucketTilt;
            }
        }

        /// <summary>
        /// Handles bell crank movement
        /// </summary>
        /// <param name="bellCrankInput"></param>
        /// <param name="frameInput"></param>
        public void MoveBellCrank(int bellCrankInput, int frameInput)
        {
            if (_isEngineOn && bellCrank != null)
            {
                float speed = bucketSpeed;

                if (levelingMode == LevelingMode.SelfLeveling && bellCrankInput == 0)
                {
                    if (loaderFrame.IsMoving)
                    {
                        bellCrankInput = -frameInput;
                        speed = selfLevelingSpeed;
                    }
                }

                _bellCrankTilt += (bellCrankInput * Time.deltaTime * speed);
                _bellCrankTilt = Mathf.Clamp01(_bellCrankTilt);
                bellCrank.MovementInput = _bellCrankTilt;
            }
        }

        /// <summary>
        /// Animate levers accordingly to player's input
        /// </summary>
        /// <param name="frameInput"></param>
        /// <param name="bucketInput"></param>
        public void UpdateLevers(int frameInput, int bucketInput)
        {
            if (_isEngineOn)
            {
                _frameLeverAngle = Mathf.MoveTowards(_frameLeverAngle, frameInput * -15f, 80f * Time.deltaTime);
                _bucketLeverAngle = Mathf.MoveTowards(_bucketLeverAngle, bucketInput * -15f, 80f * Time.deltaTime);

                if (loaderFrameLever != null) loaderFrameLever.localEulerAngles = new Vector3(_frameLeverAngle, 0f, 0f);
                if (bucketLever != null) bucketLever.localEulerAngles = new Vector3(_bucketLeverAngle, 0f, 0f);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frameMoving"></param>
        private void FrameMovementSFX(bool frameMoving)
        {
            if (IsEngineOn && partsMovingSFX != null)
            {
                if (!partsMovingSFX.isPlaying && frameMoving)
                {
                    partsMovingSFX.Play();

                    if (partsStartMovingSFX != null && !partsStartMovingSFX.isPlaying)
                        partsStartMovingSFX.Play();
                }
                else if (partsMovingSFX.isPlaying && !frameMoving)
                {
                    partsMovingSFX.Stop();

                    if (partsStopMovingSFX != null && !partsStopMovingSFX.isPlaying)
                        partsStopMovingSFX.Play();
                }
            }
        }
        #endregion
    }
}
