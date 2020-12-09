#if UNITY_EDITOR || UNITY_EDITOR_BETA
using UnityEditor;
using u040.prespective.utility.editor;
using System.Reflection;
using u040.prespective.prepair.kinematics;
using UnityEngine;

namespace u040.prespective.referenceobjects.materialhandling.gripper.editor
{
    [ObfuscationAttribute(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    [CustomEditor(typeof(AngularGripperFinger))]
    public class AngularGripperFingerEditor : PrespectiveEditor
    {
        private AngularGripperFinger component;
        private SerializedObject soTarget;

        private void OnEnable()
        {
            component = (AngularGripperFinger)target;
            soTarget = new SerializedObject(target);
        }

        public override void OnInspectorGUI()
        {
            soTarget.Update();
            //DrawDefaultInspector();

            EditorGUI.BeginChangeCheck();
            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            component.WheelJoint = (WheelJoint)EditorGUILayout.ObjectField("Wheel Joint", component.WheelJoint, typeof(WheelJoint), true);
            EditorGUI.indentLevel++;
            component.LowerLimit = EditorGUILayout.FloatField("Lower Limit (deg)", component.LowerLimit);
            component.UpperLimit = EditorGUILayout.FloatField("Upper Limit (deg)", component.UpperLimit);
            EditorGUI.indentLevel--;
            EditorGUI.EndDisabledGroup();

            component.ShowBaseInspector();

            if (EditorGUI.EndChangeCheck())
            {
                soTarget.ApplyModifiedProperties();
            }
            base.OnInspectorGUI();
        }
    }
}
#endif