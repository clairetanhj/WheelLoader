#if UNITY_EDITOR || UNITY_EDITOR_BETA
using UnityEngine;
using UnityEditor;
using u040.prespective.utility.editor;
using u040.prespective.prepair.inspector;
using System.Reflection;

namespace u040.prespective.referenceobjects.sensors.beamsensor.editor
{
    [ObfuscationAttribute(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    [CustomEditor(typeof(BeamReceiver))]
    public class BeamReceiverEditor : PrespectiveEditor
    {
        private BeamReceiver component;
        private SerializedObject soTarget;
        private SerializedProperty toolbarTab;
        private SerializedProperty signalLow;
        private SerializedProperty signalHigh;


        private void OnEnable()
        {
            component = (BeamReceiver)target;
            soTarget = new SerializedObject(target);
            toolbarTab = soTarget.FindProperty("toolbarTab");
            signalLow = soTarget.FindProperty("onSignalLow");
            signalHigh = soTarget.FindProperty("onSignalHigh");
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
                    component.ShowQuantitativeSensorValuesInspector(true);

                    break;

                case 1:
                    EditorGUI.BeginDisabledGroup(Application.isPlaying);
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