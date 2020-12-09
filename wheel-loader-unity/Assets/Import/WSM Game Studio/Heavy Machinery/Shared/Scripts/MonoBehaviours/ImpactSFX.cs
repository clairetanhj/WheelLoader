using System.Collections.Generic;
using UnityEngine;

namespace WSMGameStudio.Audio
{
    public class ImpactSFX : MonoBehaviour
    {
        public bool allowIndividualSounds = true;
        public SFX_TriggerType triggerType;
        public float minCollisionForce = 5f;
        public MaterialSFX[] MaterialSoundEffect;

        private Dictionary<string, AudioSource> _materialSFX_Dictionary;

        /// <summary>
        /// 
        /// </summary>
        void Start()
        {
            _materialSFX_Dictionary = new Dictionary<string, AudioSource>();

            for (int i = 0; i < MaterialSoundEffect.Length; i++)
            {
                if (!_materialSFX_Dictionary.ContainsKey(MaterialSoundEffect[i].physicMaterial.name))
                    _materialSFX_Dictionary.Add(MaterialSoundEffect[i].physicMaterial.name, MaterialSoundEffect[i].audioSurce);
            }
        }

        /// <summary>
        /// Handles trigger SFX
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (triggerType == SFX_TriggerType.Trigger)
                PlayMaterialSFX(other.material);
        }

        /// <summary>
        /// Handles collision SFX
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter(Collision collision)
        {
            if (triggerType == SFX_TriggerType.Collision)
            {
                float collisionForce = (collision.impulse / Time.deltaTime).magnitude;
                if (collisionForce >= minCollisionForce)
                {
                    PlayMaterialSFX(collision.collider.material);
                }
            }
        }

        /// <summary>
        /// Play physic material corresponding SFX
        /// </summary>
        /// <param name="material"></param>
        private void PlayMaterialSFX(PhysicMaterial material)
        {
            if (material != null)
            {
                string physMaterialName = material.name.Replace(" (Instance)", string.Empty);

                if (_materialSFX_Dictionary.ContainsKey(physMaterialName))
                {
                    if (allowIndividualSounds)
                        _materialSFX_Dictionary[physMaterialName].PlayOneShot(_materialSFX_Dictionary[physMaterialName].clip);
                    else if (!_materialSFX_Dictionary[physMaterialName].isPlaying)
                        _materialSFX_Dictionary[physMaterialName].Play();
                }
            }
        }
    }
}
