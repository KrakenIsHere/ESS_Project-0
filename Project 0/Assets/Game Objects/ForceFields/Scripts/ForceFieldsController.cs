using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class ForceFieldsController : MonoBehaviour
{
    [Header("Don't touch:")]
    public string enterOtherTag;
    public string exitOtherTag;
    public bool playerPassed;
    public bool playerButtonOverride;

    [Header("Other:")]
    [Tooltip("Makes the barrier pasable by Moveables")]
    public bool makeGreen = false;
    [Tooltip("Tells if the FFC is the last one")]
    public bool isEnd = false;

    [Tooltip("Indicates which dice activates the force field")]
    public Dice_Controller[] dices;

    [Tooltip("Indicates which button does the activation of the force field")]
    public ButtonController_2[] realButton;

    [Header("Force Field Objects")]
    public GameObject redForceField;
    [Tooltip("if isEnd is active this should be set to the first green field")]
    public GameObject greenForceField;

    [Header("Force Field Materials")]
    public Material redForceFieldMaterial;
    public Material greenForceFieldMaterial;
    public Material yellowForceFieldMaterial;

    [Header("Force Field Colliders")]
    public BoxCollider redForceFieldCollider;
    [Tooltip("if isEnd is active this should be set to the first green field")]
    public BoxCollider greenForceFieldCollider;
    public int i = 0;
    bool[] buttonIsActive;
    bool[] dicesAreActive;
    int[] index;

    private void Start()
    {
        switch (gameObject.tag)
        {
            case "RedField":
                {
                    gameObject.GetComponent<Renderer>().material = redForceFieldMaterial;
                    
                    break;
                }
            case "GreenField":
                {
                    gameObject.GetComponent<Renderer>().material = greenForceFieldMaterial;
                    gameObject.layer = 17;
                    break;
                }
        }

        buttonIsActive = new bool[realButton.Length];
        dicesAreActive = new bool[dices.Length];
    }

    private void Update()
    {
        switch (playerButtonOverride)
        {
            case true:
                {
                    ChangeMeterialOfForceFieldToYellowOrGreen();
                    break;
                }
            default:
                {
                    if (realButton.Length == 0 && dices.Length == 0)
                    {
                        ChangeMeterialOfForceFieldToRed();
                    }
                    else
                    {
                        switch (realButton.Length)
                        {
                            case 0:
                                {
                                    break;
                                }
                            default:
                                {
                                    buttonIsActive[i] = realButton[i].isActive;
                                    i++;

                                    if (i >= realButton.Length)
                                    {
                                        i = 0;
                                    }

                                    if (buttonIsActive.All(x => x))
                                    {
                                        ChangeMeterialOfForceFieldToYellowOrGreen();
                                    }
                                    else
                                    {
                                        ChangeMeterialOfForceFieldToRed();
                                    }
                                    break;
                                }

                        }

                        switch (dices.Length)
                        {
                            case 0:
                                {
                                    break;
                                }
                            default:
                                {
                                    dicesAreActive[i] = dices[i].isRight;

                                    i++;

                                    if (i >= dices.Length)
                                    {
                                        i = 0;
                                    }

                                    if (dicesAreActive.All(x => x))
                                    {
                                        ChangeMeterialOfForceFieldToYellowOrGreen();
                                    }
                                    else
                                    {
                                        ChangeMeterialOfForceFieldToRed();
                                    }
                                    break;
                                }

                        }
                    }
                    break;
                }
        }
    }

    private void ChangeMeterialOfForceFieldToRed()
    {
        if (tag == "RedField")
        {
            redForceFieldCollider.isTrigger = false;
            
            redForceField.GetComponent<Renderer>().material = redForceFieldMaterial;
        }
    }

    private void ChangeMeterialOfForceFieldToYellowOrGreen()
    {
        if (tag == "RedField" && makeGreen == true)
        {
            redForceFieldCollider.isTrigger = true;
            gameObject.layer = 17;
            redForceField.GetComponent<Renderer>().material = greenForceFieldMaterial;
        }
        else if (tag == "RedField")
        {
            redForceFieldCollider.isTrigger = false;
            gameObject.layer = 15;
            redForceField.GetComponent<Renderer>().material = yellowForceFieldMaterial;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        enterOtherTag = other.tag;

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {

        }
        else
        {
            if (tag == "RedField" && this.GetComponent<Renderer>().material == yellowForceFieldMaterial && other.tag != "Player")
            {
                redForceFieldCollider.isTrigger = false;
            }
            else if (tag == "RedField" && other.tag == "Player" && isEnd == true)
            {
                greenForceField.GetComponent<Renderer>().material = greenForceFieldMaterial;
                greenForceFieldCollider.isTrigger = true;
                gameObject.layer = 17;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        exitOtherTag = other.tag;

        if (tag == "GreenField" && other.tag == "Player")
        {
            playerPassed = true;

            greenForceField.GetComponent<Renderer>().material = redForceFieldMaterial;
            greenForceFieldCollider.isTrigger = false;
        }
        
        //if (tag == "GreenField" && other.tag == "MainCamera" && playerPassed)
        //{
        //    greenForceField.GetComponent<Renderer>().material = redForceFieldMaterial;
        //    greenForceFieldCollider.isTrigger = false;
        //}
    }
}
