using UnityEditor;
using UnityEngine;

public class GlowEditorWindow : EditorWindow
{
    private GameObject prefab;
    private GlowingObject glowingObject;
    private MaterialPropertyBlock propertyBlock;
    private Renderer objectRenderer;
    private Color emissionColor;
    
    [MenuItem("Window/Glow Editor")]
    public static void ShowWindow()
    {
        GetWindow<GlowEditorWindow>("Glow Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Glow Settings", EditorStyles.boldLabel);

        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), true);

        if (prefab != null)
        {
            glowingObject = prefab.GetComponent<GlowingObject>();

            if (glowingObject == null)
            {
                glowingObject = prefab.AddComponent<GlowingObject>();
            }

            objectRenderer = prefab.GetComponent<Renderer>();

            if (objectRenderer != null)
            {
                propertyBlock = new MaterialPropertyBlock();
                objectRenderer.GetPropertyBlock(propertyBlock);
                emissionColor = objectRenderer.sharedMaterial.GetColor("_EmissionColor");
                EditorGUILayout.ColorField("Emission Color", emissionColor);

                glowingObject.emissionIntensity = EditorGUILayout.FloatField("Emission Intensity", glowingObject.emissionIntensity);
                glowingObject.useAsLightSource = EditorGUILayout.Toggle("Use as Light Source", glowingObject.useAsLightSource);

                if (glowingObject.useAsLightSource)
                {
                    glowingObject.lightRange = EditorGUILayout.FloatField("Light Range", glowingObject.lightRange);
                    glowingObject.lightIntensity = EditorGUILayout.FloatField("Light Intensity", glowingObject.lightIntensity);
                }

                if (GUILayout.Button("Update Glow"))
                {
                    glowingObject.UpdateGlow();
                }

                if (GUILayout.Button("Remove Glow"))
                {
                    propertyBlock.SetColor("_EmissionColor", Color.black);
                    objectRenderer.SetPropertyBlock(propertyBlock);
                    DestroyImmediate(glowingObject);
                }
            }
            else
            {
                EditorGUILayout.HelpBox("The selected prefab does not have a Renderer component.", MessageType.Warning);
            }
        }
    }
}
