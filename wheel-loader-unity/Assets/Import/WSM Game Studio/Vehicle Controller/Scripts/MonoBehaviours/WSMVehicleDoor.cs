using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WSMGameStudio.Vehicles
{
    [RequireComponent(typeof(Animator))]
    public class WSMVehicleDoor : MonoBehaviour
    {
        private Animator _animator;
        private bool _isOpen = false;
        private AudioSource _openSFX;
        private AudioSource _closeSFX;

        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                _isOpen = value;
                SetAnimatorParameters();
            }
        }

        public AudioSource OpenSFX
        {
            get { return _openSFX; }
            set { _openSFX = value; }
        }

        public AudioSource CloseSFX
        {
            get { return _closeSFX; }
            set { _closeSFX = value; }
        }

        // Use this for initialization
        void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void SetAnimatorParameters()
        {
            _animator.SetBool(WSMVehicleParameters.IsOpen, _isOpen);
        }

        /// <summary>
        /// Play door open SFX
        /// </summary>
        public void PlayOpenSFX()
        {
            if (_openSFX != null)
                _openSFX.PlayOneShot(_openSFX.clip);
        }

        /// <summary>
        /// Play door close SFX
        /// </summary>
        public void PlayCloseSFX()
        {
            if (_closeSFX != null)
                _closeSFX.PlayOneShot(_closeSFX.clip);
        }
    }
}
