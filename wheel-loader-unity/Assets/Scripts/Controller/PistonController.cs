using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonController : BaseController
{
    protected override void Move()
    {
        transform.Translate(new Vector3(0, SpeedSetpoint * Time.deltaTime, 0));
    }
}
