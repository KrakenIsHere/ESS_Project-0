using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SqureOnlyButtonController : MonoBehaviour
{

    private AudioSource buttonAudio;
    private Animation anime;

    #region ButtonControllers

    [Header("Buttons Unlock:")]
    public int i = 0;
    [Tooltip("Sets the amount of Buttons to unlock if any")]
    public ButtonController_2[] nextButtonController = new ButtonController_2[0];

    #endregion

    #region Ints N Bools


    [Header("Booleans:")]
    [Tooltip("Is used to show the button's current state")]

    public bool isActive = false;
public string isActiveString;


    [Header("Integers:")]
    [Tooltip("The weight required to activate")]
    [Range(100.0f, 100.0f)]
    public int WeightToActivate = 100;

    [Tooltip("Sets the weight transfered by an object with tag (Moveables)")]
    [Range(100.0f, 100.0f)]
    public int moveablesWeight = 100;

    [Tooltip("Weight Currently on button")]
    [Range(0.0f, 0.0f)]
    public int currentWeight = 0;

    #endregion

    #region WeightControll

    private void AddWeight(Collider MoveableItem)
    {
        if (MoveableItem.tag == "Square Moveables")
        {
            currentWeight += moveablesWeight;
        }
    }

    private void SubtractWeight(Collider MoveableItem)
    {
        if (MoveableItem.tag == "Square Moveables")
        {
            currentWeight -= moveablesWeight;
        }
    }

    #endregion

    #region ActivationControll

    private void ActivateButton()
    {
        if (currentWeight >= WeightToActivate && isActive == false)
        {
            switch (nextButtonController.Length)
            {
                case 0:
                    {

                        break;
                    }
                default:
                    {
                        while (i <= nextButtonController.Length)
                        {
                            nextButtonController[i].isLocked = false;
                            i++;

                            if (i == nextButtonController.Length)
                            {
                                i = 0;
                                break;
                            }
                        }
                        break;
                    }
            }

            anime.Play("ButtonPad");
            buttonAudio.Play();
            isActive = true;
        }
    }


    private void DeActivateButton()
    {
        if (currentWeight < WeightToActivate && isActive == true)
        {
            switch (nextButtonController.Length)
            {
                case 0:
                    {

                        break;
                    }
                default:
                    {
                        while (i <= nextButtonController.Length)
                        {
                            nextButtonController[i].isLocked = false;
                            i++;

                            if (i == nextButtonController.Length)
                            {
                                i = 0;
                                break;
                            }
                        }
                        break;
                    }
            }

            anime.Play("ButtonPadReverse");
            buttonAudio.Play();
            isActive = false;
        }
    }

    #endregion

    #region Start N Update
    private void Start()
    {
        isActive = false;

        buttonAudio = gameObject.GetComponent<AudioSource>();
        anime = GetComponentInParent<Animation>();
    }

    private void Update()
    {
        if (currentWeight < 0)
        {
            currentWeight = 0;
        }

        ActivateButton();
        DeActivateButton();
    }

    #endregion

    #region OnTrigger

    private void OnTriggerEnter(Collider other)
    {
        AddWeight(other);
    }

    private void OnTriggerExit(Collider other)
    {
        SubtractWeight(other);
    }

    #endregion
}

