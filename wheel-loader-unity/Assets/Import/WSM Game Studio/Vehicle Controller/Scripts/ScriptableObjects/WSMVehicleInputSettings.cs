using UnityEngine;

namespace WSMGameStudio.Vehicles
{
    [CreateAssetMenu(fileName = "NewVehicleInputSettings", menuName = "WSM Game Studio/Vehicle Controller/Vehicle Input Settings", order = 1)]
    public class WSMVehicleInputSettings : ScriptableObject
    {
        public KeyCode toggleEngine = KeyCode.T;
        public KeyCode acceleration = KeyCode.W;
        public KeyCode reverse = KeyCode.S;
        public KeyCode turnRight = KeyCode.D;
        public KeyCode turnLeft = KeyCode.A;
        public KeyCode brakes = KeyCode.Space;
        public KeyCode handbrake = KeyCode.LeftControl;
        public KeyCode clutch = KeyCode.LeftShift;
        public KeyCode horn = KeyCode.H;
        public KeyCode headlights = KeyCode.L;
        public KeyCode interiorLights = KeyCode.I;
        public KeyCode leftSignalLights = KeyCode.Q;
        public KeyCode rightSignalLights = KeyCode.E;
        public KeyCode cameraLookRight = KeyCode.RightArrow;
        public KeyCode cameraLookLeft = KeyCode.LeftArrow;
        public KeyCode cameraLookBack = KeyCode.DownArrow;
        public KeyCode cameraLookUp = KeyCode.UpArrow;
        public KeyCode cameraLookDown = KeyCode.RightShift;
        public KeyCode toggleCamera = KeyCode.C;

        public KeyCode[] customEventTriggers;
    } 
}
