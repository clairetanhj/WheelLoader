#if UNITY_EDITOR || UNITY_EDITOR_BETA
using UnityEditor;
using u040.prespective.utility.editor;
#endif
using System.Reflection;
using u040.prespective.core;
using u040.prespective.prepair.components.sensors;
using u040.prespective.prepair.inspector;
using UnityEngine;

namespace u040.prespective.referenceobjects.sensors.colorsensor
{
    public class ColorSensor : QualitativeSensor<Color>, IControlPanel
    {
#pragma warning disable 0414
        [SerializeField] [Obfuscation] private int toolbarTab;
#pragma warning restore 0414

        [SerializeField] [Obfuscation(Exclude = true)] private bool fixedRendering = false;
        public bool FixedRendering
        {
            get { return this.fixedRendering; }
            set
            {
                if (this.fixedRendering != value)
                {
                    this.fixedRendering = value;
                }
            }
        }

        public Camera SensorCamera;
        [SerializeField] [Obfuscation(Exclude = true)] private RenderTexture renderTexture;
        [SerializeField] [Obfuscation(Exclude = true)] private Texture2D texture;
               
        /// <summary>
        /// Default color when nothing is detected
        /// </summary>
        public Color VoidColor = Color.black;

        public Vector2 Range = new Vector2(0f, 3f);

        private void Awake()
        {
            //Render texture
            renderTexture = new RenderTexture(1, 1, 16, RenderTextureFormat.ARGB32);
            renderTexture.name = "Generated Render Texture";
            renderTexture.Create();

            //Texture2D
            texture = new Texture2D(1, 1);

            //Create Camera object
            GameObject _cameraObject = new GameObject("SENSOR_CAMERA_OBJECT");
            _cameraObject.transform.SetParent(this.transform);
            _cameraObject.transform.localPosition = Vector3.zero;
            _cameraObject.transform.localRotation = Quaternion.identity;

            //Setup Camera
            SensorCamera = _cameraObject.AddComponent<Camera>();
            SensorCamera.clearFlags = CameraClearFlags.SolidColor;
            SensorCamera.backgroundColor = VoidColor;
            SensorCamera.orthographic = true;
            SensorCamera.orthographicSize = 0.0001f;
            SensorCamera.targetTexture = renderTexture;
            SensorCamera.nearClipPlane = Range.x;
            SensorCamera.farClipPlane = Range.y;
            if (FixedRendering) { SensorCamera.enabled = false; }

            //Create local event link
            LocalEventLink _link = _cameraObject.AddComponent<LocalEventLink>();
            _link.Listener = this;
            _link.PostRender = postRender;
        }

        private void FixedUpdate()
        {
            if (SensorCamera != null && FixedRendering)
            {
                //Manually render camera on fixed update to prevent it missing detections
                SensorCamera.Render();
            }
        }

        private void postRender()
        {
            texture.ReadPixels(new Rect(0, 0, 1, 1), 0, 0, true);
            this.Value = texture.GetPixel(0, 0);
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
                IsActive = !IsActive;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.ColorField("Output Signal", OutputSignal);
#endif
        }
    }
}