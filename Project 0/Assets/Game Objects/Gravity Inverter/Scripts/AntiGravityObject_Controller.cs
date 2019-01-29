using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiGravityObject_Controller : MonoBehaviour
{
    Rigidbody rigidbody;
    GameObject go;
    GameSettings gameSettings;
    Vector3 y_Holder = new Vector3(0.0f, 0.0f, 0.0f);

    [Header("Self handeling:")]
    public bool usingAntiGravity;
    public GameSettings.GameState gameState;
    public GameObject thisObject;

    [Header("Ajustable:")]
    public bool isInMenu;

    [Header("Gravity Controllers:")]
    [Tooltip(" Sets the power of upwords gravity when\n (Position Y is) under maxHigh\n Standard: 1")]
    [Range(0.1f, 100.0f)]
    public float AntiGravity = 1.0f;
    [Tooltip(" Sets the power of downwords gravity when\n (Position Y is) above maxHigh\n Standard: 0")]
    [Range(-100.0f, -0.0f)]
    public float NormalGravity = -1.0f;

    // Use this for initialization
    void Start ()
    {
        thisObject = gameObject;

        rigidbody = thisObject.GetComponent<Rigidbody>();

        if (!isInMenu)
        {
            go = GameObject.Find("Settings");
            gameSettings = go.GetComponent<GameSettings>();
        }
        else
        {
            gameState = GameSettings.GameState.Unpaused;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!isInMenu)
        {
            gameState = gameSettings.CurrentGameState;
        }
        switch (gameState)
        {
            case GameSettings.GameState.Unpaused:
                {
                    switch (usingAntiGravity)
                    {
                        case true:
                            {
                                y_Holder.y = AntiGravity;
                                rigidbody.AddForce(y_Holder);

                                ConsoleProDebug.Watch("y_Holder", y_Holder.ToString());

                                break;
                            }
                        case false:
                            {
                                y_Holder.y = NormalGravity;
                                rigidbody.AddForce(y_Holder);

                                ConsoleProDebug.Watch("y_Holder", y_Holder.ToString());

                                break;
                            }
                    }
                    break;
                }
            case GameSettings.GameState.Paused:
                {
                    break;
                }
        }
    }
}
