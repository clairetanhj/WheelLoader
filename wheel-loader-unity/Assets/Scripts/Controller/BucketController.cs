using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BucketController : BaseController
{
    protected override void Move()
    {
        transform.Rotate(new Vector3(SpeedSetpoint * Time.deltaTime,0, 0));
    }
}
