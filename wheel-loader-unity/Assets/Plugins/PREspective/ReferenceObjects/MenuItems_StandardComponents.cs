#if UNITY_EDITOR || UNITY_EDITOR_BETA
using UnityEngine;
using UnityEditor;
using u040.prespective.utility;
using u040.prespective.referenceobjects.materialhandling.gripper;
using u040.prespective.referenceobjects.kinetics.motor.dcmotor;
using u040.prespective.referenceobjects.kinetics.motor.servomotor;
using u040.prespective.referenceobjects.kinetics.motor.steppermotor;
using u040.prespective.referenceobjects.materialhandling.beltsystem;
using u040.prespective.referenceobjects.sensors.beamsensor;
using u040.prespective.referenceobjects.sensors.proximitysensor;
using u040.prespective.referenceobjects.userinterface.buttons.encoders;
using u040.prespective.referenceobjects.userinterface.buttons.switches;
using u040.prespective.referenceobjects.userinterface.lights;
using u040.prespective.referenceobjects.userinterface.unityui;
using u040.prespective.referenceobjects.sensors.colorsensor;
using u040.prespective.prepair.inspector;
using u040.prespective.utility.editor;
using u040.prespective.prepair.physics.kinetics.belt;

public class MenuItems_StandardComponents
{
    /*
    * Shortcurts:
    * SHIFT #
    * CTRL %
    * ALT &
    * Example: "PathToMenuItem #%e"
    * Order of symbols matters
    */

