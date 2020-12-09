using UnityEngine;
using UnityEngine.Events;

namespace WSMGameStudio.HeavyMachinery
{
    public interface IMechanicalPart
    {
        float MovementInput { get; set; }
        bool IsMoving { get; }
        MovingMode MovingMode { get; set; }
        AnimationCurve MovementFunction { get; set; }
        Vector3 Min { get; set; }
        Vector3 Max { get; set; }
        Vector3 Default { get; set; }

        UnityEvent ReachedMin { get; set; }
        UnityEvent ReachedMax { get; set; }
        UnityEvent StartedMovement { get; set; }
        UnityEvent FinishedMovement { get; set; }
    } 
}