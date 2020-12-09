﻿#if UNITY_EDITOR || UNITY_EDITOR_BETA
using UnityEngine;
using UnityEditor;
using u040.prespective.utility.editor;
using u040.prespective.prepair.inspector;
using System.Reflection;

namespace u040.prespective.referenceobjects.sensors.colorsensor.editor
{
    [ObfuscationAttribute(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    [CustomEditor(typeof(ColorDetector))]
    public class ColorDetectorEditor : PrespectiveEditor
    {
        private ColorDetector component;
        private SerializedObject soTarget;
        private SerializedProperty toolbarTab;
        private SerializedProperty signalHigh;
        private SerializedProperty signalLow;

        private void OnEnable()
        {
            component = (ColorDetector)target;
            soTarget = new SerializedObject(target);
            toolbarTab = soTarget.FindProperty("toolbarTab");
            signalHigh = soTarget.FindProperty("onSignalHigh");
            signalLow = soTarget.FindProperty("onSignalLow");
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
                    component.ShowQuantitativeSensorValuesInspector(true);

                    EditorGUILayout.LabelField("Match Percentage", (component.MatchFactor * 100f).ToString());
                    break;

                case 1:
                    EditorGUI.BeginDisabledGroup(Application.isPlaying);
                    component.ColorSensor = (ColorSensor)EditorGUILayout.ObjectField("Color Sensor", component.ColorSensor, typeof(ColorSensor), true);
                    component.ReferenceColor = EditorGUILayout.ColorField("Reference Color", component.ReferenceColor);
                    component.Threshold = EditorGUILayout.Slider("Threshold", (component.Threshold * 100f), 0f, 100f) / 100f;
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(signalHigh, true);
                    EditorGUILayout.PropertyField(signalLow, true);

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
    }
}
#endif