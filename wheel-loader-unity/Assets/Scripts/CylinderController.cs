using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderController : MonoBehaviour
{
    public float pistonSpeed = 0;
    public float sideSpeed = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (Input.GetKey("w"))
        {
            Debug.Log("Going up");
            transform.Translate(new Vector3(0, pistonSpeed * Time.deltaTime, 0));
        }
        
        else if (Input.GetKey("s"))
        {
            Debug.Log("Going down");
            transform.Translate(new Vector3(0, -pistonSpeed * Time.deltaTime, 0));
        }
        
        else if (Input.GetKey("a"))
        {
            Debug.Log("Going left");
            transform.Translate(new Vector3(sideSpeed * Time.deltaTime, 0));
        }
        
        else if (Input.GetKey("d"))
        {
            Debug.Log("Going right");
            transform.Translate(new Vector3(-sideSpeed * Time.deltaTime, 0));
        }
    }
}
