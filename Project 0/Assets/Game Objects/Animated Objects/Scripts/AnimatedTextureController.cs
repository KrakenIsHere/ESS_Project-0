using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedTextureController : MonoBehaviour
{
    #region Other

    GameObject go;
    GameSettings gameSettings;

    [Header("Animation Settings:")]
    [Range (0, 60)]
    [Tooltip("1 = 1 Second")]
    public float TimerDuration;
    private float Duration;

    [Header("Dont touch")]
    public GameSettings.GameState gameState;
    public int i = 0;
    public float CurrentDuration;
    
    [Header("Materials:")]
    [Tooltip("Make Element 0 The standard Material")]
    public Material[] AnimateMaterialArray = new Material[0];

    private void Start()
    {
        go = GameObject.Find("Settings");
        gameSettings = go.GetComponent<GameSettings>();

        Duration = TimerDuration;
    }

    #endregion

    void ChangeMaterial()
    {
        #region Timer

        Duration -= Time.deltaTime;

        if (Duration < 0)
        {
            Duration = TimerDuration;
            i++;
            if (i == AnimateMaterialArray.Length)
            {
                i = 0;
            }

            this.GetComponent<Renderer>().material = AnimateMaterialArray[i];
        }

#endregion
    }       

    void Update ()
    {
        CurrentDuration = Duration;

        gameState = gameSettings.CurrentGameState;

        switch (gameState)
        {
            case GameSettings.GameState.Paused:
                {
                    break;
                }
            case GameSettings.GameState.Unpaused:
                {
                    ChangeMaterial();
                    break;
                }
        }
	}
}
