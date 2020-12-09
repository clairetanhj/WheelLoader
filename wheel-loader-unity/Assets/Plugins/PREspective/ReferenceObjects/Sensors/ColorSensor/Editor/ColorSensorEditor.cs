#if UNITY_EDITOR || UNITY_EDITOR_BETA
using UnityEngine;
using UnityEditor;
using u040.prespective.utility.editor;
using u040.prespective.prepair.inspector;
using System.Reflection;

namespace u040.prespective.referenceobjects.sensors.colorsensor.editor
{
    [ObfuscationAttribute(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    [CustomEditor(typeof(ColorSensor))]
    public class ColorSensorEditor : PrespectiveEditor
    {
        private ColorSensor component;
        private SerializedObject soTarget;
        private SerializedProperty toolbarTab;
        private SerializedProperty valueChanged;

        private void OnEnable()
        {
            component = (ColorSensor)target;
            soTarget = new SerializedObject(target);
            toolbarTab = soTarget.FindProperty("toolbarTab");
            valueChanged = soTarget.FindProperty("onValueChanged");
        }

        public override void OnInspectorGUI()
        {
            soTarget.Update();
            //DrawDefaultInspector();

            EditorGUI.BeginChangeCheck();
            toolbarTab.intValue = GUILayout.Toolbar(toolbarTab.intValue, new string[] { "Live Data", "Properties", "Control Panel" });

            switch (toolbarTab.intValue)
            {
                case 0:
                    //Create a style for Active or Inactive
                    GUIStyle _labelStyle = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold };
                    _labelStyle.normal.textColor = component.IsActive ? new Color(0f, 0.5f, 0f) : Color.red;

                    PrespectiveEditor.BoolLabel("State", component.IsActive, "Active", "Inactive", _labelStyle);
                    EditorGUILayout.ColorField("Output Signal", component.OutputSignal);
                    break;

                case 1:
                    EditorGUI.BeginDisabledGroup(Application.isPlaying);
                    component.Range = EditorGUILayout.Vector2Field(new GUIContent("Range", "The distances between which the sensor can detect colors"), component.Range);
                    component.VoidColor = EditorGUILayout.ColorField(new GUIContent("Void Color", "The color the component 'detects' when there is nothing within range"), component.VoidColor);
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(valueChanged, true);

                    if (component.FixedRendering) { EditorGUILayout.HelpBox("Fixed Rendering currently demands a lot of resources. Using multiple ColorSensors with FixedRendering could slow your system down drastically.", MessageType.Warning); }
                    component.FixedRendering = EditorGUILayout.Toggle("Fixed Rendering", component.FixedRendering);
                    EditorGUI.EndDisabledGroup();
                    break;

                case 2:
                    ControlPanelInterface.ShowGenerationButtonForComponent(component);
                    break;
            }

            EditorUtility.SetDirty(target); //Make sure inspector updates and repaints properly 

            if (EditorGUI.EndChangeCheck())
            {
                soTarget.ApplyModifiedProperties(); 
            }
            base.OnInspectorGUI();
        }

        private void OnSceneGUI()
        {
            if (component.Range.x < component.Range.y)
            {
                //Draw Range Indicator
                Transform _transform = component.SensorCamera != null ? component.SensorCamera.transform : component.transform;

                Handles.color = Color.red;
                Vector3 _origin = _transform.position;
                Vector3 _rangeStart = _origin + (_transform.forward * component.Range.x);
                Vector3 _rangeEnd = _origin + (_transform.forward * component.Range.y);
                Handles.DrawLine(_rangeStart, _rangeEnd);
                float _handleSize = HandleUtility.GetHandleSize(_transform.position);
                Handles.DotHandleCap(0, _rangeStart, Quaternion.identity, 0.05f * _handleSize, EventType.Repaint);
                Handles.DotHandleCap(0, _rangeEnd, Quaternion.identity, 0.05f * _handleSize, EventType.Repaint);
            }
        }
    }
}
#endif