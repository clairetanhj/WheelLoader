#if UNITY_EDITOR || UNITY_EDITOR_BETA
using UnityEngine;
using UnityEditor;
using u040.prespective.utility.editor;
using u040.prespective.prepair.inspector;
using System.Reflection;

namespace u040.prespective.referenceobjects.sensors.beamsensor.editor
{
    [ObfuscationAttribute(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    [CustomEditor(typeof(BeamEmitter))]
    public class BeamEmitterEditor : PrespectiveEditor
    {
        private BeamEmitter component;
        private SerializedObject soTarget;
        private SerializedProperty toolbarTab;

        private void OnEnable()
        {
            component = (BeamEmitter)target;
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
                    EditorGUILayout.LabelField(new GUIContent("Redirection Limit Reached"), new GUIContent((!component.BeamCompleted).ToString()));
                    break;

                case 1:
                    if (Application.isPlaying) //Make sure motor physical properties cannot be editted during playmode
                    {
                        EditorGUILayout.LabelField("Reach", component.Reach.ToString());
                        EditorGUILayout.LabelField("Offset", component.PositionalOffset.ToString());
                        EditorGUILayout.LabelField("Origin direction", component.DirectionalOffset.ToString());
                        EditorGUILayout.LabelField("Max Number of Hits", component.MaxNumberOfHits.ToString());
                        EditorGUILayout.LabelField("Beam material", component.BeamMaterial.ToString());
                        EditorGUILayout.LabelField("Beam radius", component.BeamRadius.ToString());
                    }
                    else
                    {
                        component.Reach = Mathf.Max(0f, EditorGUILayout.FloatField("Reach", component.Reach));
                        component.PositionalOffset = EditorGUILayout.Vector3Field("Origin Offset", component.PositionalOffset);
                        component.DirectionalOffset = EditorGUILayout.Vector3Field("Origin direction", component.DirectionalOffset);
                        component.MaxNumberOfHits = Mathf.Max(1, EditorGUILayout.IntField("Max Number of Hits", component.MaxNumberOfHits));
                        component.BeamMaterial = (Material)EditorGUILayout.ObjectField("Beam material", component.BeamMaterial, typeof(Material), true);
                        component.BeamRadius = EditorGUILayout.FloatField("Beam radius", component.BeamRadius);
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Gizmo Settings", EditorStyles.boldLabel);
                    component.UseOriginGizmo = EditorGUILayout.Toggle("Use Origin Gizmo", component.UseOriginGizmo);
                    if (component.UseOriginGizmo)
                    {
                        component.OriginGizmoSize = EditorGUILayout.FloatField("Origin Gizmo Size", component.OriginGizmoSize);
                    }
                    component.UseBeamGizmo = EditorGUILayout.Toggle("Use Beam Gizmo", component.UseBeamGizmo);
                    if (component.UseBeamGizmo)
                    {
                        component.BeamColor = EditorGUILayout.ColorField("Beam Color", component.BeamColor);
                        component.BeamExcessColor = EditorGUILayout.ColorField("Beam Excess Color", component.BeamExcessColor);
                    }
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
            if (!Application.isPlaying)
            {
                if (component.UseOriginGizmo)
                {
                    Handles.color = component.BeamColor;
                    Handles.SphereHandleCap(0, component.OriginPosition, Quaternion.identity, HandleUtility.GetHandleSize(component.transform.position) * component.OriginGizmoSize * 0.25f, EventType.Repaint);
                    Handles.ArrowHandleCap(0, component.OriginPosition, Quaternion.FromToRotation(Vector3.forward, component.OriginDirection), HandleUtility.GetHandleSize(component.transform.position) * (component.OriginGizmoSize > 0.9f && component.OriginGizmoSize < 1.1f ? 1.1f : component.OriginGizmoSize), EventType.Repaint);
                }
            }
        }
    }
}
#endif