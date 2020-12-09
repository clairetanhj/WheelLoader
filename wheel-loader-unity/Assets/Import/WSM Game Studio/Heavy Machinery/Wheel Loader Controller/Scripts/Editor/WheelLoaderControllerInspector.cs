using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace WSMGameStudio.HeavyMachinery
{
    [CustomEditor(typeof(WheelLoaderController))]
    public class WheelLoaderControllerInspector : Editor
    {
        private WheelLoaderController _wheelLoaderController;

        protected SerializedProperty _loaderFrame;
        protected SerializedProperty _bucket;
        protected SerializedProperty _bellCrank;
        protected SerializedProperty _loaderFrameLever;
        protected SerializedProperty _bucketLever;
        protected SerializedProperty _partsMovingSFX;
        protected SerializedProperty _partsStartMovingSFX;
        protected SerializedProperty _partsStopMovingSFX;

        private int _selectedMenuIndex = 0;
        private string[] _toolbarMenuOptions = new[] { "Settings", "Mechanical Parts", "SFX" };
        private GUIStyle _menuBoxStyle;

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            _wheelLoaderController = target as WheelLoaderController;

            EditorGUI.BeginChangeCheck();
            _selectedMenuIndex = GUILayout.Toolbar(_selectedMenuIndex, _toolbarMenuOptions);
            if (EditorGUI.EndChangeCheck())
            {
                GUI.FocusControl(null);
            }

            //Set up the box style if null
            if (_menuBoxStyle == null)
            {
                _menuBoxStyle = new GUIStyle(GUI.skin.box);
                _menuBoxStyle.normal.textColor = GUI.skin.label.normal.textColor;
                _menuBoxStyle.fontStyle = FontStyle.Bold;
                _menuBoxStyle.alignment = TextAnchor.UpperLeft;
            }
            GUILayout.BeginVertical(_menuBoxStyle);

            if (_toolbarMenuOptions[_selectedMenuIndex] == "Settings")
            {
                /*
                 * SETTINGS
                 */
                GUILayout.Label("SETTINGS", EditorStyles.boldLabel);

                EditorGUI.BeginChangeCheck();
                bool isEngineOn = EditorGUILayout.Toggle("Engine On", _wheelLoaderController.IsEngineOn);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_wheelLoaderController, "Toggled Engine On");
                    _wheelLoaderController.IsEngineOn = isEngineOn;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                float loaderFrameSpeed = EditorGUILayout.FloatField("Loader Frame Speed", _wheelLoaderController.loaderFrameSpeed);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_wheelLoaderController, "Changed Loader Frame Speed");
                    _wheelLoaderController.loaderFrameSpeed = loaderFrameSpeed;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                float bucketSpeed = EditorGUILayout.FloatField("Bucket Speed", _wheelLoaderController.bucketSpeed);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_wheelLoaderController, "Changed Bucket Speed");
                    _wheelLoaderController.bucketSpeed = bucketSpeed;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                LevelingMode levelingMode = (LevelingMode)EditorGUILayout.EnumPopup("Leveling Mode", _wheelLoaderController.levelingMode);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_wheelLoaderController, "Changed Leveling Mode");
                    _wheelLoaderController.levelingMode = levelingMode;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                float selfLevelingSpeed = EditorGUILayout.FloatField("Self Leveling Speed", _wheelLoaderController.selfLevelingSpeed);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_wheelLoaderController, "Changed Self Leveling Speed");
                    _wheelLoaderController.selfLevelingSpeed = selfLevelingSpeed;
                    MarkSceneAlteration();
                }
            }
            else if (_toolbarMenuOptions[_selectedMenuIndex] == "Mechanical Parts")
            {
                /*
                 * MECHANICAL PARTS
                 */
                serializedObject.Update();

                GUILayout.Label("MECHANICAL PARTS", EditorStyles.boldLabel);

                _loaderFrame = serializedObject.FindProperty("loaderFrame");
                _bucket = serializedObject.FindProperty("bucket");
                _bellCrank = serializedObject.FindProperty("bellCrank");
                _loaderFrameLever = serializedObject.FindProperty("loaderFrameLever");
                _bucketLever = serializedObject.FindProperty("bucketLever");

                EditorGUILayout.PropertyField(_loaderFrame, new GUIContent("Loader Frame"));
                EditorGUILayout.PropertyField(_bucket, new GUIContent("Bucket"));
                EditorGUILayout.PropertyField(_bellCrank, new GUIContent("Bell Crank"));

                GUILayout.Label("LEVERS", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_loaderFrameLever, new GUIContent("Frame Lever"));
                EditorGUILayout.PropertyField(_bucketLever, new GUIContent("Bucket Lever"));

                serializedObject.ApplyModifiedProperties();
            }
            else if (_toolbarMenuOptions[_selectedMenuIndex] == "SFX")
            {
                /*
                 * SFX
                 */
                GUILayout.Label("AUDIO SOURCES", EditorStyles.boldLabel);

                serializedObject.Update();

                _partsMovingSFX = serializedObject.FindProperty("partsMovingSFX");
                _partsStartMovingSFX = serializedObject.FindProperty("partsStartMovingSFX");
                _partsStopMovingSFX = serializedObject.FindProperty("partsStopMovingSFX");

                EditorGUILayout.PropertyField(_partsMovingSFX, new GUIContent("Moving SFX"));
                EditorGUILayout.PropertyField(_partsStartMovingSFX, new GUIContent("Starts Moving SFX"));
                EditorGUILayout.PropertyField(_partsStopMovingSFX, new GUIContent("Stops Moving SFX"));

                serializedObject.ApplyModifiedProperties();
            }

            GUILayout.EndVertical();
        }

        private void MarkSceneAlteration()
        {
            if (!Application.isPlaying)
            {
#if UNITY_EDITOR
                EditorUtility.SetDirty(_wheelLoaderController);
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
#endif
            }
        }
    }
}
