using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlaformMoveController : MonoBehaviour
{
    public bool isInMenu;

    public GameSettings.GameState gameState;
    GameObject go;
    GameSettings gameSettings;

    [Header("Self Handeling:")]
    [Tooltip("Shows if the platform is returning from the final possition")]
    public bool returns = false;
    public bool Wait;
    public bool Waited;
    [Tooltip("Indicates current possision to use")]
    public int i = 0;
    [Tooltip("Shows the time left on PauseTime")]
    public float RestTime;
    public bool[] Active;

    [Header("Settings:")]
    public bool UseButtons;
    public ButtonController_2[] Buttons;
    [Tooltip("Tells the platform to move")]
    public bool PlatformMoves;
    [Tooltip("Amount of time to wait before moving to the next possision Default = 5")]
    public float PauseTime = 5;
    [Tooltip("Platform movement speed 1-9")]
    [Range(1, 9)]
    public int PickSpeed;
    [Range(0, 0)]
    public float MoveSpeed = 0.0625f;
    [Tooltip("Element 0 is default")]
    public Vector3[] Possitions; // X Y Z

    #region Speeds

    private float speed1 = 0.0625f;
    private float speed2 = 0.125f;
    private float speed3 = 0.5f;
    private float speed4 = 1f;
    private float speed5 = 2f;
    private float speed6 = 4f;
    private float speed7 = 5f;
    private float speed8 = 10f;
    private float speed9 = 20f;

    #endregion

    private bool TimerOn;
    private int ii = 0;
    private Rigidbody rigidbody;

    private void Movement()
    {
        //Move up (+Y)
        if (transform.position.y < Possitions[i].y)
        {
            transform.Translate(Vector3.up * MoveSpeed, Space.World);
        }

        //Move down (-Y)
        if (transform.position.y > Possitions[i].y)
        {
            transform.Translate(Vector3.down * MoveSpeed, Space.World);
        }

        //Move right (+X)
        if (transform.position.x < Possitions[i].x)
        {
            transform.Translate(Vector3.right * MoveSpeed, Space.World);
        }

        //Move left (-X)
        if (transform.position.x > Possitions[i].x)
        {
            transform.Translate(Vector3.left * MoveSpeed, Space.World);
        }

        //Move forward (+Z)
        if (transform.position.z < Possitions[i].z)
        {
            transform.Translate(Vector3.forward * MoveSpeed, Space.World);
        }

        //Move back (-Z)
        if (transform.position.z > Possitions[i].z)
        {
            transform.Translate(Vector3.back * MoveSpeed, Space.World);
        }
    }

    void Check()
    {
        if (transform.position == Possitions[Possitions.Length - 1])
        {
            returns = true;
            Wait = true;
        }
        else if (transform.position == Possitions[0])
        {
            returns = false;
            Wait = true;
        }

        if (transform.position == Possitions[i])
        {
            switch (returns)
            {
                case true:
                    {
                        i--;
                        break;
                    }
                case false:
                    {
                        i++;
                        break;
                    }
            }
        }
    }

    void Timer(float time)
    {
        if (!TimerOn)
        {
            RestTime = time;
            TimerOn = true;
        }

        RestTime = RestTime - Time.deltaTime;

        if (RestTime <= 0)
        {
            Wait = false;
            TimerOn = false;
            Waited = true;
        }
    }

    void Start()
    {
        if (!isInMenu)
        {
            go = GameObject.Find("Settings");
            gameSettings = go.GetComponent<GameSettings>();
        }
        else
        {
            gameState = GameSettings.GameState.Unpaused;
        }

        rigidbody = gameObject.GetComponent<Rigidbody>();

        switch (PickSpeed)
        {
            case 1:
                {
                    MoveSpeed = speed1;
                    break;
                }
            case 2:
                {
                    MoveSpeed = speed2;
                    break;
                }
            case 3:
                {
                    MoveSpeed = speed3;
                    break;
                }
            case 4:
                {
                    MoveSpeed = speed4;
                    break;
                }
            case 5:
                {
                    MoveSpeed = speed5;
                    break;
                }
            case 6:
                {
                    MoveSpeed = speed6;
                    break;
                }
            case 7:
                {
                    MoveSpeed = speed7;
                    break;
                }
            case 8:
                {
                    MoveSpeed = speed8;
                    break;
                }
            case 9:
                {
                    MoveSpeed = speed9;
                    break;
                }
        }

        Active = new bool[Buttons.Length];
    }

    void TheDoDo()
    {
        switch (Wait)
        {
            case false:
                {
                    switch (PlatformMoves)
                    {
                        case true:
                            {
                                Movement();
                                Check();
                                Waited = false;
                            }
                            break;
                    }
                    break;
                }
            case true:
                {
                    Timer(PauseTime);
                    break;
                }
        }
    }

    bool ButtonUsage()
    {
        if (ii >= Buttons.Length - 1)
        {
            ii = 0;

        }

        Active[ii] = Buttons[ii].isActive;

        ii++;

        if (Active.All(x => x))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Update()
    {
        if (!isInMenu)
        {
            gameState = gameSettings.CurrentGameState;
        }

        switch (gameState)
        {
            case GameSettings.GameState.Unpaused:
                {
                    switch (UseButtons)
                    {
                        case true:
                            {
                                switch (ButtonUsage())
                                {
                                    case true:
                                        {
                                            TheDoDo();
                                            break;
                                        }
                                }
                                break;
                            }
                        case false:
                            {
                                TheDoDo();
                                break;
                            }
                    }
                    break;
                }

        }
    }

    bool PlatfomCloseEnough(Vector3 CurrentPos, Vector3 PosToGetTo)
    {
        float CurrentX = CurrentPos.x;
        float CurrentY = CurrentPos.y;
        float CurrentZ = CurrentPos.z;

        float GetToX = PosToGetTo.x;
        float GetToY = PosToGetTo.y;
        float GetToZ = PosToGetTo.z;

        bool GoodX = false, GoodY = false, GoodZ = false;

        if (TestRange(CurrentX, GetToX - 0.5f, GetToX + 0.5f))
        {
            GoodX = true;
        }
        if (TestRange(CurrentY, GetToY - 0.5f, GetToY + 0.5f))
        {
            GoodY = true;
        }
        if (TestRange(CurrentZ, GetToZ - 0.5f, GetToZ + 0.5f))
        {
            GoodZ = true;
        }

        if (GoodX == true || GoodY == true || GoodZ == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool TestRange(float numberToCheck, float bottom, float top)
    {
        return (numberToCheck >= bottom && numberToCheck <= top);
    }
}
