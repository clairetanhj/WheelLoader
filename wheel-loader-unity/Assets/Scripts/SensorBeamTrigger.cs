using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorBeamTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeamTriggered()
    {
        Debug.Log("Obstacle detected");
    }

    public void BeamCleared()
    {
        Debug.Log("Cleared");
    }
}
