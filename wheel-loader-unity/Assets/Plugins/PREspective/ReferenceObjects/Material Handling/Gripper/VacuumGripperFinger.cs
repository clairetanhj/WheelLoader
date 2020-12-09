using System.Collections.Generic;
using u040.prespective.prepair.kinematics;
using u040.prespective.referenceobjects.materialhandling.gripper;
using UnityEngine;

public class VacuumGripperFinger : GripperFinger
{
    public override KinematicTransform KinematicTransform { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private bool isActive = true;

    private List<GameObject> touchingObjects = new List<GameObject>();

    protected override void onObjectDetected(Collider _collider)
    {
        //If the list does not already contain the GameObject
        if (!touchingObjects.Contains(_collider.gameObject))
        {
            //Add it to the list
            touchingObjects.Add(_collider.gameObject);
            if (isActive)
            {
                detectedObjects.Add(_collider.gameObject);
            }
        }
    }

    protected override void onObjectLost(Collider _collider)
    {
        touchingObjects.Remove(_collider.gameObject);
        if (isActive)
        {
            detectedObjects.Remove(_collider.gameObject);
        }
    }


    public override void SetPosition(float _percentage)
    {
        //If position is 50%+, state is active
        bool _newState = _percentage >= 0.5f ? true : false;

        //If state switches
        if (_newState != isActive)
        {
            //Set new state
            isActive = _newState;

            //If switching active
            if (_newState)
            {
                //Add all touching objects to detected objects
                detectedObjects.AddRange(touchingObjects);
            }

            //If switching inactive
            else
            {
                //remove all detected objects
                detectedObjects.Clear();
            }
        }
    }
}
