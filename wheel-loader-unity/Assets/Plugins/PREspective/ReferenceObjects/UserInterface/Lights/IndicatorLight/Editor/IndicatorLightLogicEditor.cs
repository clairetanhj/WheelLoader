#if UNITY_EDITOR || UNITY_EDITOR_BETA
using UnityEngine;
using UnityEditor;
using u040.prespective.utility.editor;
using System.Reflection;

namespace u040.prespective.referenceobjects.userinterface.lights.editor
{
    [ObfuscationAttribute(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    [CustomEditor(typeof(IndicatorLightLogic))]
    public class IndicatorLightLogicEditor : PrespectiveEditor
    {
        private IndicatorLightLogic component;
        private SerializedObject soTarget;
        private SerializedProperty toolbarTab;
        private SerializedProperty signalNamingRuleOverrides;
        private SerializedProperty implicitNamingRule;


        private void OnEnable()
        {
            component = (IndicatorLightLogic)target;
            soTarget = new SerializedObject(target);
            toolbarTab = soTarget.FindProperty("toolbarTab");
            signalNamingRuleOverrides = soTarget.FindProperty("signalNamingRuleOverrides");
            implicitNamingRule = soTarget.FindProperty("implicitNamingRule");
        }

        public override void OnInspectorGUI()
        {
            soTarget.Update();
            //DrawDefaultInspector();

            if (component.IndicatorLight == null)
            {
                EditorGUILayout.HelpBox("No IndicatorLight has been set. This component will not function properly untill all required components have been assigned. You can do this in the Properties tab.", MessageType.Warning);
            }

            EditorGUI.BeginChangeCheck();
            toolbarTab.intValue = GUILayout.Toolbar(toolbarTab.intValue, new string[] { "Properties", "I/O", "Settings", "Debugging" });

            switch (toolbarTab.intValue)
            {
                case 0:
                    if (Application.isPlaying) //Make sure motor physical properties cannot be editted during playmode
                    {
                        if (component.IndicatorLight == null)
                        {
                            EditorGUILayout.LabelField("No IndicatorLight has been set so no properties can be shown.");
                        }
                        else
                        {
                            EditorGUILayout.LabelField("IndicatorLight", component.IndicatorLight.ToString());
                        }
                    }
                    else
                    {
                        component.IndicatorLight = (IndicatorLight)EditorGUILayout.ObjectField("IndicatorLight", component.IndicatorLight, typeof(IndicatorLight), true);
                    }
                    break;

                case 1:
                    //Header
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Property", "Input", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField("Output", EditorStyles.boldLabel);
                    EditorGUILayout.EndHorizontal();

                    //Outputs
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Active");
                    EditorGUILayout.LabelField(component.oActive.ToString());
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Intensity");
                    EditorGUILayout.LabelField(component.oIntensity.ToString());
                    EditorGUILayout.EndHorizontal();
                    break;

                case 2:
                    EditorGUILayout.PropertyField(signalNamingRuleOverrides, true);
                    EditorGUILayout.PropertyField(implicitNamingRule, true);

                    break;

                case 3:
                    component.VERBOSE = EditorGUILayout.Toggle("VERBOSE", component.VERBOSE);
                    component.UXShowSignalsForDebugging = EditorGUILayout.Toggle("UX Show Signals For Debugging", component.UXShowSignalsForDebugging);
                    component.UXTextOffset = EditorGUILayout.Vector2Field("UX Text Offset", component.UXTextOffset);
                    component.UXTextColor = EditorGUILayout.ColorField("UX Text Color", component.UXTextColor);
                    component.UXTextSize = EditorGUILayout.IntField("UX Text Size", component.UXTextSize);
                    component.UXTextLineSpacing = EditorGUILayout.IntField("UX Text Line Spacing", component.UXTextLineSpacing);
                    break;
            }

            if (EditorGUI.EndChangeCheck())
            {
                soTarget.ApplyModifiedProperties();
            }
            base.OnInspectorGUI();
        }

    }
}
#endif