namespace WSMGameStudio.Vehicles
{
    public enum WSMVehicleDrivetrainType
    {
        FWD, // Front Wheel Drive
        RWD, // Rear Wheel Drive
        AWD  // All Wheels Drive
    }

    public enum WSMVehicleSteeringMode
    {
        FrontWheelsSteering,
        RearWheelsSteering
        //ArticulatedSteering
    }

    public enum WSMVehicleSpeedUnit
    {
        MPH,
        KPH
    }

    public enum WSMVehicleTransmissionType
    {
        Automatic,
        Manual
    }

    public enum WSMVehicleCameraType
    {
        TPS,
        FPS,
        TopDown
    }

    public enum WSMVehicleCameraLookDirection
    {
        Forward,
        Backwards,
        Right,
        Left,
        Up,
        Down,
    }
}
