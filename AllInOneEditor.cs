using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AllInOneEditor : EditorWindow
{
    private List<GameObject> objectsWithMissingScripts = new List<GameObject>();
    private bool foundMissingScripts = false;

    [MenuItem("Tools/Missing Scripts Cleaner")]
    public static void ShowWindow()
    {
        GetWindow<AllInOneEditor>("Missing Scripts Cleaner");
    }

    void OnGUI()
    {
        GUILayout.Label("Missing Scripts Cleaner", EditorStyles.boldLabel);

        if (GUILayout.Button("Find Missing Scripts in Scene"))
        {
            FindMissingScripts();
        }

        if (foundMissingScripts && objectsWithMissingScripts.Count > 0)
        {
            GUILayout.Label("GameObjects with Missing Scripts:");
            foreach (var obj in objectsWithMissingScripts)
            {
                EditorGUILayout.ObjectField(obj, typeof(GameObject), true);
            }

            if (GUILayout.Button("Remove All Missing Scripts"))
            {
                RemoveMissingScripts();
            }
        }
    }

    void FindMissingScripts()
    {
        objectsWithMissingScripts.Clear();
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(obj) > 0)
            {
                objectsWithMissingScripts.Add(obj);
            }
        }

        foundMissingScripts = objectsWithMissingScripts.Count > 0;
    }

    void RemoveMissingScripts()
    {
        foreach (GameObject obj in objectsWithMissingScripts)
        {
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj);
        }

        objectsWithMissingScripts.Clear();
        foundMissingScripts = false;
        EditorUtility.DisplayDialog("Cleanup Complete", "All missing scripts have been removed from the scene.", "OK");
    }
}
