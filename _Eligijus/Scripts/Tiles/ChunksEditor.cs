using System.Collections;
using System.Collections.Generic;
// using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(GameTileMap))]
public class ChunksEditor : Editor
{
 
    public override void OnInspectorGUI()
    {
        GameTileMap gameTileMap = (GameTileMap) target;
        base.OnInspectorGUI();
        
        // if (GUI.changed)
        // {
        //     EditorUtility.SetDirty(target);
        //     EditorSceneManager.MarkSceneDirty(soundManager.gameObject.scene);
        // }
        //
        // if (SoundsData.Instance == null && soundManager.SoundsData != null)
        // {
        //     SoundsData.Instance = soundManager.SoundsData;
        // }

        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("GenerateChunks"))
        {
            gameTileMap.GenerateChunks();  
        }
                
        if (GUILayout.Button("ClearChuncks"))
        {
            gameTileMap.ResetChunks();  
        }

        GUILayout.EndHorizontal();
        
    }

}
#endif