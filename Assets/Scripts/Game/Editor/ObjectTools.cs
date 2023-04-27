using UnityEditor;
using UnityEngine;

public class ObjectTools : EditorWindow
{
    private GameObject selectedObject;

    [MenuItem("Window/Object Tools")]
    public static void ShowWindow()
    {
        GetWindow<ObjectTools>("Object Tools");
    }

    private void OnGUI()
    {
        GUILayout.Label("Selected Object", EditorStyles.boldLabel);
        selectedObject = (GameObject)EditorGUILayout.ObjectField(selectedObject, typeof(GameObject), true);

        if (selectedObject != null)
        {
            GUILayout.Label("Object Tools", EditorStyles.boldLabel);
            
            if (GUILayout.Button("Stick to Surface"))
            {
                ObjectManager objectManager = GetObjectManager();
                objectManager.StickObjectToSurface(selectedObject);
            }

            if (GUILayout.Button("Align to Surface"))
            {
                ObjectManager objectManager = GetObjectManager();
                objectManager.AlignObjectToSurface(selectedObject);
            }
        }
    }

    private ObjectManager GetObjectManager()
    {
        ObjectManager objectManager = FindObjectOfType<ObjectManager>();

        if (objectManager == null)
        {
            Debug.LogError("No ObjectManager found in the scene.");
        }

        return objectManager;
    }
}
