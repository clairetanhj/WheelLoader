#if UNITY_EDITOR || UNITY_EDITOR_BETA
using UnityEditor;
using u040.prespective.utility.editor;
using System.Reflection;
using u040.prespective.prepair.kinematics;
using UnityEngine;

namespace u040.prespective.referenceobjects.materialhandling.gripper.editor
{
    [ObfuscationAttribute(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    [CustomEditor(typeof(VacuumGripperFinger))]
    public class VacuumGripperFingerEditor : PrespectiveEditor
    {
        private VacuumGripperFinger component;
        private SerializedObject soTarget;

        private void OnEnable()
        {
            component = (VacuumGripperFinger)target;
            soTarget = new SerializedObject(target);
        }


        public override void OnInspectorGUI()
        {
            soTarget.Update();
            //DrawDefaultInspector();
            EditorGUI.BeginChangeCheck();
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