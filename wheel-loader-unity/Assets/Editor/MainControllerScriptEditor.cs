using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.Rendering;

[CustomEditor(typeof(MainController))]
public class MainControllerScript : Editor
{
    private int _currentBar;
    private List<bool> _foldList = Enumerable.Repeat(true,10).ToList();
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        MainController t = (MainController)target;
        
        _currentBar = GUILayout.Toolbar(_currentBar, new string[] {"Settings", "Tuning"});
        switch (_currentBar)
        {
            case 0:
                DrawDefaultInspector();
                break;
            case 1:
                for (int i = 0; i < t.baseControllerList.Count; i++)
                {
                    var controller = t.baseControllerList[i];
                    
                    _foldList[i] = EditorGUILayout.BeginFoldoutHeaderGroup(_foldList[i], controller.name);
                    if (_foldList[i])
                    {
                        SerializedObject so = new SerializedObject(controller);
                        SerializedProperty prop = so.GetIterator();
                        if (prop.NextVisible(true)) {
                            do
                            {
                                // Debug.Log("prop name - " + prop.name);
                                if (prop.name == "m_Script") continue;
                                EditorGUILayout.PropertyField(so.FindProperty(prop.name), true);
                            }
                            while (prop.NextVisible(false));
                        }
	
                        so.ApplyModifiedProperties();
                    }
                    EditorGUILayout.EndFoldoutHeaderGroup();
                }

                break;
        }
    }
}
