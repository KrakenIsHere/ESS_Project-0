using Vexe.Runtime.Types;
using UnityEngine;
using System.Collections.Generic;

public enum PercentageUpdateSpeedBoost
{
    UpWith125Percentage,
    UpWith150Percentage,
    UpWith175Percentage,
    UpWith200Percentage
}
public enum PercentageUpdateJumpBoost
{
    UpWith125Percentage,
    UpWith150Percentage,
    UpWith175Percentage,
    UpWith200Percentage
}
public enum PercentageUpdateStrengthBoost
{
    UpWith125Percentage,
    UpWith150Percentage,
    UpWith175Percentage,
    UpWith200Percentage
}
public enum PercentageUpdatePsyBoost
{
    UpWith125Percentage,
    UpWith150Percentage,
    UpWith175Percentage,
    UpWith200Percentage
}
public enum BoostTag
{
    speed,
    jump,
    strength,
    psy,
    playerbtn,
    dice
}

public class Mechanics : BaseBehaviour
{
    //Enums
    BoostTag bt = new BoostTag();

    // Classes
    public ActivateAnimation activateAnimation;
    public Timer timer;
    public GetUniqueID guid;
    public MoveBehaviour moveBehaviour;

    // Gameobjects
    private GameObject Player;
    public List<GameObject> hasBeenUsed;
    private GameObject Object;

    // Strings
    private string buffNameTemp;
    private List<string> usedGuids = new List<string>();

    // Tags
    [Tags]
    [Comment("Tag(s) for the Boost Station(s)", helpButton: true)]
    public new string tag = null;

    // Bools
    private bool activate;

    [Comment("Visualizes if Strengthboost is active in inspector", helpButton: true)]
    public bool SpeedBoostActivated;

    [Comment("Visualizes if Jumpboost is active in inspector", helpButton: true)]
    public bool JumpBoostActivated;

    [Comment("Visualizes if Speedboost is active in inspector", helpButton: true)]
    public bool StrengthBoostActivated;

    [Comment("Visualizes if Psyboost is active in inspector", helpButton: true)]
    public bool PsyBoostActivated;

    // Collision
    [Comment("A layermask is a bitmask of one or more layer which allow the casted ray to detect the chosen layer", helpButton: true)]
    public LayerMask layerMask;

    [Comment("From where the ray will come. Recommended: Main Camera", helpButton: true)]
    public Transform center = null;

    [fMax(5)]
    [fMin(0.5f)]
    [Comment("From how far you can reach objects", helpButton: true)]
    public float reach = 2.5f;

    private void Start()
    {
        Player = GameObject.Find("Project_0_Character");
    }

    private void Update()
    {
        ActivateBoosts();
    }

