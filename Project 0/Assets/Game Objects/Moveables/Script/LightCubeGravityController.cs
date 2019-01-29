using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCubeGravityController : MonoBehaviour {

    public GameSettings.GameState gameState;
    GameObject go;
    GameSettings gameSettings;

    Vector3 y_Holder = new Vector3(0.0f, 0.0f, 0.0f);
    Rigidbody thisRigidbody;
    public GameObject thisObject;

    [Header("Settings")]
    public bool isInMenu;

    [Header("Gravity Controllers:")]
    [Tooltip(" Sets the power of upwords gravity when\n (Position Y is) under maxHigh\n Standard: 1")]
    [Range(0.1f, 100.0f)]
    public float AntiGravity = 1.0f;
    [Tooltip(" Sets the power of downwords gravity when\n (Position Y is) above maxHigh\n Standard: -1")]
    [Range(-100.0f, -0.1f)]
    public float NormalGravity = -1.0f;

    [Header("Maximum Y Position:")]
    [Tooltip(" maxHigh is the max Y level\n The Standard: 6.475f")]
    [Range(0.0f, 100.0f)]
    public float maxHigh = 6.475f;

    //Gets rigidbody of object
    private void Start()
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
        thisRigidbody = thisObject.GetComponent<Rigidbody>();
    }

    //adds force every frame to make raising effect consistant
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
                    if (thisObject.transform.position.y > maxHigh)
                    {
                        y_Holder.y = NormalGravity;
                        thisRigidbody.AddForce(y_Holder);
                    }
                    else
                    {
                        y_Holder.y = AntiGravity;
                        thisRigidbody.AddForce(y_Holder);
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
