#if UNITY_EDITOR || UNITY_EDITOR_BETA
using UnityEngine;
using UnityEditor;
using u040.prespective.utility.editor;
using u040.prespective.prepair.inspector;
using System.Reflection;

namespace u040.prespective.referenceobjects.userinterface.lights.editor
{
    [ObfuscationAttribute(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    [CustomEditor(typeof(IndicatorLight))]
    public class IndicatorLightEditor : PrespectiveEditor
    {
        private IndicatorLight component;
        private SerializedObject soTarget;
        private SerializedProperty toolbarTab;

        private void OnEnable()
        {
            component = (IndicatorLight)target;
            soTarget = new SerializedObject(target);
            toolbarTab = soTarget.FindProperty("toolbarTab");
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
                    EditorGUILayout.LabelField(new GUIContent("Intensity", "Light intensity of the 'active' material"), new GUIContent((component.Intensity * 100f) + "%"));
                    break;

                case 1:
                    EditorGUI.BeginDisabledGroup(Application.isPlaying);
                    component.LightColor = EditorGUILayout.ColorField("Light Color", component.LightColor);
                    component.BaseColor = EditorGUILayout.ColorField("Base Color", component.BaseColor);

                    EditorGUI.indentLevel++;
                    EditorGUILayout.LabelField("Select a Material", EditorStyles.boldLabel);
                    Material[] _sharedMats = component.GetComponent<Renderer>().sharedMaterials;

                    for (int _i = 0; _i < _sharedMats.Length; _i++)
                    {
                        EditorGUILayout.BeginHorizontal();

                        //Create label
                        EditorGUILayout.LabelField(new GUIContent(" " + _sharedMats[_i].name, ColorIcon(_sharedMats[_i].color)));

                        //Create button
                        if (component.OriginalMaterialIndex == _i)
                        {
                            if (GUILayout.Button("Deselect material"))
                            {
                                component.LoadOriginalMaterial();
                            }
                        }
                        else
                        {
                            if (GUILayout.Button("Select material"))
                            {
                                component.OriginalMaterial = _sharedMats[_i];
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUI.indentLevel--;

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