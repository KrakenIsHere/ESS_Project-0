using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController_2 : MonoBehaviour
{
 public AudioSource buttonAudio;
  public Animation anime;
 public Vector3 standardScale = new Vector3();

    #region ButtonControllers

    [Header("Buttons Unlock:")]
    public int i = 0;
    [Tooltip("Sets the amount of Buttons to unlock if any")]
    public ButtonController_2[] nextButtonController = new ButtonController_2[0];

#endregion

    #region Ints N Bools

    [Header("Button Lock:")]
    [Tooltip("Set true if you want to use another button to unlock this one")]
    public bool isLocked;


    [Header("Booleans:")]
    [Tooltip("Is used to show the button's current state")]
    public bool isActive = false;
    public string isActiveString;

    [Header("Integers:")]
    [Tooltip("The weight required to activate")]
    [Range(1.0f, 10000.0f)]
    public int WeightToActivate = 100;

    [Tooltip("Sets the weight transfered by an object with tag (Player)")]
    [Range(50.0f, 200.0f)]
    public int PlayerWeight = 100;

    [Tooltip("Sets the weight transfered by an object with tag (Moveables)")]
    [Range(30.0f, 50.0f)]
    public int SquareMoveablesWeight = 50;

    [Tooltip("Sets the weight transfered by an object with tag (Light Moveables)")]
    [Range(1.0f, 200.0f)]
    public int LightMoveablesWeight = 10;

    [Tooltip("Sets the weight transfered by an object with tag (Heavy Moveables)")]
    [Range(60.0f, 100.0f)]
    public int HeavyMoveablesWeight = 100;

    [Tooltip("Sets the weight transfered by an object with tag (Heavy Moveables)")]
    [Range(1.0f, 100.0f)]
    public int SphereMoveablesWeight = 75;

    [Tooltip("Weight Currently on button")]
    [Range(0.0f, 0.0f)]
 public int currentWeight = 0;

#endregion

    #region ColliderControll

    [Header("Collider Controll:")]
    public Vector3 amountToExtraExtendCollider1;
    public Vector3 amountToExtraExtendCollider2;
    public Vector3 amountToExtraExtendCollider3;
    public Vector3 amountToExtraExtendCollider4;

    [Range(50.0f, 10000.0f)]
    public int WeightBeforeColliderExtend1;
    [Range(50.0f, 10000.0f)]
    public int WeightBeforeColliderExtend2;
    [Range(50.0f, 10000.0f)]
    public int WeightBeforeColliderExtend3;
    [Range(50.0f, 10000.0f)]
    public int WeightBeforeColliderExtend4;

    private bool extended1;
    private bool extended2;
    private bool extended3;
    private bool extended4;

    private void ExtendCollider()
    {
        if (currentWeight >= WeightBeforeColliderExtend4 && extended4 == false)
        {
            gameObject.transform.localScale += amountToExtraExtendCollider4;
            extended4 = true;
        }
        else if (currentWeight >= WeightBeforeColliderExtend3 && extended3 == false)
        {
            gameObject.transform.localScale += amountToExtraExtendCollider3;
            extended3 = true;
        }
        else if (currentWeight >= WeightBeforeColliderExtend2 && extended2 == false)
        {
            gameObject.transform.localScale += amountToExtraExtendCollider2;
            extended2 = true;
        }
        else if (currentWeight >= WeightBeforeColliderExtend1 && extended1 == false)
        {
            gameObject.transform.localScale += amountToExtraExtendCollider1;
            extended1 = true;
        }
    }

    private void RetractCollider()
    {
        if (currentWeight < WeightBeforeColliderExtend4 && extended4 == true)
        {
            gameObject.transform.localScale -= amountToExtraExtendCollider4;
            extended4 = false;
        }
        else if (currentWeight < WeightBeforeColliderExtend3 && extended3 == true)
        {
            gameObject.transform.localScale -= amountToExtraExtendCollider3;
            extended3 = false;
        }
        else if (currentWeight < WeightBeforeColliderExtend2 && extended2 == true)
        {
            gameObject.transform.localScale -= amountToExtraExtendCollider2;
            extended2 = false;
        }
        else if (currentWeight < WeightBeforeColliderExtend1 && extended1 == true)
        {
            gameObject.transform.localScale -= amountToExtraExtendCollider1;
            extended1 = false;
        }
    }

    #endregion

    #region WeightControll

    private void AddWeight(Collider MoveableItem)
    {
        switch (MoveableItem.tag)
            {
            case "Player":
                {
                    currentWeight += PlayerWeight;
                    break;
                }
            case "Square Moveables":
                {
                    currentWeight += SquareMoveablesWeight;
                    break;
                }
            case "Heavy Moveables":
                {
                    currentWeight += HeavyMoveablesWeight;
                    break;
                }
            case "Light Moveables":
                {
                    currentWeight += LightMoveablesWeight;
                    break;
                }
            case "Sphere Moveables":
                {
                    currentWeight += SphereMoveablesWeight;
                    break;
                }
        }
    }

    private void SubtractWeight(Collider MoveableItem)
    {
        switch (MoveableItem.tag)
        {
            case "Player":
                {
                    currentWeight -= PlayerWeight;
                    break;
                }
            case "Square Moveables":
                {
                    currentWeight -= SquareMoveablesWeight;
                    break;
                }
            case "Heavy Moveables":
                {
                    currentWeight -= HeavyMoveablesWeight;
                    break;
                }
            case "Light Moveables":
                {
                    currentWeight -= LightMoveablesWeight;
                    break;
                }
            case "Sphere Moveables":
                {
                    currentWeight -= SphereMoveablesWeight;
                    break;
                }
        }
    }

    #endregion

    #region ActivationControll

    private void ActivateButton()
    {

        if (isLocked == false)
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

        standardScale = gameObject.transform.localScale;
    }

    private void Update()
    {
        isActiveString = isActive.ToString();

        if (currentWeight < 0)
        {
            currentWeight = 0;
        }

        if (isLocked == true && isActive == true)
        {
            anime.Play("ButtonPadReverse");
            buttonAudio.Play();
            isActive = false;
        }

        ActivateButton();
        DeActivateButton();
    }

    #endregion

    #region OnTrigger

    private void OnTriggerEnter(Collider other)
    {
        AddWeight(other);
        ExtendCollider();
    }

    private void OnTriggerExit(Collider other)
    {
        SubtractWeight(other);
        RetractCollider();
    }

#endregion
}
