using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorBeamController : MonoBehaviour
{
    public float beamLength;
    
    private string _searchTag = "sensorBeam";
    private List<GameObject> _beamList = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        if (_searchTag != null)
        {
            beamLength = 3;
            FindObjectWithTag(_searchTag);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject beam in _beamList)
        {
            Transform t = beam.transform;
            if (beamLength != t.localScale.y)
            {
                Vector3 newLocalScale = t.localScale;
                newLocalScale.y = beamLength;
                t.localScale = newLocalScale;

                Vector3 newLocalPosition = t.localPosition;
                // newLocalPosition.y = beamLength / 2;
                newLocalPosition.y = beamLength / 2 + (float)0.5;
                t.localPosition = newLocalPosition;
            }
        }
    }

    private void FindObjectWithTag(string tag)
    {
        _beamList.Clear();
        Transform parent = transform;
        GetChildObject(parent, tag);
    }

    private void GetChildObject(Transform parent, string tag)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.CompareTag(tag))
            {
                _beamList.Add(child.gameObject);
            }

            if (child.childCount > 0)
            {
                GetChildObject(child, tag);
            }
        }
    }
}
