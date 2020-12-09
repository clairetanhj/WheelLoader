#if UNITY_EDITOR || UNITY_EDITOR_BETA
using UnityEditor;
using u040.prespective.utility.editor;
using System.Reflection;
using u040.prespective.prepair.kinematics;
using UnityEngine;

namespace u040.prespective.referenceobjects.materialhandling.gripper.editor
{
    [ObfuscationAttribute(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    [CustomEditor(typeof(ParallelGripperFinger))]
    public class ParallelGripperFingerEditor : PrespectiveEditor
    {
        private ParallelGripperFinger component;
        private SerializedObject soTarget;

        private void OnEnable()
        {
            component = (ParallelGripperFinger)target;
            soTarget = new SerializedObject(target);
        }


        public override void OnInspectorGUI()
        {
            soTarget.Update();
            //DrawDefaultInspector();

            EditorGUI.BeginChangeCheck();
            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            component.PrismaticJoint = (PrismaticJoint)EditorGUILayout.ObjectField("Prismatic Joint", component.PrismaticJoint, typeof(PrismaticJoint), true);
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