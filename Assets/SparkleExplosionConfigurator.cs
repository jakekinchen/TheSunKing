// using UnityEngine;

// public class SparkleExplosionConfigurator : MonoBehaviour
// {
//     public ParticleSystem parentParticleSystem;
//     private ParticleSystem _particleSystem;
//     public Texture2D sparkleTexture;

//     void Awake()
//     {
//         _particleSystem = GetComponent<ParticleSystem>();
//         ConfigureParticleSystem();
//     }

//     private void ConfigureParticleSystem()
//     {
//         // Main settings
//         var parentMain = parentParticleSystem.main;
//         var main = _particleSystem.main;
//         main.duration = 1f;
//         main.startLifetime = new ParticleSystem.MinMaxCurve(0.5f, 1f);
//         main.startSpeed = new ParticleSystem.MinMaxCurve(5f, 10f);
//         main.startSize = new ParticleSystem.MinMaxCurve(0.1f, 0.3f);
//         main.startColor = parentMain.startColor;
//         main.gravityModifier = parentMain.gravityModifier;
//         main.playOnAwake = false;
//         main.loop = false;
//         main.simulationSpace = ParticleSystemSimulationSpace.Local;

//         // Emission settings
//         var emission = _particleSystem.emission;
//         emission.rateOverTime = 1f;
//         emission.SetBurst(0, new ParticleSystem.Burst(0f, 50));

//         // Shape settings
//         var shape = _particleSystem.shape;
//         shape.shapeType = ParticleSystemShapeType.Sphere;
//         shape.radius = parentMain.startSize.constantMax * 0.1f;
//         shape.radiusThickness = 0f; // Emit from shell

//         // Renderer settings
//         var renderer = _particleSystem.GetComponent<ParticleSystemRenderer>();
//         Material material = new Material(Shader.Find("Particles/Standard Unlit"));
//         material.SetColor("_Emission", parentMain.startColor.color);
//         // Make sure to set the texture to a sparkle shape
//         material.SetTexture("_MainTex", sparkleTexture);
//         renderer.material = material;
//     }
// }
