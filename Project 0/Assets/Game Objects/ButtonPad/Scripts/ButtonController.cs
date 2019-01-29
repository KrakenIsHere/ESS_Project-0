using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private AudioSource buttonAudio;
    private Animation anime;

    [Header("Locked Buttons")]
    [Tooltip("Sets the amount of Buttons to unlock if any")]
    [Range(0.0f, 4.0f)]
    public int amountOfButtons;
    public ButtonController nextButtonController1;
    public ButtonController nextButtonController2;
    public ButtonController nextButtonController3;
    public ButtonController nextButtonController4;



    [Header("Allowed to change")]
    [Tooltip("Set true if you want to use another button to unlock this one")]
    public bool isLocked;
    [Tooltip("Amount of tagged objects (Spheres) needed to activate button\n Standard: 5")]
    [Range(1.0f, 5.0f)]
    public int moveableSpheresNeeded = 5;
    [Tooltip("Amount of tagged objects (Cubes) needed to activate button\n Standard: 2")]
    [Range(1.0f, 5.0f)]
    public int moveableStandardCubesNeeded = 2;

    [Header("Booleans (Do NOT change)")]
    [Tooltip("Is used to show the button's current state")]
    //Dertermins if button is Active
    public bool isActive = false;

    [Tooltip("Determins if button is activated by the player")]
    //Determins if button is activated by the player
    public bool playerIsOn = false;

    [Tooltip("Determins if button is activated by a heavy cube")]
    //Determins if button is activated by a heavy cube
    public bool MovableHeavyCubeIsOn = false;

    [Tooltip("Determins if button is activated by a light cube")]
    //Determins if button is activated by a light cube
    public bool MovableLightCubeIsOn = false;

    [Tooltip("Determins if button is activated by a cube")]
    //Determins if button is activated by a cube
    public bool MovableStandardCubeIsOn = false;

    [Tooltip("Determins if button is activated by multiple sphears (Needs: 5)")]
    //Determins if button is activated by multiple sphears (Needs: 5)
    public bool MovableSphearsOn = false;

    [Header("Integers (Do NOT Change)")]
    [Tooltip("Sees the amount of standard cubes on button")]
    [Range(0.0f, 0.0f)]
    //Used to track amount of Standard cube inside collider
    public int MovableStandardCubeInt = 0;

    [Tooltip("Sees the amount of spheres on button")]
    [Range(0.0f, 0.0f)]
    //Used to track amount of sphears inside collider
    public int MovableSphearsInt = 0;

    //Gets the animation component from the gameobject the script is connected to;
    private void Start()
    {

        isActive = false;
        playerIsOn = false;
        MovableHeavyCubeIsOn = false;
        MovableLightCubeIsOn = false;
        MovableStandardCubeIsOn = false;
        MovableSphearsOn = false;

        buttonAudio = gameObject.GetComponent<AudioSource>();
        anime = GetComponentInParent<Animation>();
    }

    private void Update()
    {
        if (isLocked == true && isActive == true)
        {
            anime.Play("ButtonPadReverse");
            buttonAudio.Play();
            isActive = false;
        }
    }

    //Activates button if right conditions are met
    private void OnTriggerEnter(Collider other)
    {
        //Controlls Movable Conditions
        switch (other.tag)
        {
            case "Player":
                {
                    playerIsOn = true;
                    break;
                }
            case "Sphere Moveables":
                {
                    MovableSphearsInt++;

                    if (MovableSphearsInt >= moveableSpheresNeeded)
                    {
                        MovableSphearsOn = true;
                    }
                    break;
                }
            case "Moveables":
                {
                    MovableSphearsInt++;

                    if (MovableSphearsInt >= moveableSpheresNeeded)
                    {
                        MovableSphearsOn = true;
                    }
                    break;
                }
            case "Heavy Moveables":
                {
                    MovableHeavyCubeIsOn = true;
                    break;
                }
            case "Light Moveables":
                {
                    MovableLightCubeIsOn = true;
                    break;
                }
            case "Square Moveables":
                {
                    MovableStandardCubeInt++;

                    if (MovableStandardCubeInt >= moveableStandardCubesNeeded)
                    {
                        MovableStandardCubeIsOn = true;
                    }
                    break;
                }
        }
        ActivateButton();
    }

    //Activation Block:
    private void ActivateButton()
    {
        if (isLocked == false)
        {
            if (MovableStandardCubeIsOn == true && isActive == false || MovableSphearsOn == true && isActive == false || playerIsOn == true && isActive == false || MovableLightCubeIsOn == true && isActive == false || MovableHeavyCubeIsOn == true && isActive == false)
            {
                switch (amountOfButtons)
                {
                    case 1:
                        {
                            nextButtonController1.isLocked = false;
                            break;
                        }
                    case 2:
                        {
                            nextButtonController1.isLocked = false;
                            nextButtonController2.isLocked = false;
                            break;
                        }
                    case 3:
                        {
                            nextButtonController1.isLocked = false;
                            nextButtonController2.isLocked = false;
                            nextButtonController3.isLocked = false;
                            break;
                        }
                    case 4:
                        {
                            nextButtonController1.isLocked = false;
                            nextButtonController2.isLocked = false;
                            nextButtonController3.isLocked = false;
                            nextButtonController4.isLocked = false;
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
                
                anime.Play("ButtonPad");
                buttonAudio.Play();
                isActive = true;
            }
        }
    }

    //De-activates button if right conditions are met
    private void DeActivateButton()
    {
        //De-activation Block
        if (MovableStandardCubeIsOn == false && isActive == true || MovableSphearsOn == false && isActive == true || playerIsOn == false && isActive == true || MovableLightCubeIsOn == false && isActive == true || MovableHeavyCubeIsOn == false && isActive == true)
        {
            switch (amountOfButtons)
            {
                case 1:
                    {
                        nextButtonController1.isLocked = true;
                        break;
                    }
                case 2:
                    {
                        nextButtonController1.isLocked = true;
                        nextButtonController2.isLocked = true;
                        break;
                    }
                case 3:
                    {
                        nextButtonController1.isLocked = true;
                        nextButtonController2.isLocked = true;
                        nextButtonController3.isLocked = true;
                        break;
                    }
                case 4:
                    {
                        nextButtonController1.isLocked = true;
                        nextButtonController2.isLocked = true;
                        nextButtonController3.isLocked = true;
                        nextButtonController4.isLocked = true;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }


            anime.Play("ButtonPadReverse");
            buttonAudio.Play();
            isActive = false;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        //Controlls Movable Conditions
        switch (other.tag)
        {
            case "Player":
                {
                    playerIsOn = false;
                    break;
                }
            case "Sphere Moveables":
                {
                    MovableSphearsInt--;

                    if (MovableSphearsInt < 5)
                    {
                        MovableSphearsOn = false;
                    }
                    break;
                }
            case "Moveables":
                {
                    MovableSphearsInt--;

                    if (MovableSphearsInt < 2)
                    {
                        MovableSphearsOn = false;
                    }
                    break;
                }
            case "Heavy Square Moveables":
                {
                    MovableHeavyCubeIsOn = true;
                    break;
                }
            case "Light Square Moveables":
                {
                    MovableLightCubeIsOn = true;
                    break;
                }
            case "Square Moveables":
                {
                    MovableStandardCubeInt--;

                    if (MovableStandardCubeInt < 2)
                    {
                        MovableStandardCubeIsOn = false;
                    }
                    break;
                }
        }

        DeActivateButton();
    }
}

