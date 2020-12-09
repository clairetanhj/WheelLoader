using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingMasterObject : MonoBehaviour
{
    // public GameObject testObject;
    public TestingScript testObject;
    
    private void Update()
    {
        Debug.Log(testObject.health);
    }
}
