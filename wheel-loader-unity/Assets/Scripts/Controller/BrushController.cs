using System.Collections;
using System.Collections.Generic;
using u040.prespective.referenceobjects.kinetics.motor.dcmotor;
using UnityEngine;

public class BrushController : BaseController
{
    private DCMotor _motor;

    private void Start()
    {
        _motor = gameObject.GetComponent<DCMotor>();
    }

    protected override void Move()
    {
        _motor.TargetVelocity = SpeedSetpoint * _motor.MaxVelocity;
    }
}