    private void ActivateBoosts()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (Input.GetKey(KeyCode.E))
            {
                Ray ray = new Ray(center.position, center.forward);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, reach, layerMask))
                {
                    ConsoleProDebug.Watch("Current colider tag: ", hit.collider.tag);
                    // Determines which tag on the object you're pointing at
                    switch (hit.collider.tag)
                    {
                        case "Strength-Boost Station":
                            tag = "Strength-Boost Station";
                            bt = BoostTag.strength;
                            break;

                        case "Jump-Boost Station":
                            tag = "Jump-Boost Station";
                            bt = BoostTag.jump;
                            break;

                        case "Speed-Boost Station":
                            tag = "Speed-Boost Station";
                            bt = BoostTag.speed;
                            break;

                        case "Psy-Boost Station":
                            tag = "Psy-Boost Station";
                            bt = BoostTag.psy;
                            break;

                        case "PlayerButton":
                            tag = "PlayerButton";
                            bt = BoostTag.playerbtn;
                            break;
                        case "Dice":
                            tag = "Dice";
                            bt = BoostTag.dice;
                            break;
                    }

                    // Acts based on which enum has been chosen
                    switch (bt)
                    {
                        case BoostTag.speed:
                            if (hit.collider.tag == tag)
                            {
                                Object = hit.collider.gameObject;
                                guid = Object.GetComponent<GetUniqueID>();
                                moveBehaviour = Player.GetComponent<MoveBehaviour>();
                                activateAnimation = Object.GetComponent<ActivateAnimation>();


                                if (!usedGuids.Contains(guid.ID))
                                {
                                    if (guid.hasBeenUsed == false)
                                    {
                                        hasBeenUsed.Add(Object);
                                        usedGuids.Add(guid.ID);
                                        activateAnimation.activateScript = true;
                                        timer = GameObject.Find("Buffs(Speed)").GetComponent<Timer>();
                                        timer.buffTimerActivated = true;
                                        StrengthBoostActivated = true;
                                        StartCoroutine(timer.StartCountdown(timer.buffDuration, "Speed: "));
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }

                                foreach (string str in usedGuids)
                                {
                                    if (usedGuids.Contains(guid.ID))
                                    {
                                        GameObject temp = guid.gameObject;
                                        guid.hasBeenUsed = true;
                                    }
                                }
                            }
                            break;

                        case BoostTag.jump:
                            if (hit.collider.tag == tag)
                            {
                                Object = hit.collider.gameObject;
                                guid = Object.GetComponent<GetUniqueID>();
                                moveBehaviour = Player.GetComponent<MoveBehaviour>();
                                activateAnimation = Object.GetComponent<ActivateAnimation>();


                                if (!usedGuids.Contains(guid.ID))
                                {
                                    if (guid.hasBeenUsed == true)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        hasBeenUsed.Add(Object);
                                        usedGuids.Add(guid.ID);
                                        activateAnimation.activateScript = true;
                                        timer = GameObject.Find("Buffs(Jump)").GetComponent<Timer>();
                                        timer.buffTimerActivated = true;
                                        JumpBoostActivated = true;
                                        StartCoroutine(timer.StartCountdown(timer.buffDuration, "Jump: "));
                                    }
                                }

                                foreach (string str in usedGuids)
                                {
                                    if (usedGuids.Contains(guid.ID))
                                    {
                                        GameObject temp = guid.gameObject;
                                        guid.hasBeenUsed = true;
                                    }
                                }
                            }
                            break;

                        case BoostTag.strength:
                            if (hit.collider.tag == tag)
                            {
                                Object = hit.collider.gameObject;
                                guid = Object.GetComponent<GetUniqueID>();
                                moveBehaviour = Player.GetComponent<MoveBehaviour>();
                                activateAnimation = Object.GetComponent<ActivateAnimation>();


                                if (!usedGuids.Contains(guid.ID))
                                {
                                    if (guid.hasBeenUsed == true)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        hasBeenUsed.Add(Object);
                                        usedGuids.Add(guid.ID);
                                        activateAnimation.activateScript = true;
                                        timer = GameObject.Find("Buffs(Strength)").GetComponent<Timer>();
                                        timer.buffTimerActivated = true;
                                        StrengthBoostActivated = true;
                                        StartCoroutine(timer.StartCountdown(timer.buffDuration, "Strength: "));
                                    }
                                }

                                foreach (string str in usedGuids)
                                {
                                    if (usedGuids.Contains(guid.ID))
                                    {
                                        GameObject temp = guid.gameObject;
                                        guid.hasBeenUsed = true;
                                    }
                                }
                            }
                            break;

                        case BoostTag.psy:
                            if (hit.collider.tag == tag)
                            {
                                Object = hit.collider.gameObject;
                                guid = Object.GetComponent<GetUniqueID>();
                                moveBehaviour = Player.GetComponent<MoveBehaviour>();
                                activateAnimation = Object.GetComponent<ActivateAnimation>();


                                if (!usedGuids.Contains(guid.ID))
                                {
                                    if (guid.hasBeenUsed == true)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        hasBeenUsed.Add(Object);
                                        usedGuids.Add(guid.ID);
                                        activateAnimation.activateScript = true;
                                        timer = GameObject.Find("Buffs(Psy)").GetComponent<Timer>();
                                        timer.buffTimerActivated = true;
                                        PsyBoostActivated = true;
                                        StartCoroutine(timer.StartCountdown(timer.buffDuration, "Psy: "));
                                    }
                                }

                                foreach (string str in usedGuids)
                                {
                                    if (usedGuids.Contains(guid.ID))
                                    {
                                        GameObject temp = guid.gameObject;
                                        guid.hasBeenUsed = true;
                                    }
                                }
                            }
                            break;

                        case BoostTag.playerbtn:
                            if (hit.collider.tag == tag)
                            {
                                PlayerButtonController PBC;

                                Object = hit.collider.gameObject;

                                PBC = Object.GetComponent<PlayerButtonController>();

                                PBC.isBeingActivated = true;
                            }
                            break;

                        case BoostTag.dice:
                            if (hit.collider.tag == tag)
                            {
                                Dice_Controller dice;

                                Object = hit.collider.gameObject;

                                dice = Object.GetComponent<Dice_Controller>();

                                dice.Activate = true;
                            }
                            break;

                    }
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            // If mousebutton1 is not held down, reset the object variable to null so the e button wont activate boosts
            Object = null;
        }
    }
}
