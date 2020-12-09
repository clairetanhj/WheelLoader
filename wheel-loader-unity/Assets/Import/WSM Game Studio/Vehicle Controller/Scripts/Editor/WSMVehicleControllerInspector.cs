using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace WSMGameStudio.Vehicles
{
    [CustomEditor(typeof(WSMVehicleController))]
    public class WSMVehicleControllerInspector : Editor
    {
        private WSMVehicleController _vehicleController;

        protected SerializedProperty _frontWheelsColliders;
        protected SerializedProperty _rearWheelsColliders;
        protected SerializedProperty _extraWheelsColliders;
        protected SerializedProperty _frontWheelsMeshes;
        protected SerializedProperty _rearWheelsMeshes;
        protected SerializedProperty _extraWheelsMeshes;
        protected SerializedProperty _txtSpeed;
        protected SerializedProperty _steeringWheel;
        protected SerializedProperty _gasPedal;
        protected SerializedProperty _brakesPedal;
        protected SerializedProperty _clutchPedal;
        protected SerializedProperty _centerOfMass;
        protected SerializedProperty _articulatedVehiclePivot;
        protected SerializedProperty _engineStartSFX;
        protected SerializedProperty _engineSFX;
        protected SerializedProperty _hornSFX;
        protected SerializedProperty _wheelsSkiddingSFX;
        protected SerializedProperty _backUpBeeperSFX;
        protected SerializedProperty _signalLightsSFX;
        protected SerializedProperty _openDoorSFX;
        protected SerializedProperty _closeDoorSFX;
        protected SerializedProperty _headlights;
        protected SerializedProperty _rearLights;
        protected SerializedProperty _brakeLights;
        protected SerializedProperty _interiorLights;
        protected SerializedProperty _signalLightsLeft;
        protected SerializedProperty _signalLightsRight;
        protected SerializedProperty _reverseAlarmLights;
        protected SerializedProperty _alarmLightsPivot;
        protected SerializedProperty _driverDoor;
        protected SerializedProperty _passengersDoors;

        private int _selectedMenuIndex = 0;
        private string[] _toolbarMenuOptions = new[] { "Settings", "Wheels", "Parts", "SFX", "Lights", "UI" };
        private GUIStyle _menuBoxStyle;

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            _vehicleController = target as WSMVehicleController;

            EditorGUI.BeginChangeCheck();
            _selectedMenuIndex = GUILayout.Toolbar(_selectedMenuIndex, _toolbarMenuOptions);
            if (EditorGUI.EndChangeCheck())
            {
                GUI.FocusControl(null);
            }

            //Set up the box style if null
            if (_menuBoxStyle == null)
            {
                _menuBoxStyle = new GUIStyle(GUI.skin.box);
                _menuBoxStyle.normal.textColor = GUI.skin.label.normal.textColor;
                _menuBoxStyle.fontStyle = FontStyle.Bold;
                _menuBoxStyle.alignment = TextAnchor.UpperLeft;
            }
            GUILayout.BeginVertical(_menuBoxStyle);

            if (_toolbarMenuOptions[_selectedMenuIndex] == "Settings")
            {
                /*
                 * SETTINGS
                 */
                GUILayout.Label("SETTINGS", EditorStyles.boldLabel);

                EditorGUI.BeginChangeCheck();
                bool isEngineOn = EditorGUILayout.Toggle("Engine On", _vehicleController.IsEngineOn);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleController, "Toggled Engine On");
                    _vehicleController.IsEngineOn = isEngineOn;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                WSMVehicleDrivetrainType drivetrainType = (WSMVehicleDrivetrainType)EditorGUILayout.EnumPopup("Drivetrain Type", _vehicleController.drivetrainType);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleController, "Changed Drivetrain");
                    _vehicleController.drivetrainType = drivetrainType;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                float maxTorque = EditorGUILayout.FloatField("Max Torque", _vehicleController.MaxTorque);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleController, "Changed Max Torque");
                    _vehicleController.MaxTorque = maxTorque;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                float reverseTorque = EditorGUILayout.FloatField("Reverse Torque", _vehicleController.ReverseTorque);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleController, "Changed Reverse Torque");
                    _vehicleController.ReverseTorque = reverseTorque;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                float maxBrakesTorque = EditorGUILayout.FloatField("Brakes Torque", _vehicleController.BrakesTorque);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleController, "Changed Brakes Torque");
                    _vehicleController.BrakesTorque = maxBrakesTorque;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                float handbrakeTorque = EditorGUILayout.FloatField("Handbrake Torque", _vehicleController.HandbrakesTorque);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleController, "Changed Handbrake Torque");
                    _vehicleController.HandbrakesTorque = handbrakeTorque;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                WSMVehicleSpeedUnit speedUnit = (WSMVehicleSpeedUnit)EditorGUILayout.EnumPopup("Speed Unit", _vehicleController.speedUnit);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleController, "Changed Speed Unit");
                    _vehicleController.speedUnit = speedUnit;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                float maxSpeed = EditorGUILayout.FloatField("Max Speed", _vehicleController.MaxSpeed);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleController, "Changed Max Speed");
                    _vehicleController.MaxSpeed = maxSpeed;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                float maxReverseSpeed = EditorGUILayout.FloatField("Max Reverse Speed", _vehicleController.MaxReverseSpeed);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleController, "Changed Max Reverse Speed");
                    _vehicleController.MaxReverseSpeed = maxReverseSpeed;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                WSMVehicleSteeringMode steeringMode = (WSMVehicleSteeringMode)EditorGUILayout.EnumPopup("Steering Mode", _vehicleController.steeringMode);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleController, "Changed Steering Mode");
                    _vehicleController.steeringMode = steeringMode;
                    MarkSceneAlteration();
                }

                //EditorGUI.BeginChangeCheck();
                //bool nonMovingArticulatedSteering = EditorGUILayout.Toggle("Non-Moving Articulated Steering", _vehicleController.NonMovingArticulatedSteering);
                //if (EditorGUI.EndChangeCheck())
                //{
                //    Undo.RecordObject(_vehicleController, "Toggled Non-Moving Articulated Steering");
                //    _vehicleController.NonMovingArticulatedSteering = nonMovingArticulatedSteering;
                //    MarkSceneAlteration();
                //}

                EditorGUI.BeginChangeCheck();
                float maxSteeringAngle = EditorGUILayout.FloatField("Max Steering Angle", _vehicleController.MaxSteeringAngle);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleController, "Changed Steering Angle");
                    _vehicleController.MaxSteeringAngle = maxSteeringAngle;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                float highSpeedSteeringAngle = EditorGUILayout.FloatField("High Speed Steering Angle", _vehicleController.HighSpeedSteeringAngle);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleController, "Changed High Speed Steering Angle");
                    _vehicleController.HighSpeedSteeringAngle = highSpeedSteeringAngle;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                bool softSteering = EditorGUILayout.Toggle("Soft Steering", _vehicleController.SoftSteering);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleController, "Toggled Soft Steering");
                    _vehicleController.SoftSteering = softSteering;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                float steeringWheelAngleMultiplier = EditorGUILayout.FloatField("Steering Wheel Angle Multiplier", _vehicleController.SteeringWheelAngleMultiplier);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleController, "Changed Steering Wheel Angle Multiplier");
                    _vehicleController.SteeringWheelAngleMultiplier = steeringWheelAngleMultiplier;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                int numberOfGears = EditorGUILayout.IntField("Number of Gears", _vehicleController.NumberOfGears);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleController, "Changed Number of Gears");
                    _vehicleController.NumberOfGears = numberOfGears;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                float steerHelper = EditorGUILayout.Slider("Steer Helper", _vehicleController.SteerHelper, 0f, 1f);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleController, "Changed Steer Helper");
                    _vehicleController.SteerHelper = steerHelper;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                float tractionControl = EditorGUILayout.Slider("Traction Control", _vehicleController.TractionControl, 0f, 1f);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleController, "Changed Traction Control");
                    _vehicleController.TractionControl = tractionControl;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                float tractionSlipLimit = EditorGUILayout.FloatField("Traction Slip Slimit", _vehicleController.TractionSlipLimit);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleController, "Changed Traction Slip Limit");
                    _vehicleController.TractionSlipLimit = tractionSlipLimit;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                Vector3 centerOfMassOffset = EditorGUILayout.Vector3Field("Center Of Mass Offset", _vehicleController.CenterOfMassOffset);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleController, "Changed Center Of Mass Offset");
                    _vehicleController.CenterOfMassOffset = centerOfMassOffset;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                float idleEngineRevs = EditorGUILayout.FloatField("Idle Engine Revs", _vehicleController.IdleEngineRevs);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleController, "Changed Idle Engine Revs");
                    _vehicleController.IdleEngineRevs = idleEngineRevs;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                float maxEngineRevs = EditorGUILayout.FloatField("Max Engine Revs", _vehicleController.MaxEngineRevs);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleController, "Changed Max Engine Revs");
                    _vehicleController.MaxEngineRevs = maxEngineRevs;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                float downforce = EditorGUILayout.FloatField("Downforce", _vehicleController.Downforce);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleController, "Changed Downforce");
                    _vehicleController.Downforce = downforce;
                    MarkSceneAlteration();
                }
            }
            else if (_toolbarMenuOptions[_selectedMenuIndex] == "Wheels")
            {
                /*
                 * WHEELS
                 */
                serializedObject.Update();
                _frontWheelsColliders = serializedObject.FindProperty("frontWheelsColliders");
                _rearWheelsColliders = serializedObject.FindProperty("rearWheelsColliders");
                _extraWheelsColliders = serializedObject.FindProperty("extraWheelsColliders");
                _frontWheelsMeshes = serializedObject.FindProperty("frontWheelsMeshes");
                _rearWheelsMeshes = serializedObject.FindProperty("rearWheelsMeshes");
                _extraWheelsMeshes = serializedObject.FindProperty("extraWheelsMeshes");

                GUILayout.Label("WHEELS COLLIDERS", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_frontWheelsColliders, true);
                EditorGUILayout.PropertyField(_rearWheelsColliders, true);
                EditorGUILayout.PropertyField(_extraWheelsColliders, true);

                GUILayout.Label("WHEELS MESHES", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_frontWheelsMeshes, true);
                EditorGUILayout.PropertyField(_rearWheelsMeshes, true);
                EditorGUILayout.PropertyField(_extraWheelsMeshes, true);

                serializedObject.ApplyModifiedProperties();
            }
            else if (_toolbarMenuOptions[_selectedMenuIndex] == "Parts")
            {
                serializedObject.Update();

                _steeringWheel = serializedObject.FindProperty("steeringWheel");
                _gasPedal = serializedObject.FindProperty("gasPedal");
                _brakesPedal = serializedObject.FindProperty("brakesPedal");
                _clutchPedal = serializedObject.FindProperty("clutchPedal");
                _centerOfMass = serializedObject.FindProperty("centerOfMass");
                //_articulatedVehiclePivot = serializedObject.FindProperty("articulatedVehiclePivot");
                _driverDoor = serializedObject.FindProperty("driverDoor");
                _passengersDoors = serializedObject.FindProperty("passengersDoors");

                GUILayout.Label("VEHICLE PARTS", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_steeringWheel);
                EditorGUILayout.PropertyField(_gasPedal);
                EditorGUILayout.PropertyField(_brakesPedal);
                EditorGUILayout.PropertyField(_clutchPedal);
                EditorGUILayout.PropertyField(_centerOfMass);
                //EditorGUILayout.PropertyField(_articulatedVehiclePivot);
                EditorGUILayout.PropertyField(_driverDoor);
                EditorGUILayout.PropertyField(_passengersDoors, true);

                serializedObject.ApplyModifiedProperties();
            }
            else if (_toolbarMenuOptions[_selectedMenuIndex] == "SFX")
            {
                serializedObject.Update();

                _engineStartSFX = serializedObject.FindProperty("engineStartSFX");
                _engineSFX = serializedObject.FindProperty("engineSFX");
                _hornSFX = serializedObject.FindProperty("hornSFX");
                _wheelsSkiddingSFX = serializedObject.FindProperty("wheelsSkiddingSFX");
                _backUpBeeperSFX = serializedObject.FindProperty("backUpBeeperSFX");
                _signalLightsSFX = serializedObject.FindProperty("signalLightsSFX");
                _openDoorSFX = serializedObject.FindProperty("openDoorSFX");
                _closeDoorSFX = serializedObject.FindProperty("closeDoorSFX");

                GUILayout.Label("AUDIO SOURCES", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_engineStartSFX);
                EditorGUILayout.PropertyField(_engineSFX);
                EditorGUILayout.PropertyField(_hornSFX);
                EditorGUILayout.PropertyField(_wheelsSkiddingSFX);
                EditorGUILayout.PropertyField(_backUpBeeperSFX);
                EditorGUILayout.PropertyField(_signalLightsSFX);
                EditorGUILayout.PropertyField(_openDoorSFX);
                EditorGUILayout.PropertyField(_closeDoorSFX);
                serializedObject.ApplyModifiedProperties();

                GUILayout.Label("AUDIO SETTINGS", EditorStyles.boldLabel);
                EditorGUI.BeginChangeCheck();
                float minEnginePitch = EditorGUILayout.FloatField("Min Engine Pitch", _vehicleController.MinEnginePitch);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleController, "Changed Min Engine Pitch");
                    _vehicleController.MinEnginePitch = minEnginePitch;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                float maxEnginePitch = EditorGUILayout.FloatField("Max Engine Pitch", _vehicleController.MaxEnginePitch);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleController, "Changed Max Engine Pitch");
                    _vehicleController.MaxEnginePitch = maxEnginePitch;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                float sfxForwardSlipLimit = EditorGUILayout.FloatField("Sfx Forward Slip Limit", _vehicleController.SfxForwardSlipLimit);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleController, "Changed Sfx Forward Slip Limit");
                    _vehicleController.SfxForwardSlipLimit = sfxForwardSlipLimit;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                float sfxSidewaysSlipLimit = EditorGUILayout.FloatField("Sfx Sideway Slip Limit", _vehicleController.SfxSidewaysSlipLimit);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleController, "Changed Sfx Sideways Slip Limit");
                    _vehicleController.SfxSidewaysSlipLimit = sfxSidewaysSlipLimit;
                    MarkSceneAlteration();
                }
            }
            else if (_toolbarMenuOptions[_selectedMenuIndex] == "Lights")
            {
                GUILayout.Label("VEHICLE LIGHTS", EditorStyles.boldLabel);

                serializedObject.Update();
                _headlights = serializedObject.FindProperty("headlights");
                _rearLights = serializedObject.FindProperty("rearLights");
                _brakeLights = serializedObject.FindProperty("brakeLights");
                _interiorLights = serializedObject.FindProperty("interiorLights");
                _signalLightsLeft = serializedObject.FindProperty("signalLightsLeft");
                _signalLightsRight = serializedObject.FindProperty("signalLightsRight");
                _reverseAlarmLights = serializedObject.FindProperty("reverseAlarmLights");
                _alarmLightsPivot = serializedObject.FindProperty("alarmLightsPivot");

                EditorGUILayout.PropertyField(_headlights, true);
                EditorGUILayout.PropertyField(_rearLights, true);
                EditorGUILayout.PropertyField(_brakeLights, true);
                EditorGUILayout.PropertyField(_interiorLights, true);
                EditorGUILayout.PropertyField(_signalLightsLeft, true);
                EditorGUILayout.PropertyField(_signalLightsRight, true);
                EditorGUILayout.PropertyField(_reverseAlarmLights, true);
                EditorGUILayout.PropertyField(_alarmLightsPivot);

                serializedObject.ApplyModifiedProperties();

                GUILayout.Label("LIGHTS SETTINGS", EditorStyles.boldLabel);
                EditorGUI.BeginChangeCheck();
                bool headlighstOn = EditorGUILayout.Toggle("Headlights On", _vehicleController.HeadlightsOn);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleController, "Toogled Headlights");
                    _vehicleController.HeadlightsOn = headlighstOn;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                bool InteriorLightsOn = EditorGUILayout.Toggle("Interior Lights On", _vehicleController.InteriorLightsOn);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleController, "Toogled Interior Lights");
                    _vehicleController.InteriorLightsOn = InteriorLightsOn;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                bool rotateAlarmLights = EditorGUILayout.Toggle("Rotate Alarm Lights", _vehicleController.RotateAlarmLights);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleController, "Toogled Alarm Lights Rotation");
                    _vehicleController.RotateAlarmLights = rotateAlarmLights;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                float alarmLightsRotationSpeed = EditorGUILayout.FloatField("Alarm Lights Rotation Speed", _vehicleController.AlarmLightsRotationSpeed);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleController, "Changed Alarm Lights Rotation Speed");
                    _vehicleController.AlarmLightsRotationSpeed = alarmLightsRotationSpeed;
                    MarkSceneAlteration();
                }
            }
            else if (_toolbarMenuOptions[_selectedMenuIndex] == "UI")
            {
                serializedObject.Update();

                _txtSpeed = serializedObject.FindProperty("speedText");
                GUILayout.Label("UI REFERENCES", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_txtSpeed);

                serializedObject.ApplyModifiedProperties();
            }

            GUILayout.EndVertical();
        }

        private void MarkSceneAlteration()
        {
            if (!Application.isPlaying)
            {
#if UNITY_EDITOR
                EditorUtility.SetDirty(_vehicleController);
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
#endif
            }
        }
    }
}
