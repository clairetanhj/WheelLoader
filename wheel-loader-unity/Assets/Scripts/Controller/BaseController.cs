using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    public int Input { get; set; }
    public float maxSpeed;
    public bool revertDirection;

    protected float SpeedSetpoint { get; private set; }

    private bool isDisabled;
    
    void Start()
    {
        Input = 0;
    }

    private void FixedUpdate()
    {
        if (isDisabled)
        {
            SpeedSetpoint = 0;
            return;
        }
            
        // Cap input from -100% to 100% (-10000 to 10000)
        var capInput = Mathf.Clamp(Input, -10000, 10000);
        var normInput = (float) capInput / 10000;

        SpeedSetpoint = Mathf.Lerp(0, maxSpeed, Math.Abs(normInput));
        if (normInput < 0)
            SpeedSetpoint *= -1;

        if (revertDirection)
            SpeedSetpoint *= -1;
        
        Move();
    }

    protected virtual void Move() {}

    public void Disable()
    {
        isDisabled = true;
    }

    public void Enable()
    {
        isDisabled = false;
    }
}
