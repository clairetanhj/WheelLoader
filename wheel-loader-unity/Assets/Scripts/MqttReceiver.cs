using System;
using System.Collections.Generic;
using Data.Util;
using UnityEngine;
using u040.prespective.prelogic;
using u040.prespective.prelogic.component;
using u040.prespective.prelogic.signal;
using u040.prespective.referenceobjects.kinetics.motor.dcmotor;
using WSMGameStudio.HeavyMachinery;

public class MqttReceiver : PreLogicComponent
{
    private const string BrushTopic = "rotate";
    private const string PistonTopic = "extend";
    private const string BoomTopic = "lift";
    private const string BucketTopic = "tilt";
    
    [Header("Wheel Loader Settings")]
    public BrushController brush;
    public PistonController piston;
    public BucketController bucket;
    public BoomController boom;

    private bool _isEnabled;
    private int _boomDirection;

    public override List<SignalDefinition> SignalDefinitions
    {
        get
        {
            return new List<SignalDefinition>()
            {
                new SignalDefinition(BrushTopic, PLCSignalDirection.OUTPUT, SupportedSignalType.INT16, "", "BrushValue", OnSignalChanged, null, 0),
                new SignalDefinition(PistonTopic, PLCSignalDirection.OUTPUT, SupportedSignalType.INT16, "", "PistonValue", OnSignalChanged, null, 0),
                new SignalDefinition(BucketTopic, PLCSignalDirection.OUTPUT, SupportedSignalType.INT16, "", "BucketValue", OnSignalChanged, null, 0),
                new SignalDefinition(BoomTopic, PLCSignalDirection.OUTPUT, SupportedSignalType.INT16, "", "BoomValue", OnSignalChanged, null, 0)
            };
        }
    }
    
    void OnSignalChanged(SignalInstance signal, object newValue, DateTime newValueReceived,
        object oldValue, DateTime oldValueReceived)
    {
        var input = Convert.ToInt16(newValue);
        switch (signal.definition.defaultSignalName)
        {
            case BrushTopic:
                Debug.Log("BrushTopic : " + input);
                brush.Input = input;
                break;
            case PistonTopic:
                Debug.Log("PistonTopic : " + input);
                piston.Input = input;
                break;
            case BucketTopic:
                Debug.Log("BucketTopic : " + input);
                bucket.Input = input;
                break;
            case BoomTopic:
                Debug.Log("BoomTopic : " + input);
                boom.Input = input;
                break;
        }
    }

    public void Enable()
    {
        if (!_isEnabled)
            _isEnabled = true;
        
        var controllerList = new List<BaseController> {brush, piston, bucket, boom};
        foreach (var controller in controllerList)
        {
            controller.Enable();
        }
    }
    
    public void Disable()
    {
        if (!_isEnabled) return;

        var controllerList = new List<BaseController> {brush, piston, bucket, boom};
        foreach (var controller in controllerList)
        {
            controller.Disable();
        }
    }
}
