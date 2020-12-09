using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace WSMGameStudio.Vehicles
{
    [CustomEditor(typeof(WSMVehicleCamera))]
    public class WSMVehicleCameraInspector : Editor
    {
        private WSMVehicleCamera _vehicleCamera;

        protected SerializedProperty _target;
        protected SerializedProperty _cameraType;

        private GUIStyle _menuBoxStyle;

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            _vehicleCamera = target as WSMVehicleCamera;

            //Set up the box style if null
            if (_menuBoxStyle == null)
            {
                _menuBoxStyle = new GUIStyle(GUI.skin.box);
                _menuBoxStyle.normal.textColor = GUI.skin.label.normal.textColor;
                _menuBoxStyle.fontStyle = FontStyle.Bold;
                _menuBoxStyle.alignment = TextAnchor.UpperLeft;
            }
            GUILayout.BeginVertical(_menuBoxStyle);

            GUILayout.Label("CAMERA SETTINGS", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();
            bool cameraToggleEnabled = EditorGUILayout.Toggle("Camera Toggle Enabled", _vehicleCamera.CameraToggleEnabled);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_vehicleCamera, "Changed Camera Toggle Enabled");
                _vehicleCamera.CameraToggleEnabled = cameraToggleEnabled;
                MarkSceneAlteration();
            }

            serializedObject.Update();
            _target = serializedObject.FindProperty("_target");
            _cameraType = serializedObject.FindProperty("_cameraType");
            EditorGUILayout.PropertyField(_target);
            EditorGUILayout.PropertyField(_cameraType);
            serializedObject.ApplyModifiedProperties();

            GUILayout.Label("CAMERA TYPE SETTINGS", EditorStyles.boldLabel);

            if (_vehicleCamera.CameraType == WSMVehicleCameraType.TPS)
            {
                EditorGUI.BeginChangeCheck();
                float tpsHeight = EditorGUILayout.FloatField("TPS Height", _vehicleCamera.TpsHeight);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleCamera, "Changed TPS Height");
                    _vehicleCamera.TpsHeight = tpsHeight;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                float tpsDistance = EditorGUILayout.FloatField("TPS Distance", _vehicleCamera.TpsDistance);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleCamera, "Changed TPS Distance");
                    _vehicleCamera.TpsDistance = tpsDistance;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                float tpsRotationSpeed = EditorGUILayout.FloatField("TPS Rotation Speed", _vehicleCamera.TpsRotationSpeed);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleCamera, "Changed TPS Rotation Speed");
                    _vehicleCamera.TpsRotationSpeed = tpsRotationSpeed;
                    MarkSceneAlteration();
                }
            }
            else if (_vehicleCamera.CameraType == WSMVehicleCameraType.FPS)
            {
                EditorGUI.BeginChangeCheck();
                float fpsRotationSpeed = EditorGUILayout.FloatField("FPS Rotation Speed", _vehicleCamera.FpsRotationSpeed);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleCamera, "Changed FPS Rotation Speed");
                    _vehicleCamera.FpsRotationSpeed = fpsRotationSpeed;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                float fpsHorizontalAngleLimit = EditorGUILayout.FloatField("FPS Horizontal Angle Limit", _vehicleCamera.FpsHorizontalAngleLimit);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleCamera, "Changed FPS Horizontal Angle Limit");
                    _vehicleCamera.FpsHorizontalAngleLimit = fpsHorizontalAngleLimit;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                float fpsVerticalAngleLimit = EditorGUILayout.FloatField("FPS Vertical Angle Limit", _vehicleCamera.FpsVerticalAngleLimit);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleCamera, "Changed FPS Vertical Angle Limit");
                    _vehicleCamera.FpsVerticalAngleLimit = fpsVerticalAngleLimit;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                Vector3 fpsDefaultLookPosition = EditorGUILayout.Vector3Field("FPS Default Look Position", _vehicleCamera.FpsDefaultLookPosition);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleCamera, "Changed Default Look Position");
                    _vehicleCamera.FpsDefaultLookPosition = fpsDefaultLookPosition;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                Vector3 fpsSideLookPosition = EditorGUILayout.Vector3Field("FPS Side Look Position", _vehicleCamera.FpsSideLookPosition);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleCamera, "Changed Side Look Position");
                    _vehicleCamera.FpsSideLookPosition = fpsSideLookPosition;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                Vector3 fpsLookBackPosition = EditorGUILayout.Vector3Field("FPS Look Back Position", _vehicleCamera.FpsLookBackPosition);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleCamera, "Changed Look Back Position");
                    _vehicleCamera.FpsLookBackPosition = fpsLookBackPosition;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                Vector3 fpsLookDownPosition = EditorGUILayout.Vector3Field("FPS Look Down Position", _vehicleCamera.FpsLookDownPosition);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleCamera, "Changed Look Down Position");
                    _vehicleCamera.FpsLookDownPosition = fpsLookDownPosition;
                    MarkSceneAlteration();
                }
            }
            else if (_vehicleCamera.CameraType == WSMVehicleCameraType.TopDown)
            {
                EditorGUI.BeginChangeCheck();
                bool topDownOrthographicCam = EditorGUILayout.Toggle("Orthographic Cam", _vehicleCamera.TopDownOrthographicCam);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleCamera, "Changed Top Down Orthographic Cam");
                    _vehicleCamera.TopDownOrthographicCam = topDownOrthographicCam;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                Vector3 topPositionOffset = EditorGUILayout.Vector3Field("Top Down Position Offset", _vehicleCamera.TopPositionOffset);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_vehicleCamera, "Changed Top Down Position Offset");
                    _vehicleCamera.TopPositionOffset = topPositionOffset;
                    MarkSceneAlteration();
                }
            }

            GUILayout.Label("CAMERA OPERATIONS", EditorStyles.boldLabel);

            if (GUILayout.Button("Move to Start Position"))
            {
                _vehicleCamera.MoveToStartPosition();
                MarkSceneAlteration();
            }

            GUILayout.EndVertical();
        }

            private void MarkSceneAlteration()
        {
            if (!Application.isPlaying)
            {
#if UNITY_EDITOR
                EditorUtility.SetDirty(_vehicleCamera);
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
#endif
            }
        }
    } 
}
