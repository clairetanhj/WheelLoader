using System;
using UnityEngine;
using UnityEngine.Events;

namespace WSMGameStudio.HeavyMachinery
{
    [Serializable]
    public class MovingMechanicalPart : MechanicalPart, IMechanicalPart
    {
        #region Overriding Base Class Methods

        /// <summary>
        /// Linear interpolation between Min and Max values
        /// </summary>
        protected override void LinearMovement()
        {
            _transform.localPosition = Vector3.Lerp(_min, _max, _movementInput);
        }

        /// <summary>
        /// Non-Linear interpolation between Min and Max values
        /// </summary>
        protected override void NonLinearMovement()
        {
            _transform.localPosition = Vector3.Lerp(_min, _max, _movementFunction.Evaluate(_movementInput));
        }

        /// <summary>
        /// Set current to Min
        /// </summary>
        protected override void CurrentToMin()
        {
            _min = _transform.localPosition;
        }

        /// <summary>
        /// Set current to Max
        /// </summary>
        protected override void CurrentToMax()
        {
            _max = _transform.localPosition;
        }

        /// <summary>
        /// Set current to default
        /// </summary>
        protected override void CurrentToDefault()
        {
            _default = _transform.localPosition;
        }

        /// <summary>
        /// Set min to current
        /// </summary>
        protected override void MinToCurrent()
        {
            _transform.localPosition = _min;
        }

        /// <summary>
        /// Set max to current
        /// </summary>
        protected override void MaxToCurrent()
        {
            _transform.localPosition = _max;
        }

        /// <summary>
        /// Set default to current
        /// </summary>
        protected override void DefaultToCurrent()
        {
            _transform.localPosition = _default;
        }

        /// <summary>
        /// Return current value
        /// </summary>
        /// <returns></returns>
        protected override Vector3 GetCurrentValue()
        {
            return _transform.localPosition;
        }

        #endregion
    }
}
