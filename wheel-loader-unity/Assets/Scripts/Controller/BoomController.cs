using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSMGameStudio.HeavyMachinery;

public class BoomController : BaseController
{
    private RotatingMechanicalPart _boom;
    
    private void Start()
    {
        _boom = gameObject.GetComponent<RotatingMechanicalPart>();
    }

    protected override void Move()
    {
        _boom.MovementInput = Mathf.Clamp01(_boom.MovementInput + SpeedSetpoint * Time.deltaTime);
    }
}
