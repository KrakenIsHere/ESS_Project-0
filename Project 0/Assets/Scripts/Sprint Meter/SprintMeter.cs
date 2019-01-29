using UnityEngine;
using Vexe.Runtime.Types;

[RequireComponent(typeof(Transform))]
public class SprintMeter : BaseBehaviour
{
    GenericBehaviour genericBehaviour;
    MoveBehaviour moveBehaviour;
    private GameSettings.GameState currentGameState;

    GameObject go;
    GameObject Character;
    GameSettings gameSettings;
    
    public Transform staminaMeter;

    public static float currentStam = 200f;
    public static float maxStam = 200f;
    private float stamUseage = 5f;
    private float stamRegen = .25f;

    public float timeCheck = 0f;
    
    public KeyCode forwardKey;
    public KeyCode altForwardKey;
    
    void Awake()
    {
        go = GameObject.Find("Settings");
        Character = GameObject.Find("Project_0_Character");
        gameSettings = go.GetComponent<GameSettings>();
        moveBehaviour = Character.GetComponent<MoveBehaviour>();

        currentGameState = gameSettings.CurrentGameState;
    }

    void Update()
    {
        UpdateGameSettings();
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKey(forwardKey) || Input.GetKey(altForwardKey))
            {
                timeCheck += Time.deltaTime;
            }
        }
        else
        {
            timeCheck = 0;
        }

        if (Input.GetKeyUp(forwardKey) || Input.GetKeyUp(altForwardKey))
        {
            timeCheck = 0;
        }

        if (timeCheck <= 0)
        {
            if (currentStam < maxStam)
            {
                switch (currentGameState)
                {
                    case GameSettings.GameState.Paused:

                        break;

                    case GameSettings.GameState.Unpaused:
                        currentStam += stamRegen;
                        break;
                }
            }
        }
        

        if (timeCheck > .5f)
        {
            timeCheck = 0f;

            if (currentStam <= 0f)
            {
                currentStam = 0f;
            }
            else
            {
                currentStam -= stamUseage;
            }
        }

        if (currentStam <= 0)
        {
            moveBehaviour.sprintSpeed = 0.5f;
        }
        else
        {
            moveBehaviour.sprintSpeed = 1f;
        }

        staminaMeter.GetComponent<RectTransform>().localScale = new Vector3(currentStam / maxStam, 1, 1);
    }

    private void UpdateGameSettings()
    {
        currentGameState = gameSettings.CurrentGameState;
    }
}