    [MenuItem(MenuDefinitions_StandardComponents.ControlPanelPath, false, MenuDefinitions_StandardComponents.PRIO_CONTROL_PANEL_PSL)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.ControlPanelPath, false, MenuDefinitions_StandardComponents.PRIO_CONTROL_PANEL_PSL + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addControlPanelPhysical() { PreSpectiveUtility.AddComponentToSelection(typeof(ControlPanelInterface), typeof(ControlPanelInterface).Name); }

    //////////
    //////////

    [MenuItem(MenuDefinitions_StandardComponents.DCMotorPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_DC_MOTOR_PSL)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.DCMotorPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_DC_MOTOR_PSL + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addDCMotorPhysical() { PreSpectiveUtility.AddComponentToSelection(typeof(DCMotor), typeof(DCMotor).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.DCMotorPath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_DC_MOTOR_LGC)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.DCMotorPath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_DC_MOTOR_LGC + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addDCMotorLogic() { PreSpectiveUtility.AddComponentToSelection(typeof(DCMotorLogic), typeof(DCMotorLogic).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.ContinuousServoMotorPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_CONTINUOUS_SERVO_PSL)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.ContinuousServoMotorPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_CONTINUOUS_SERVO_PSL + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addContinuousServoMotorPhysical() { PreSpectiveUtility.AddComponentToSelection(typeof(ContinuousServoMotor), typeof(ContinuousServoMotor).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.ContinuousServoMotorPath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_CONTINUOUS_SERVO_LGC)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.ContinuousServoMotorPath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_CONTINUOUS_SERVO_LGC + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void AddContinuousServoMotorLogic() { PreSpectiveUtility.AddComponentToSelection(typeof(ContinuousServoMotorLogic), typeof(ContinuousServoMotorLogic).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.LimitedServoMotorPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_LIMITED_SERVO_PSL)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.LimitedServoMotorPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_LIMITED_SERVO_PSL + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addLimitedServoMotorPhysical() { PreSpectiveUtility.AddComponentToSelection(typeof(LimitedServoMotor), typeof(LimitedServoMotor).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.LimitedServoMotorPath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_LIMITED_SERVO_LGC)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.LimitedServoMotorPath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_LIMITED_SERVO_LGC + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addLimitedServoMotorLogic() { PreSpectiveUtility.AddComponentToSelection(typeof(LimitedServoMotorLogic), typeof(LimitedServoMotorLogic).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.DrivenServoMotorPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_DRIVEN_SERVO_PSL)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.DrivenServoMotorPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_DRIVEN_SERVO_PSL + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addDrivenServoMotorPhysical() { PreSpectiveUtility.AddComponentToSelection(typeof(DrivenServoMotor), typeof(DrivenServoMotor).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.DrivenServoMotorPath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_DRIVEN_SERVO_LGC)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.DrivenServoMotorPath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_DRIVEN_SERVO_LGC + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addDrivenServoMotorLogic() { PreSpectiveUtility.AddComponentToSelection(typeof(DrivenServoMotorLogic), typeof(DrivenServoMotorLogic).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.DrivenStepperMotorPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_DRIVEN_STEPPER_PSL)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.DrivenStepperMotorPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_DRIVEN_STEPPER_PSL + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addDrivenStepperMotorPhysical() { PreSpectiveUtility.AddComponentToSelection(typeof(DrivenStepperMotor), typeof(DrivenStepperMotor).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.DrivenStepperMotorPath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_DRIVEN_STEPPER_LGC)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.DrivenStepperMotorPath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_DRIVEN_STEPPER_LGC + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addDrivenStepperMotorLogic() { PreSpectiveUtility.AddComponentToSelection(typeof(DrivenStepperMotorLogic), typeof(DrivenStepperMotorLogic).Name); }

    //////////
    //////////

    [MenuItem(MenuDefinitions_StandardComponents.IndicatorLightPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_INDICATOR_LIGHT_PSL)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.IndicatorLightPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_INDICATOR_LIGHT_PSL + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addIndicatorLightPhysical() { PreSpectiveUtility.AddComponentToSelection(typeof(IndicatorLight), typeof(IndicatorLight).Name, true, PrimitiveType.Sphere); }

    [MenuItem(MenuDefinitions_StandardComponents.IndicatorLightPath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_INDICATOR_LIGHT_LGC)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.IndicatorLightPath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_INDICATOR_LIGHT_LGC + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addIndicatorLightLogic() { PreSpectiveUtility.AddComponentToSelection(typeof(IndicatorLightLogic), typeof(IndicatorLightLogic).Name); }

    //////////
    //////////

    [MenuItem(MenuDefinitions_StandardComponents.BeltSystemPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_BELT_SYSTEM_PSL)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.BeltSystemPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_BELT_SYSTEM_PSL + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addBeltSystemPhysical() { PreSpectiveUtility.AddComponentToSelection(typeof(BeltSystem), typeof(BeltSystem).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.BeltRendererPath, false, MenuDefinitions_StandardComponents.PRIO_BELT_RENDERER_PSL)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.BeltRendererPath, false, MenuDefinitions_StandardComponents.PRIO_BELT_SYSTEM_PSL + 20 + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addBeltRendererPhysical() { PreSpectiveUtility.AddComponentToSelection(typeof(BeltRenderer), typeof(BeltRenderer).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.BeltRollPath, false, MenuDefinitions_StandardComponents.PRIO_BELT_ROLL_PSL)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.BeltRollPath, false, MenuDefinitions_StandardComponents.PRIO_BELT_SYSTEM_PSL + 21 + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addBeltRollPhysical() { PreSpectiveUtility.AddComponentToSelection(typeof(BeltRoll), typeof(BeltRoll).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.GripperBasePath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_GRIPPER_BASE_PSL)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.GripperBasePath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_GRIPPER_BASE_PSL + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addGripperBasePhysical() { PreSpectiveUtility.AddComponentToSelection(typeof(GripperBase), typeof(GripperBase).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.GripperBasePath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_GRIPPER_BASE_LGC)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.GripperBasePath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_GRIPPER_BASE_LGC + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addGripperBaseLogic() { PreSpectiveUtility.AddComponentToSelection(typeof(GripperBaseLogic), typeof(GripperBaseLogic).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.AngularGripperFingerPath, false, MenuDefinitions_StandardComponents.PRIO_GRIPPER_ANGULAR_FINGER_PSL)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.AngularGripperFingerPath, false, MenuDefinitions_StandardComponents.PRIO_GRIPPER_ANGULAR_FINGER_PSL + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addAngularGripperFingerPhysical() { PreSpectiveUtility.AddComponentToSelection(typeof(AngularGripperFinger), typeof(AngularGripperFinger).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.ParallelGripperFingerPath, false, MenuDefinitions_StandardComponents.PRIO_GRIPPER_PARALLEL_FINGER_PSL)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.ParallelGripperFingerPath, false, MenuDefinitions_StandardComponents.PRIO_GRIPPER_PARALLEL_FINGER_PSL + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addParallelGripperFingerPhysical() { PreSpectiveUtility.AddComponentToSelection(typeof(ParallelGripperFinger), typeof(ParallelGripperFinger).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.VacuumGripperFingerPath, false, MenuDefinitions_StandardComponents.PRIO_GRIPPER_VACUUM_FINGER_PSL)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.VacuumGripperFingerPath, false, MenuDefinitions_StandardComponents.PRIO_GRIPPER_VACUUM_FINGER_PSL + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addVacuumGripperFingerPhysical() { PreSpectiveUtility.AddComponentToSelection(typeof(VacuumGripperFinger), typeof(VacuumGripperFinger).Name); }

    //////////
    //////////

    [MenuItem(MenuDefinitions_StandardComponents.BeamEmitterPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_BEAM_EMITTER_PSL)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.BeamEmitterPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_BEAM_EMITTER_PSL + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addBeamEmitterPhysical() { PreSpectiveUtility.AddComponentToSelection(typeof(BeamEmitter), typeof(BeamEmitter).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.BeamEmitterPath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_BEAM_EMITTER_LGC)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.BeamEmitterPath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_BEAM_EMITTER_LGC + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addBeamEmitterLogic() { PreSpectiveUtility.AddComponentToSelection(typeof(BeamEmitterLogic), typeof(BeamEmitterLogic).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.BeamReceiverPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_BEAM_RECEIVER_PSL)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.BeamReceiverPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_BEAM_RECEIVER_PSL + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addBeamReceiverPhysical() { PreSpectiveUtility.AddComponentToSelection(typeof(BeamReceiver), typeof(BeamReceiver).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.BeamReceiverPath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_BEAM_RECEIVER_LGC)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.BeamReceiverPath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_BEAM_RECEIVER_LGC + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addBeamReceiverLogic() { PreSpectiveUtility.AddComponentToSelection(typeof(BeamReceiverLogic), typeof(BeamReceiverLogic).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.BeamReflectorsPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_BEAM_REFLECTOR_PSL)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.BeamReflectorsPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_BEAM_REFLECTOR_PSL + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addBeamReflectorPhysical() { PreSpectiveUtility.AddComponentToSelection(typeof(PerfectBeamReflector), typeof(PerfectBeamReflector).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.ColorSensorComponentPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_COLOR_SENSOR_PSL)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.ColorSensorComponentPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_COLOR_SENSOR_PSL + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addColorSensorPhysical() { PreSpectiveUtility.AddComponentToSelection(typeof(ColorSensor), typeof(ColorSensor).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.ColorSensorComponentPath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_COLOR_SENSOR_LGC)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.ColorSensorComponentPath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_COLOR_SENSOR_LGC + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addColorSensorLogic() { PreSpectiveUtility.AddComponentToSelection(typeof(ColorSensorLogic), typeof(ColorSensorLogic).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.ColorDetectorPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_COLOR_DETECTOR_PSL)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.ColorDetectorPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_COLOR_DETECTOR_PSL + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addColorDetectorPhysical() { PreSpectiveUtility.AddComponentToSelection(typeof(ColorDetector), typeof(ColorDetector).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.ColorDetectorPath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_COLOR_DETECTOR_LGC)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.ColorDetectorPath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_COLOR_DETECTOR_LGC + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addColorDetectorLogic() { PreSpectiveUtility.AddComponentToSelection(typeof(ColorDetectorLogic), typeof(ColorDetectorLogic).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.ContrastSensorPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_CONTRAST_SENSOR_PSL)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.ContrastSensorPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_CONTRAST_SENSOR_PSL + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addContrastSensorPhysical() { PreSpectiveUtility.AddComponentToSelection(typeof(ContrastSensor), typeof(ContrastSensor).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.ContrastSensorPath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_CONTRAST_SENSOR_LGC)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.ContrastSensorPath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_CONTRAST_SENSOR_LGC + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addContrastSensorLogic() { PreSpectiveUtility.AddComponentToSelection(typeof(ContrastSensorLogic), typeof(ContrastSensorLogic).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.ProximitySensorPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_PROXIMITY_SENSOR_PSL)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.ProximitySensorPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_PROXIMITY_SENSOR_PSL + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addProximitySensorPhysical() { PreSpectiveUtility.AddComponentToSelection(typeof(ProximitySensor), typeof(ProximitySensor).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.ProximitySensorPath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_PROXIMITY_SENSOR_LGC)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.ProximitySensorPath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_PROXIMITY_SENSOR_LGC + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addProximitySensorLogic() { PreSpectiveUtility.AddComponentToSelection(typeof(ProximitySensorLogic), typeof(ProximitySensorLogic).Name); }

    //////////
    //////////

    [MenuItem(MenuDefinitions_StandardComponents.RotarySwitchPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_ROTARY_SWITCH_PSL)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.RotarySwitchPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_ROTARY_SWITCH_PSL + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addRotarySwitchPhysical() { PreSpectiveUtility.AddComponentToSelection(typeof(RotarySwitch), typeof(RotarySwitch).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.RotarySwitchPath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_ROTARY_SWITCH_LGC)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.RotarySwitchPath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_ROTARY_SWITCH_LGC + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addRotarySwitchLogic() { PreSpectiveUtility.AddComponentToSelection(typeof(RotarySwitchLogic), typeof(RotarySwitchLogic).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.SlideSwitchPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_SLIDE_SWITCH_PSL)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.SlideSwitchPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_SLIDE_SWITCH_PSL + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addSlideSwitchPhysical() { PreSpectiveUtility.AddComponentToSelection(typeof(SlideSwitch), typeof(SlideSwitch).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.SlideSwitchPath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_SLIDE_SWITCH_LGC)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.SlideSwitchPath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_SLIDE_SWITCH_LGC + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addSlideSwitchLogic() { PreSpectiveUtility.AddComponentToSelection(typeof(SlideSwitchLogic), typeof(SlideSwitchLogic).Name); }

    //////////
    //////////

    [MenuItem(MenuDefinitions_StandardComponents.LinearEncoderPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_LINEAR_ENCODER_PSL)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.LinearEncoderPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_LINEAR_ENCODER_PSL + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addLinearEncoderPhysical() { PreSpectiveUtility.AddComponentToSelection(typeof(LinearEncoder), typeof(LinearEncoder).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.LinearEncoderPath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_LINEAR_ENCODER_LGC)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.LinearEncoderPath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_LINEAR_ENCODER_LGC + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addLinearEncoderLogic() { PreSpectiveUtility.AddComponentToSelection(typeof(LinearEncoderLogic), typeof(LinearEncoderLogic).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.RotaryEncoderPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_ROTARY_ENCODER_PSL)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.RotaryEncoderPath + MenuDefinitions_StandardComponents.PhysicalComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_ROTARY_ENCODER_PSL + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addRotaryEncoderPhysical() { PreSpectiveUtility.AddComponentToSelection(typeof(RotaryEncoder), typeof(RotaryEncoder).Name); }

    [MenuItem(MenuDefinitions_StandardComponents.RotaryEncoderPath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_ROTARY_ENCODER_LGC)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.RotaryEncoderPath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_ROTARY_ENCODER_LGC + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addRotaryEncoderLogic() { PreSpectiveUtility.AddComponentToSelection(typeof(RotaryEncoderLogic), typeof(RotaryEncoderLogic).Name); }

    //////////
    //////////

    [MenuItem(MenuDefinitions_StandardComponents.UnityUILogicPath + MenuDefinitions_StandardComponents.LogicComponentSuffix, false, MenuDefinitions_StandardComponents.PRIO_UNITY_UI_LGC)]
    [MenuItem(MenuDefinitions_StandardComponents.GameObjectMenuPath + MenuDefinitions_StandardComponents.UnityUILogicPath, false, MenuDefinitions_StandardComponents.PRIO_UNITY_UI_LGC + MenuDefinitions_StandardComponents.GAMEOBJECT_MENU_PRIO_MODIFIER)]
    private static void addUnityUILogic() { PreSpectiveUtility.AddComponentToSelection(typeof(UnityUILogic), typeof(UnityUILogic).Name); }
}
#endif