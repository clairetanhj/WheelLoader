using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltrasonicSensorController : MonoBehaviour
{
    public float angle = 30;
    public float rayLength = 2;
    public int rayNumber = 10;

    private float _theta;
    private double[] _distance;
    // private Color _missColor = new Color(0, 1, 0, 1);
    private Color _missColor = Color.green;
    private Color _hitColor = Color.red;
    private Color[] _gizmosLineColor;
    private Vector3[] _gizmosLineVector;


    private void OnValidate()
    {
        _theta = angle * Mathf.Deg2Rad;
    }

    // Update is called once per frame
    void Update()
    {
        _distance = new double[rayNumber];
        _gizmosLineColor = new Color[rayNumber];
        _gizmosLineVector = new Vector3[rayNumber];
        
        for (int i = 0; i < rayNumber; i++)
        {
            float chordLength = 2 * Mathf.Sin(_theta / 2);
            var dir = transform.forward + UnityEngine.Random.insideUnitSphere * (chordLength / 2);
            if (Physics.Raycast(transform.position, dir, out RaycastHit hitInfo, rayLength))
            {
                _gizmosLineColor[i] = _hitColor;
                _gizmosLineVector[i] = dir.normalized * hitInfo.distance;
                _distance[i] = Math.Round(hitInfo.distance, 3);
            }
            else
            {
                _gizmosLineColor[i] = _missColor;
                _gizmosLineVector[i] = dir.normalized * rayLength;
                _distance[i] = rayLength;
            }
        }

        // Debug.Log(string.Join(" ", _distance));
    }
    
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            for (int i = 0; i < rayNumber; i++)
            {
                Gizmos.color = _gizmosLineColor[i];
                Gizmos.DrawLine(transform.position, transform.position + _gizmosLineVector[i]);
            }            
        }
    }
}
