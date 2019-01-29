using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelFadeController : MonoBehaviour
{
    [Tooltip("Texture of fade")]
    public Texture2D fadeTexture;
    [Tooltip("Sets the speed ot the fading")]
    public float speedOfFade = 0.6f;

    private int drawDepth = -999;
    private float alpha = 1.0f;
    private int fadeDir = -1;

    private void OnGUI()
    {
        alpha += fadeDir * speedOfFade * Time.deltaTime;

        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
    }

    public float FadeBegin(int direct)
    {
        fadeDir = direct;
        return (speedOfFade);
    }

    private void OnLevelWasLoaded(int level)
    {
        FadeBegin(-1);
    }
}
