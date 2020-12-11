using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WSMGameStudio.HeavyMachinery
{
    [CustomEditor(typeof(RotationModerator))]
    public class RotationModeratorInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.Label("SETTINGS", EditorStyles.boldLabel);
            DrawDefaultInspector();

            RotationModerator myScript = (RotationModerator)target;
            
            GUILayout.Label("SETTINGS OPERATIONS", EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();
            
            // Bucket Pivot
            GUILayout.BeginVertical();
            GUILayout.Label("Bucket Pivot");
            if(GUILayout.Button("Set Current as Origin"))
            {
                myScript.BucketPivotSetCurrentAsOrigin();
            }
            
            if(GUILayout.Button("Back to Origin"))
            {
                myScript.BucketPivotBackToOrigin();
            }
            GUILayout.EndVertical();

            // Loader Frame
            GUILayout.BeginVertical();
            GUILayout.Label("Loader Frame");
            if(GUILayout.Button("Set Current as Origin"))
            {
                myScript.LoaderFrameSetCurrentAsOrigin();
            }
            
            if(GUILayout.Button("Back to Origin"))
            {
                myScript.LoaderFrameBackToOrigin();
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            // BOTH
            GUILayout.BeginVertical();
            GUILayout.Label("Both");
            if(GUILayout.Button("Set Current as Origin"))
            {
                myScript.LoaderFrameSetCurrentAsOrigin();
                myScript.BucketPivotSetCurrentAsOrigin();
            }
            
            if(GUILayout.Button("Back to Origin"))
            {
                myScript.LoaderFrameBackToOrigin();
                myScript.BucketPivotBackToOrigin();
            }
            GUILayout.EndVertical();
        }
    }
}
