using UnityEditor;
using UnityEngine;

public class TeleportSceneViewCamera : EditorWindow
{
    private Vector3 targetPosition;

    [MenuItem("Window/Teleport Scene View Camera")]
    public static void ShowWindow()
    {
        GetWindow<TeleportSceneViewCamera>("Teleport Scene View Camera");
    }

    private void OnGUI()
    {
        GUILayout.Label("Enter the target position (x, y, z):", EditorStyles.boldLabel);
        targetPosition = EditorGUILayout.Vector3Field("", targetPosition);

        if (GUILayout.Button("Teleport Camera"))
        {
            TeleportCameraToPosition(targetPosition);
        }
    }

    private void TeleportCameraToPosition(Vector3 position)
    {
        if (SceneView.lastActiveSceneView != null)
        {
            SceneView.lastActiveSceneView.pivot = position;
            SceneView.lastActiveSceneView.size = 10f;
            SceneView.lastActiveSceneView.Repaint();
        }
    }
}
