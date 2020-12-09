using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace WSMGameStudio.HeavyMachinery
{
    [CustomEditor(typeof(MovingMechanicalPart))]
    public class MovingMechanicalPartInspector : Editor
    {
        private MovingMechanicalPart _mechanicalPart;

        protected SerializedProperty _reachedMin;
        protected SerializedProperty _reachedMax;
        protected SerializedProperty _startedMovement;
        protected SerializedProperty _finishedMovement;

        private int _selectedMenuIndex = 0;
        private string[] _toolbarMenuOptions = new[] { "Settings", "Events" };
        private GUIStyle _menuBoxStyle;

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            _mechanicalPart = target as MovingMechanicalPart;

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
                float movementInput = EditorGUILayout.Slider("Position Input", _mechanicalPart.MovementInput, 0f, 1f);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_mechanicalPart, "Changed Input");
                    _mechanicalPart.MovementInput = movementInput;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                MovingMode movingMode = (MovingMode)EditorGUILayout.EnumPopup("Moving Mode", _mechanicalPart.MovingMode);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_mechanicalPart, "Changed Moving Mode");
                    _mechanicalPart.MovingMode = movingMode;
                    MarkSceneAlteration();
                }

                if (_mechanicalPart.MovingMode == MovingMode.Function)
                {
                    EditorGUI.BeginChangeCheck();
                    AnimationCurve movementFunction = EditorGUILayout.CurveField("Movement Function", _mechanicalPart.MovementFunction);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(_mechanicalPart, "Changed Movement Function");
                        _mechanicalPart.MovementFunction = movementFunction;
                        MarkSceneAlteration();
                    } 
                }

                EditorGUI.BeginChangeCheck();
                Vector3 minPosition = EditorGUILayout.Vector3Field("Min Position", _mechanicalPart.Min);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_mechanicalPart, "Changed Input");
                    _mechanicalPart.Min = minPosition;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                Vector3 maxPosition = EditorGUILayout.Vector3Field("Max Position", _mechanicalPart.Max);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_mechanicalPart, "Changed Input");
                    _mechanicalPart.Max = maxPosition;
                    MarkSceneAlteration();
                }

                EditorGUI.BeginChangeCheck();
                Vector3 defaultPosition = EditorGUILayout.Vector3Field("Default Position", _mechanicalPart.Default);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_mechanicalPart, "Changed Input");
                    _mechanicalPart.Default = defaultPosition;
                    MarkSceneAlteration();
                }

                GUILayout.Label("SETTINGS OPERATIONS", EditorStyles.boldLabel);

                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                if (GUILayout.Button("Set Current as Min"))
                {
                    _mechanicalPart.SetCurrentAsMin();
                    MarkSceneAlteration();
                }

                if (GUILayout.Button("Reset to Min"))
                {
                    _mechanicalPart.ResetToMin();
                    MarkSceneAlteration();
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                if (GUILayout.Button("Set Current as Max"))
                {
                    _mechanicalPart.SetCurrentAsMax();
                    MarkSceneAlteration();
                }

                if (GUILayout.Button("Reset to Max"))
                {
                    _mechanicalPart.ResetToMax();
                    MarkSceneAlteration();
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                if (GUILayout.Button("Set Current as Default"))
                {
                    _mechanicalPart.SetCurrentAsDefault();
                    MarkSceneAlteration();
                }

                if (GUILayout.Button("Reset to Default"))
                {
                    _mechanicalPart.ResetToDefault();
                    MarkSceneAlteration();
                }
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
            else if (_toolbarMenuOptions[_selectedMenuIndex] == "Events")
            {
                GUILayout.Label("EVENTS", EditorStyles.boldLabel);

                serializedObject.Update();

                _reachedMin = serializedObject.FindProperty("_reachedMin");
                _reachedMax = serializedObject.FindProperty("_reachedMax");
                _startedMovement = serializedObject.FindProperty("_startedMovement");
                _finishedMovement = serializedObject.FindProperty("_finishedMovement");

                EditorGUILayout.PropertyField(_reachedMin, new GUIContent("Reached Min"));
                EditorGUILayout.PropertyField(_reachedMax, new GUIContent("Reached Max"));
                EditorGUILayout.PropertyField(_startedMovement, new GUIContent("Started Movement"));
                EditorGUILayout.PropertyField(_finishedMovement, new GUIContent("Finished Movement"));
                serializedObject.ApplyModifiedProperties();
            }

            GUILayout.EndVertical();
        }

        private void MarkSceneAlteration()
        {
            if (!Application.isPlaying)
            {
#if UNITY_EDITOR
                EditorUtility.SetDirty(_mechanicalPart);
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
#endif
            }
        }
    } 
}
