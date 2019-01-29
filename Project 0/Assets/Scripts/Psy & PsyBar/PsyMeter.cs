using UnityEngine;
using Vexe.Runtime.Types;

[RequireComponent(typeof(Transform))]
public class PsyMeter : BaseBehaviour
{
    private GameSettings.GameState currentGameState;
    public GameSettings.CurrentlyGrabbed currentlyGrabbed;

    GameObject go;
    GameObject Cam;
    GameSettings gameSettings;

    MoveLightMoveables moveLightMoveables;
    MoveNormalMoveables moveNormalMoveables;
    MoveHeavyMoveables moveHeavyMoveables;
    MoveSphereMoveables moveSphereMoveables;
    MoveSquareMoveables moveSquareMoveables;

    public Transform psyMeter;

    public static float currentPsy = 100f;
    public static float maxPsy = 100f;
    public float psyUseage = 2.5f;
    public float psyRegen = .25f;

    public float timeCheck = 0f;

    public KeyCode activationKey;
    public KeyCode altActivationKey;

    void Awake()
    {
        go = GameObject.Find("Settings");
        Cam = GameObject.Find("Main Camera");
        gameSettings = go.GetComponent<GameSettings>();

        moveLightMoveables = Cam.GetComponent<MoveLightMoveables>();
        moveNormalMoveables = Cam.GetComponent<MoveNormalMoveables>();
        moveHeavyMoveables = Cam.GetComponent<MoveHeavyMoveables>();
        moveSphereMoveables = Cam.GetComponent<MoveSphereMoveables>();
        moveSquareMoveables = Cam.GetComponent<MoveSquareMoveables>();

        currentGameState = gameSettings.CurrentGameState;
        currentlyGrabbed = gameSettings.currentlyGrabbed;
    }
    
    void Update()
    {
        UpdateGameSettings();

        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (Input.GetKey(activationKey) || Input.GetKey(altActivationKey))
            {
                switch (currentlyGrabbed)
                {
                    case GameSettings.CurrentlyGrabbed.NoneGrabbed:
                        timeCheck = 0f;
                        break;

                    case GameSettings.CurrentlyGrabbed.LightMoveablesGrabbed:
                        timeCheck += Time.deltaTime;
                        break;

                    case GameSettings.CurrentlyGrabbed.NormalMoveablesGrabbed:
                        timeCheck += Time.deltaTime;
                        break;

                    case GameSettings.CurrentlyGrabbed.HeavyMoveablesGrabbed:
                        timeCheck += Time.deltaTime;
                        break;

                    case GameSettings.CurrentlyGrabbed.SphereMoveablesGrabbed:
                        timeCheck += Time.deltaTime;
                        break;

                    case GameSettings.CurrentlyGrabbed.SquareMoveablesGrabbed:
                        timeCheck += Time.deltaTime;
                        break;
                }
            }
        }

        if (Input.GetKeyUp(activationKey) || Input.GetKeyUp(altActivationKey))
        {
            timeCheck = 0;
        }

        if (timeCheck <= 0)
        {
            if (currentPsy < maxPsy)
            {
                switch (currentGameState)
                {
                    case GameSettings.GameState.Paused:
                        
                        break;
                        
                    case GameSettings.GameState.Unpaused:
                        currentPsy += psyRegen;
                        break;
                }
            }
        }


        if (timeCheck > .5f)
        {
            timeCheck = 0f;

            if (currentPsy <= 0f)
            {
                currentPsy = 0f;
            }
            else
            {
                switch (currentlyGrabbed)
                {
                    case GameSettings.CurrentlyGrabbed.NoneGrabbed:
                        if (currentPsy < maxPsy)
                        {
                            switch (currentGameState)
                            {
                                case GameSettings.GameState.Paused:

                                    break;

                                case GameSettings.GameState.Unpaused:
                                    currentPsy += psyRegen;
                                    break;
                            }
                        }
                        break;

                    case GameSettings.CurrentlyGrabbed.LightMoveablesGrabbed:
                        currentPsy -= psyUseage;
                        break;

                    case GameSettings.CurrentlyGrabbed.NormalMoveablesGrabbed:
                        currentPsy -= psyUseage;
                        break;

                    case GameSettings.CurrentlyGrabbed.HeavyMoveablesGrabbed:
                        currentPsy -= psyUseage;
                        break;

                    case GameSettings.CurrentlyGrabbed.SphereMoveablesGrabbed:
                        currentPsy -= psyUseage;
                        break;

                    case GameSettings.CurrentlyGrabbed.SquareMoveablesGrabbed:
                        currentPsy -= psyUseage;
                        break;
                }
            }
        }

        if (currentPsy <= 0)
        {
            DisablePsy();
        }
        else
        {
            EnablePsy();
        }
        
        //psyMeter.GetComponent<RectTransform>().localScale = new Vector3(currentPsy / maxPsy, 1, 1);
    }

    private void DisablePsy()
    {
        moveLightMoveables.canGrab = false;
        moveLightMoveables.canPush = false;
        moveLightMoveables.canReposition = false;
        moveLightMoveables.canRotate = false;
        moveLightMoveables.canThrow = false;

        moveNormalMoveables.canGrab = false;
        moveNormalMoveables.canPush = false;
        moveNormalMoveables.canReposition = false;
        moveNormalMoveables.canRotate = false;
        moveNormalMoveables.canThrow = false;

        moveHeavyMoveables.canGrab = false;
        moveHeavyMoveables.canPush = false;
        moveHeavyMoveables.canReposition = false;
        moveHeavyMoveables.canRotate = false;
        moveHeavyMoveables.canThrow = false;

        moveSphereMoveables.canGrab = false;
        moveSphereMoveables.canPush = false;
        moveSphereMoveables.canReposition = false;
        moveSphereMoveables.canRotate = false;
        moveSphereMoveables.canThrow = false;

        moveSquareMoveables.canGrab = false;
        moveSquareMoveables.canPush = false;
        moveSquareMoveables.canReposition = false;
        moveSquareMoveables.canRotate = false;
        moveSquareMoveables.canThrow = false;
    }

    private void EnablePsy()
    {
        moveLightMoveables.canGrab = true;
        moveLightMoveables.canPush = true;
        moveLightMoveables.canReposition = true;
        moveLightMoveables.canRotate = true;
        moveLightMoveables.canThrow = true;

        moveNormalMoveables.canGrab = true;
        moveNormalMoveables.canPush = true;
        moveNormalMoveables.canReposition = true;
        moveNormalMoveables.canRotate = true;
        moveNormalMoveables.canThrow = true;

        moveHeavyMoveables.canGrab = true;
        moveHeavyMoveables.canPush = true;
        moveHeavyMoveables.canReposition = true;
        moveHeavyMoveables.canRotate = true;
        moveHeavyMoveables.canThrow = true;

        moveSphereMoveables.canGrab = true;
        moveSphereMoveables.canPush = true;
        moveSphereMoveables.canReposition = true;
        moveSphereMoveables.canRotate = true;
        moveSphereMoveables.canThrow = true;

        moveSquareMoveables.canGrab = true;
        moveSquareMoveables.canPush = true;
        moveSquareMoveables.canReposition = true;
        moveSquareMoveables.canRotate = true;
        moveSquareMoveables.canThrow = true;
    }

    private void UpdateGameSettings()
    {
        currentGameState = gameSettings.CurrentGameState;
        currentlyGrabbed = gameSettings.currentlyGrabbed;
    }
}
