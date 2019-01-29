using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice_Controller : MonoBehaviour
{
    [Header("Self handeling")]
    public int result;
    [Range(1, 6)]
    public int toBeRight;
    public bool isRight;
    int i = 0;

    [Range(200, 400)]
    public float force_y;

    Vector3 originalPos;
    Vector3 force;

    public bool Activate;
    public bool mouseUsed;
    bool thrown;
    bool hasLanded;
    bool onGround;

    public float x;
    public float y;
    public float z;

    public DiceSide_Controller[] diceSides;
    Rigidbody rb;

    [Header("Materials for the Dice sides")]
    public Material[] InactiveMaterials = new Material[6];
    public Material[] ActiveMaterials = new Material[6];

    private void Start()
    {
        originalPos = transform.position;
        rb = GetComponentInParent<Rigidbody>();
    }

    private void Update()
    {
        if (result == toBeRight)
        {
            isRight = true;
            ChangeTexture(isRight);
        }
        else
        {
            isRight = false;
            ChangeTexture(isRight);
        }

        force.y = force_y;

        x = Random.Range(-5000, 5000);
        y = Random.Range(-5000, 5000);
        z = Random.Range(-5000, 5000);

        yAgain:
        if (y < 500 && y > -500)
        {
            y = Random.Range(-5000, 5000);
            goto yAgain;
        }

        if (Activate || mouseUsed)
        {
            ConsoleProDebug.Watch("A dice has been activated ", Activate.ToString());

            RollDice();
        }

        SeeCurrentResult();
    }

    void RollDice()
    {
        if (!thrown && hasLanded)
        {
            thrown = true;
            rb.useGravity = true;
            hasLanded = false;

            rb.AddForce(force);

            rb.AddTorque(x, y, z);
        }
    }

    void SeeCurrentResult()
    {
        if (diceSides[i].isDown)
        {
            result = diceSides[i].OtherSide;
            hasLanded = true;
            thrown = false;
            Activate = false;
            mouseUsed = false;
        }

        i++;

        if (i >= diceSides.Length)
        {
            i = 0;
        }
    }

    void ChangeTexture(bool status)
    {
        switch(status)
        {
            case true:
                {
                    int i = 0;

                    foreach (DiceSide_Controller dice in diceSides)
                    {
                        dice.sideRendere.material = ActiveMaterials[i];
                        i++;
                    }

                    break;
                }
            case false:
                {
                    int i = 0;

                    foreach (DiceSide_Controller dice in diceSides)
                    {
                        dice.sideRendere.material = InactiveMaterials[i];
                        i++;
                    }

                    break;
                }
        }
    }
}
