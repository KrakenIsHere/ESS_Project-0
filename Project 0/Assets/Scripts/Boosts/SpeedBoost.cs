using Vexe.Runtime.Types;
using UnityEngine;

public class SpeedBoost : BaseBehaviour
{
    // Clases
    [Comment("Object that the MoveBehaviour class is attatched to", helpButton: true)]
    public MoveBehaviour moveBehaviour;

    [Comment("Object that the Mechanics class is attatched to", helpButton: true)]
    public Mechanics mechanics;

    // Bools
    [Comment("Shows if the Speedboost is currently is active", helpButton: true)]
    public bool activated;

    // Objects
    private GameObject Player;

    // MovementValues
    private float walkSpeed;
    private float runSpeed;
    private float sprintSpeed;

    private float defaultWalkSpeed;
    private float defaultRunSpeed;
    private float defaultSprintSpeed;

    // PercentageUpdates
    [Comment("Choose between 25%/50%/75%/100% to boost speed with", helpButton: true)]
    public PercentageUpdateSpeedBoost percentageUpdateSpeedBoost;

    void Start ()
    {
        activated = false;

        Player = GameObject.Find("Project_0_Character");
        moveBehaviour = Player.GetComponent<MoveBehaviour>();

        walkSpeed = moveBehaviour.walkSpeed;
        runSpeed = moveBehaviour.runSpeed;
        sprintSpeed = moveBehaviour.sprintSpeed;

        defaultWalkSpeed = moveBehaviour.walkSpeed;
        defaultRunSpeed = moveBehaviour.runSpeed;
        defaultSprintSpeed = moveBehaviour.sprintSpeed;
	}

	void Update ()
    {
        if (mechanics.SpeedBoostActivated == true)
        {
            ActivateBoost();
            activated = mechanics.SpeedBoostActivated;
        }
        else if (mechanics.SpeedBoostActivated == false)
        {
            UpdateMovementValues(defaultWalkSpeed, defaultRunSpeed, defaultSprintSpeed);
        }
	}

    private void UpdateMovementValues(float updatedWalk, float updatedRun, float updatedSprint)
    {
        moveBehaviour.walkSpeed = updatedWalk;
        moveBehaviour.runSpeed = updatedRun;
        moveBehaviour.sprintSpeed = updatedSprint;
    }

    private void ActivateBoost()
    {
        if (activated)
        {
            switch (percentageUpdateSpeedBoost)
            {
                case PercentageUpdateSpeedBoost.UpWith125Percentage:
                    float walk25 = walkSpeed * 0.25f;
                    float run25 = runSpeed * 0.25f;
                    float sprint25 = sprintSpeed * 0.25f;

                    walkSpeed = walkSpeed + walk25;
                    runSpeed = runSpeed + run25;
                    sprintSpeed = sprintSpeed + sprint25;

                    UpdateMovementValues(walkSpeed, runSpeed, sprintSpeed);
                    activated = false;
                    break;

                case PercentageUpdateSpeedBoost.UpWith150Percentage:
                    float walk50 = walkSpeed * 0.50f;
                    float run50 = runSpeed * 0.50f;
                    float sprint50 = sprintSpeed * 0.50f;

                    walkSpeed = walkSpeed + walk50;
                    runSpeed = runSpeed + run50;
                    sprintSpeed = sprintSpeed + sprint50;

                    UpdateMovementValues(walkSpeed, runSpeed, sprintSpeed);
                    activated = false;
                    break;

                case PercentageUpdateSpeedBoost.UpWith175Percentage:
                    float walk75 = walkSpeed * 0.75f;
                    float run75 = runSpeed * 0.75f;
                    float sprint75 = sprintSpeed * 0.75f;

                    walkSpeed = walkSpeed + walk75;
                    runSpeed = runSpeed + run75;
                    sprintSpeed = sprintSpeed + sprint75;

                    UpdateMovementValues(walkSpeed, runSpeed, sprintSpeed);
                    activated = false;
                    break;

                case PercentageUpdateSpeedBoost.UpWith200Percentage:
                    float walk100 = walkSpeed;
                    float run100 = runSpeed;
                    float sprint100 = sprintSpeed;

                    walkSpeed = walkSpeed + walk100;
                    runSpeed = runSpeed + run100;
                    sprintSpeed = sprintSpeed + sprint100;

                    UpdateMovementValues(walkSpeed, runSpeed, sprintSpeed);
                    activated = false;
                    break;
            }
        }
    }
}
