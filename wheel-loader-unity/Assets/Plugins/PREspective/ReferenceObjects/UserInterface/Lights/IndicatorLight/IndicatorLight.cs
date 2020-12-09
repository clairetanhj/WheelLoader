#if UNITY_EDITOR || UNITY_EDITOR_BETA
using UnityEditor;
using u040.prespective.utility.editor;
#endif
using System.Reflection;
using u040.prespective.prepair.inspector;
using UnityEngine;


namespace u040.prespective.referenceobjects.userinterface.lights
{
    [ExecuteInEditMode]
    public class IndicatorLight : MonoBehaviour, IControlPanel
    {
#pragma warning disable 0414
        [SerializeField] [Obfuscation] private int toolbarTab;
#pragma warning restore 0414

        [SerializeField] [Obfuscation(Exclude = true)] private int originalMaterialIndex = -1;
        [SerializeField] [Obfuscation(Exclude = true)] public int OriginalMaterialIndex
        {
            get { return this.originalMaterialIndex; }
            private set
            {
                this.originalMaterialIndex = value;
            }
        }
        [SerializeField] [Obfuscation(Exclude = true)] private Material originalMaterial;
        public Material OriginalMaterial
        {
            get { return this.originalMaterial; }
            set
            {
                if (value != this.originalMaterial && value != currentMaterial)
                {
                    saveOriginalMaterial(value);
                }
            }
        }

        [SerializeField] [Obfuscation(Exclude = true)] private Material currentMaterial;

        [SerializeField] [Obfuscation(Exclude = true)] private Color lightColor = Color.yellow;
        public Color LightColor
        {
            get { return lightColor; }
            set
            {
                if (this.lightColor != value * Intensity)
                {
                    lightColor = value;
                    if (currentMaterial != null)
                    {
                        currentMaterial.SetColor("_EmissionColor", value * Intensity);
                    }
                }
            }
        }

        [SerializeField] [Obfuscation(Exclude = true)] private Color baseColor = Color.black;
        public Color BaseColor
        {
            get { return baseColor; }
            set
            {
                if(this.baseColor != value)
                {
                    baseColor = value;
                    if (currentMaterial != null)
                    {
                        currentMaterial.SetColor("_Color", value);
                    }
                }
            }
        }

        [SerializeField] [Obfuscation(Exclude = true)] [Range(0f, 1f)] private float intensity = 0.75f;
        public float Intensity
        {
            get { return intensity; }
            set
            {
                intensity = value;
                LightColor = LightColor;
            }
        }

        public bool IsActive
        {
            get { return currentMaterial.IsKeywordEnabled("_EMISSION"); }
        }

        private const string MATERIAL_NAME_ADDITIVE = " (IndicatorLight)";

        private void OnDestroy()
        {
            LoadOriginalMaterial();
        }

        private void OnEnable()
        {
            if (detectedRequiredComponents())
            {
                if (OriginalMaterial == null)
                {
                    saveOriginalMaterial();
                }
            }
            else
            {
                Debug.LogError("Cannot be attached to this GameObject since it either does not have a MeshRenderer and a Mesh, or another " + typeof(IndicatorLight).Name + " already exists on it.");
                DestroyImmediate(this);
            }

        }

        #region <<Original Material Handling>>
        /// <summary>
        /// A method the cache the original material
        /// </summary>
        /// <param name="_material"></param>
        private void saveOriginalMaterial(Material _material = null)
        {
            if (detectedRequiredComponents())
            {
                LoadOriginalMaterial();

                Material[] _sharedMats = GetComponent<Renderer>().sharedMaterials;

                OriginalMaterialIndex = getSharedMaterialIndex(_material, _sharedMats);
                this.originalMaterial = _sharedMats[OriginalMaterialIndex];
                currentMaterial = new Material(OriginalMaterial);
                currentMaterial.name += MATERIAL_NAME_ADDITIVE;
                BaseColor = currentMaterial.color;
                currentMaterial.SetColor("_EmissionColor", lightColor * Intensity);

                _sharedMats[OriginalMaterialIndex] = currentMaterial;
                GetComponent<Renderer>().sharedMaterials = _sharedMats;

            }
            else { Debug.LogWarning("Cannot save Material because no Renderer was detected"); }
        }

        /// <summary>
        /// A method to reset the material back to its original
        /// </summary>
        public void LoadOriginalMaterial()
        {
            if (detectedRequiredComponents())
            {
                if (OriginalMaterial != null)
                {
                    Material[] _currentMats = GetComponent<Renderer>().sharedMaterials;
                    _currentMats[OriginalMaterialIndex] = OriginalMaterial;
                    GetComponent<Renderer>().sharedMaterials = _currentMats;

                    this.originalMaterial = null;
                    OriginalMaterialIndex = -1;
                    currentMaterial = null;
                }
            }
        }

        

        /// <summary>
        /// Get the index of a material in the SharedMaterials array
        /// </summary>
        /// <param name="_material"></param>
        /// <returns></returns>
        private int getSharedMaterialIndex(Material _material, Material[] _mats)
        {
            for (int _i = 0; _i < _mats.Length; _i++)
            {
                if (_mats[_i] == _material)
                {
                    return _i;
                }
            }
            return 0; //Not found
        }
        #endregion

        public void SetActive(bool _state)
        {
            if (_state != IsActive)
            {
                //Set active
                if (_state)
                {
                    currentMaterial.EnableKeyword("_EMISSION");
                }

                //Set inactive
                else
                {
                    currentMaterial.DisableKeyword("_EMISSION");
                }
            }
        }

        private bool detectedRequiredComponents()
        {
            return GetComponents<Renderer>().Length > 0 && GetComponents<MeshFilter>().Length > 0 && GetComponents<IndicatorLight>().Length <= 1;
        }


        public void ShowControlPanel()
        {
#if UNITY_EDITOR || UNITY_EDITOR_BETA
            
            //Create a style for Active or Inactive
            GUIStyle _labelStyle = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold };
            _labelStyle.normal.textColor = IsActive ? new Color(0f, 0.5f, 0f) : Color.red;

            //Draw Active field with toggle button
            EditorGUILayout.BeginHorizontal();
            PrespectiveEditor.BoolLabel("State", IsActive, "Active", "Inactive", _labelStyle);
            if (GUILayout.Button(IsActive ? "Disable" : "Enable"))
            {
                SetActive(!IsActive);
            }
            EditorGUILayout.EndHorizontal();
            Intensity = EditorGUILayout.Slider(new GUIContent("Intensity", "Light intensity of the 'active' material"), Intensity, 0f, 1f);
#endif
        }
    }
}