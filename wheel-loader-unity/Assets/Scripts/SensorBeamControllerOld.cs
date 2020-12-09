using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorBeamControllerOld : MonoBehaviour
{
    public float beamLength = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newLocalScale;
        newLocalScale = transform.localScale;
        newLocalScale.y = beamLength;
        transform.localScale = newLocalScale;

        Vector3 newLocalPosition;
        newLocalPosition = transform.localPosition;
        newLocalPosition.y = beamLength / 2;
        transform.localPosition = newLocalPosition;
    }
}
