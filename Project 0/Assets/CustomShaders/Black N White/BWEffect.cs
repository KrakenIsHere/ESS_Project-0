using UnityEngine;
using System.Collections;
using System;
 
[ExecuteInEditMode]
public class BWEffect : MonoBehaviour
{
    System.Random rand;

    public bool randIntensityActive;

    public float intensity;
    private Material material;

    // Creates a private material used to the effect
    void Awake()
    {
        rand = new System.Random();
        material = new Material(Shader.Find("Hidden/BWDiffuse"));
    }

    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (intensity == 0)
        {
            Graphics.Blit(source, destination);
            return;
        }

        material.SetFloat("_bwBlend", intensity);
        Graphics.Blit(source, destination, material);
    }

    private void Update()
    {
        if (randIntensityActive)
        {
            intensity = (float)rand.Next(-20, 20);
        }
    }
}