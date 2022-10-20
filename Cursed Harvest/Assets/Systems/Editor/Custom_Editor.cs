using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Custom_Editor : Editor
{
    [MenuItem("GameObject/Custom/Custom Object", false, 10)]
    static void Create_CustomObject(MenuCommand menuCommand)
    {
        GameObject prefab = new GameObject();
        // Create a custom game object
        GameObject go = Instantiate(prefab);
        go.name = "Custom Object";
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }
}
