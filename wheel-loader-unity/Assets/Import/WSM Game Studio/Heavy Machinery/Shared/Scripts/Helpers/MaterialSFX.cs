using System;
using UnityEngine;

namespace WSMGameStudio.Audio
{
    [Serializable]
    public struct MaterialSFX
    {
        [SerializeField] public PhysicMaterial physicMaterial;
        [SerializeField] public AudioSource audioSurce;
    }
}
