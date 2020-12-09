using UnityEngine;

namespace WSMGameStudio.HeavyMachinery
{
    [CreateAssetMenu(fileName = "NewWheelLoaderInputSettings", menuName = "WSM Game Studio/Heavy Machinery/Wheel Loader Input Settings", order = 1)]
    public class WheelLoaderInputSettings : ScriptableObject
    {
        public KeyCode toggleEngine = KeyCode.T;
        public KeyCode loaderFrameUp = KeyCode.Keypad5;
        public KeyCode loaderFrameDown = KeyCode.Keypad2;
        public KeyCode bucketUp = KeyCode.Keypad4;
        public KeyCode bucketDown = KeyCode.Keypad1;

        public KeyCode[] customEventTriggers;
    } 
}
