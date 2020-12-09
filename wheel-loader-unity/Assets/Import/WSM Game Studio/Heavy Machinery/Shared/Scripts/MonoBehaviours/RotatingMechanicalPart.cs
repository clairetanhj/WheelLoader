using System;
using UnityEngine;
using UnityEngine.Events;

namespace WSMGameStudio.HeavyMachinery
{
    [Serializable]
    public class RotatingMechanicalPart : MechanicalPart, IMechanicalPart
    {
        #region Overriding Base Class Methods

        /// <summary>
        /// Linear interpolation between Min and Max values
        /// </summary>
        protected override void LinearMovement()
        {
            _transform.localEulerAngles = Vector3.Lerp(_min, _max, _movementInput);
        }

        /// <summary>
        /// Non-Linear interpolation between Min and Max values
        /// </summary>
        protected override void NonLinearMovement()
        {
            _transform.localEulerAngles = Vector3.Lerp(_min, _max, _movementFunction.Evaluate(_movementInput));
        }

        /// <summary>
        /// Set current to Min
        /// </summary>
        protected override void CurrentToMin()
        {
            _min = _transform.localEulerAngles;
        }

        /// <summary>
        /// Set current to Max
        /// </summary>
        protected override void CurrentToMax()
        {
            _max = _transform.localEulerAngles;
        }

        /// <summary>
        /// Set current to default
        /// </summary>
        protected override void CurrentToDefault()
        {
            _default = _transform.localEulerAngles;
        }

        /// <summary>
        /// Set min to current
        /// </summary>
        protected override void MinToCurrent()
        {
            _transform.localEulerAngles = _min;
        }

        /// <summary>
        /// Set max to current
        /// </summary>
        protected override void MaxToCurrent()
        {
            _transform.localEulerAngles = _max;
        }

        /// <summary>
        /// Set default to current
        /// </summary>
        protected override void DefaultToCurrent()
        {
            _transform.localEulerAngles = _default;
        }

        /// <summary>
        /// Return current value
        /// </summary>
        /// <returns></returns>
        protected override Vector3 GetCurrentValue()
        {
            return _transform.localEulerAngles;
        }

        #endregion
    }
}