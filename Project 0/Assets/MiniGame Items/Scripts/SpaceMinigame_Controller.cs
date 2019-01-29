using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class SpaceMinigame_Controller : MonoBehaviour
{
    public float ShipHealth = 100;
    public float AsteroidDamage = 33.34f;
    public float HealItem = 10;

    [Header("Movement:")]
    [Range(0, 10f)]
    public float moveSpeed_2D;
    [Header("Turning:")]
    [Tooltip("When pressing A")]
    [Range(-360,360)]
    public float turnAngle_Left;
    [Tooltip("When pressing D")]
    [Range(-360, 360)]
    public float turnAngle_Right;

    [Header("Shots")]
    public GameObject shot;
    public Transform Spawn;

    Vector3 CorrectionVector3;
    Vector3 turningAngle;

    Rigidbody thisRigidbody;

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Asteroid":
                {
                    TakeDamage(AsteroidDamage);
                    break;
                }
            case "Heal":
                {
                    MakeHeal(HealItem);
                    break;
                }
        }
    }

    private void Start()
    {
        thisRigidbody = GetComponent<Rigidbody>();
        CorrectionVector3 = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    private void Update ()
    {
        Move(moveSpeed_2D);
    }

    private void Move(float move)
    {
        if (gameObject.transform.position.z >= 4.2f || gameObject.transform.position.x <= -7.9f || gameObject.transform.position.z <= -4.4f || gameObject.transform.position.x >= 8.2f)
        {
            Stop();
            Corrector();
        }
        else
        {
            //var input = Input.inputString;
            //switch(input)
            //{
            //    case "W":
            //        {
            //            break;
            //        }
            //}

            if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            {
                thisRigidbody.velocity = new Vector3(thisRigidbody.velocity.x, 0, moveSpeed_2D);
            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                thisRigidbody.velocity = new Vector3(thisRigidbody.velocity.x, 0, 0);
            }

            if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                Turn(turnAngle_Left, 90, 90);
                thisRigidbody.velocity = new Vector3(-moveSpeed_2D, 0, thisRigidbody.velocity.z);
            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                thisRigidbody.velocity = new Vector3(0, 0, thisRigidbody.velocity.z);
            }

            if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
            {
                thisRigidbody.velocity = new Vector3(thisRigidbody.velocity.x, 0, -moveSpeed_2D);
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                thisRigidbody.velocity = new Vector3(thisRigidbody.velocity.x, 0, 0);
            }

            if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {
                Turn(turnAngle_Right, 270, 270);
                thisRigidbody.velocity = new Vector3(moveSpeed_2D, 0, thisRigidbody.velocity.z);
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                thisRigidbody.velocity = new Vector3(0, 0, thisRigidbody.velocity.z);
            }

            if (Input.GetKey(KeyCode.Space))
            {
                Shoot();
            }

            if (!Input.anyKey)
            {
                Stop();
            }
        }
    }

    private void Corrector()
    {
        CorrectionVector3.x = transform.position.x;
        CorrectionVector3.y = transform.position.y;
        CorrectionVector3.z = transform.position.z;

        if (gameObject.transform.position.z >= 4.2f)
        {
            CorrectionVector3.z = 4.1f;
            gameObject.transform.position = CorrectionVector3;
        }
        if (gameObject.transform.position.x <= -7.9f)
        {
            CorrectionVector3.x = -7.8f;
            gameObject.transform.position = CorrectionVector3;
        }
        if (gameObject.transform.position.z <= -4.4f)
        {
            CorrectionVector3.z = -4.3f;
            gameObject.transform.position = CorrectionVector3;
        }
        if (gameObject.transform.position.x >= 8.2f)
        {
            CorrectionVector3.x = 8.1f;
            gameObject.transform.position = CorrectionVector3;
        }
    }

    private void Stop()
    {
        Turn(90, 90, 90);
        thisRigidbody.velocity = new Vector3(0, 0, 0);
    }

    private void Turn(float X = 0f, float Y = 0f, float Z = 0f)
    {
        turningAngle.y = Y;
        turningAngle.x = X;
        turningAngle.z = Z;

        transform.localEulerAngles = turningAngle;
    }

    private void Shoot()
    {
        Instantiate(shot, Spawn);
    }

    public void MakeHeal(float amount)
    {
        ShipHealth = ShipHealth + amount;
    }

    public void TakeDamage(float amount)
    {
        ShipHealth = ShipHealth - amount;
    }
}
