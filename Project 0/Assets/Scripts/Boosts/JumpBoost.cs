using Vexe.Runtime.Types;
using UnityEngine;

public class JumpBoost : BaseBehaviour
{
    // Clases
    [Comment("Object that the MoveBehaviour class is attatched to", helpButton: true)]
    public MoveBehaviour moveBehaviour;

    [Comment("Object that the Mechanics class is attatched to", helpButton: true)]
    public Mechanics mechanics;

    // Bools
    [Comment("Shows if the JumpBoost is currently is active", helpButton: true)]
    public bool activated;

    // Objects
    private GameObject Player;

    // MovementValues
    private float JumpHeight;
    private float defaultJumpHeight;

    // PercentageUpdates
    [Comment("Choose between 25%/50%/75%/100% to boost jump with", helpButton: true)]
    public PercentageUpdateJumpBoost percentageUpdateJumpBoost;
    
    void Start()
    {
        activated = false;

        Player = GameObject.Find("Project_0_Character");
        moveBehaviour = Player.GetComponent<MoveBehaviour>();

        JumpHeight = moveBehaviour.jumpHeight;
        defaultJumpHeight = moveBehaviour.jumpHeight;
    }

    void Update()
    {
        if (mechanics.JumpBoostActivated == true)
        {
            ActivateBoost();
            activated = mechanics.JumpBoostActivated;
        }
        else if (mechanics.JumpBoostActivated == false)
        {
            UpdateMovementValues(defaultJumpHeight);
        }
    }

    private void UpdateMovementValues(float updatedJump)
    {
        moveBehaviour.jumpHeight = updatedJump;
    }

    private void ActivateBoost()
    {
        if (activated)
        {
            switch (percentageUpdateJumpBoost)
            {
                case PercentageUpdateJumpBoost.UpWith125Percentage:
                    float jump25 = JumpHeight * 0.25f;

                    JumpHeight = JumpHeight + jump25;

                    UpdateMovementValues(JumpHeight);
                    activated = false;
                    break;

                case PercentageUpdateJumpBoost.UpWith150Percentage:
                    float jump50 = JumpHeight * 0.50f;

                    JumpHeight = JumpHeight + jump50;

                    UpdateMovementValues(JumpHeight);
                    activated = false;
                    break;

                case PercentageUpdateJumpBoost.UpWith175Percentage:
                    float jump75 = JumpHeight * 0.75f;

                    JumpHeight = JumpHeight + jump75;

                    UpdateMovementValues(JumpHeight);
                    activated = false;
                    break;

                case PercentageUpdateJumpBoost.UpWith200Percentage:
                    float jump100 = JumpHeight;

                    JumpHeight = JumpHeight + jump100;

                    UpdateMovementValues(JumpHeight);
                    activated = false;
                    break;
            }
        }
    }
}
