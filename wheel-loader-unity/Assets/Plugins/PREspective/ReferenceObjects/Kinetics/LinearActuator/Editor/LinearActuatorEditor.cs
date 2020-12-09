#if UNITY_EDITOR || UNITY_EDITOR_BETA
using UnityEngine;
using UnityEditor;
using u040.prespective.utility.editor;
using u040.prespective.prepair.inspector;
using System.Reflection;
using u040.prespective.prepair.kinematics;

namespace u040.prespective.referenceobjects.kinetics.motor.linearactuator.editor
{
    [ObfuscationAttribute(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    [CustomEditor(typeof(LinearActuator))]
    public class LinearActuatorEditor : PrespectiveEditor
    {
        private LinearActuator component;
        private SerializedObject soTarget;
        private SerializedProperty toolbarTab;

        private void OnEnable()
        {
            component = (LinearActuator)target;
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
                    EditorGUILayout.LabelField("Target (%)", component.Target.ToString());
                    EditorGUILayout.LabelField("Position (%)", component.Position.ToString());
                    break;

                case 1:
                    EditorGUI.BeginDisabledGroup(Application.isPlaying);

                    component.PrismaticJoint = (PrismaticJoint)EditorGUILayout.ObjectField("PrismaticJoint", component.PrismaticJoint, typeof(PrismaticJoint), true);
                    component.InvertPosition = EditorGUILayout.Toggle("Invert Position", component.InvertPosition);
                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("Extending", EditorStyles.boldLabel);
                    component.ExtendingMoveSpeed = EditorGUILayout.FloatField("Move Speed (m/s)", component.ExtendingMoveSpeed);
                    component.ExtendingCycleTime = EditorGUILayout.FloatField("Cycle Time (s)", component.ExtendingCycleTime);
                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("Retracting", EditorStyles.boldLabel);
                    component.RetractingMoveSpeed = EditorGUILayout.FloatField("Move Speed (m/s)", component.RetractingMoveSpeed);
                    component.RetractingCycleTime = EditorGUILayout.FloatField("Cycle Time (s)", component.RetractingCycleTime);
                    
                    EditorGUI.EndDisabledGroup();
                    break;

                case 2:
                    ControlPanelInterface.ShowGenerationButtonForComponent(component);
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




