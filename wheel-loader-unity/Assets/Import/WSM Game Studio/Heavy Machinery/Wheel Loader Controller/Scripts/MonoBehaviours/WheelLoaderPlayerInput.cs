using UnityEngine;
using UnityEngine.Events;

namespace WSMGameStudio.HeavyMachinery
{
    [RequireComponent(typeof(WheelLoaderController))]
    public class WheelLoaderPlayerInput : MonoBehaviour
    {
        public bool enablePlayerInput = true;
        public WheelLoaderInputSettings inputSettings;
        public UnityEvent[] customEvents;

        private WheelLoaderController _wheelLoaderController;

        private int _loaderFrameTilt = 0;
        private int _bucketTilt = 0;

        /// <summary>
        /// Initializing references
        /// </summary>
        void Start()
        {
            _wheelLoaderController = GetComponent<WheelLoaderController>();
        }

        /// <summary>
        /// Handling player input
        /// </summary>
        void Update()
        {
            if (enablePlayerInput)
            {
                if (inputSettings == null) return;

                #region Wheel Loader Controls

                if (Input.GetKeyDown(inputSettings.toggleEngine))
                    _wheelLoaderController.IsEngineOn = !_wheelLoaderController.IsEngineOn;

                _loaderFrameTilt = Input.GetKey(inputSettings.loaderFrameUp) ? 1 : (Input.GetKey(inputSettings.loaderFrameDown) ? -1 : 0);
                _bucketTilt = Input.GetKey(inputSettings.bucketUp) ? 1 : (Input.GetKey(inputSettings.bucketDown) ? -1 : 0);

                _wheelLoaderController.MoveLoaderFrame(_loaderFrameTilt);
                _wheelLoaderController.MoveBellCrank(_bucketTilt, _loaderFrameTilt);
                _wheelLoaderController.MoveBucket(_bucketTilt, _loaderFrameTilt);
                _wheelLoaderController.UpdateLevers(_loaderFrameTilt, _bucketTilt);

                #endregion

                #region Player Custom Events

                for (int i = 0; i < inputSettings.customEventTriggers.Length; i++)
                {
                    if (Input.GetKeyDown(inputSettings.customEventTriggers[i]))
                    {
                        if (customEvents.Length > i)
                            customEvents[i].Invoke();
                    }
                }

                #endregion
            }
        }
    } 
}
