using Vexe.Runtime.Types;
using UnityEngine;

public class PsyBoost : BaseBehaviour
{
    // Clases
    [Comment("Object that the Mechanics class is attatched to", helpButton: true)]
    public Mechanics mechanics;

    [Comment("Object that the MoveLightMoveables class is attatched to", helpButton: true)]
    public MoveLightMoveables moveLightMoveables;

    [Comment("Object that the MoveHeavyMoveables class is attatched to", helpButton: true)]
    public MoveHeavyMoveables moveHeavyMoveables;

    [Comment("Object that the MoveSphereMoveables class is attatched to", helpButton: true)]
    public MoveSphereMoveables moveSphereMoveables;

    [Comment("Object that the MoveSquareMoveables class is attatched to", helpButton: true)]
    public MoveSquareMoveables moveSquareMoveables;

    [Comment("Object that the MoveNormalMoveables class is attatched to", helpButton: true)]
    public MoveNormalMoveables moveNormalMoveables;

    // Bools
    [Comment("Shows if the PsyBoost is currently is active", helpButton: true)]
    public bool activated;

    // Objects
    private GameObject Player;

    // Object Values
    float LightReach;
    float NormalReach;
    float HeavyReach;
    float SphereReach;
    float SquareReach;

    float defaultLightReach;
    float defaultNormalReach;
    float defaultHeavyReach;
    float defaultSphereReach;
    float defaultSquareReach;

    // PercentageUpdates
    [Comment("Choose between 25%/50%/75%/100% to boost psy with", helpButton: true)]
    public PercentageUpdatePsyBoost percentageUpdatePsyBoost;

    void Start()
    {
        activated = false;

        Player = GameObject.Find("Project_0_Character");

        LightReach = moveLightMoveables.reach;
        NormalReach = moveNormalMoveables.reach;
        HeavyReach = moveHeavyMoveables.reach;
        SphereReach = moveSphereMoveables.reach;
        SquareReach = moveSquareMoveables.reach;

        defaultLightReach = moveLightMoveables.reach;
        defaultNormalReach = moveNormalMoveables.reach;
        defaultHeavyReach = moveHeavyMoveables.reach;
        defaultSphereReach = moveSphereMoveables.reach;
        defaultSquareReach = moveSquareMoveables.reach;
    }

    void Update()
    {
        if (mechanics.PsyBoostActivated == true)
        {
            ActivateBoost();
            activated = mechanics.PsyBoostActivated;
        }
        else if (mechanics.PsyBoostActivated == false)
        {
            UpdateMovementValues(defaultLightReach,
                                 defaultNormalReach,
                                 defaultHeavyReach,
                                 defaultSphereReach,
                                 defaultSquareReach);
        }
    }

    private void UpdateMovementValues(float updatedLightReach, float updatedNormalReach, float updatedHeavyReach, float updatedSphereReach, float updatedSquareReach)
    {
        moveLightMoveables.reach = updatedLightReach;
        moveNormalMoveables.reach = updatedNormalReach;
        moveHeavyMoveables.reach = updatedHeavyReach;
        moveSphereMoveables.reach = updatedSphereReach;
        moveSquareMoveables.reach = updatedSquareReach;
    }

    private void ActivateBoost()
    {
        if (activated)
        {
            switch (percentageUpdatePsyBoost)
            {
                case PercentageUpdatePsyBoost.UpWith125Percentage:
                    float lightReach25 = LightReach * 0.25f;
                    float normalReach25 = NormalReach * 0.25f;
                    float heavyReach25 = HeavyReach * 0.25f;
                    float sphereReach25 = SphereReach * 0.25f;
                    float squareReach25 = SquareReach * 0.25f;

                    LightReach = LightReach + lightReach25;
                    NormalReach = NormalReach + normalReach25;
                    HeavyReach = HeavyReach + heavyReach25;
                    SphereReach = SphereReach + sphereReach25;
                    SquareReach = SquareReach + squareReach25;

                    UpdateMovementValues(LightReach,
                                         NormalReach,
                                         HeavyReach,
                                         SphereReach,
                                         SquareReach);
                    activated = false;
                    break;

                case PercentageUpdatePsyBoost.UpWith150Percentage:

                    float lightReach50 = LightReach * 0.50f;
                    float normalReach50 = NormalReach * 0.50f;
                    float heavyReach50 = HeavyReach * 0.50f;
                    float sphereReach50 = SphereReach * 0.50f;
                    float squareReach50 = SquareReach * 0.50f;

                    LightReach = LightReach + lightReach50;
                    NormalReach = NormalReach + normalReach50;
                    HeavyReach = HeavyReach + heavyReach50;
                    SphereReach = SphereReach + sphereReach50;
                    SquareReach = SquareReach + squareReach50;

                    UpdateMovementValues(LightReach,
                                         NormalReach,
                                         HeavyReach,
                                         SphereReach,
                                         SquareReach);
                    activated = false;
                    break;

                case PercentageUpdatePsyBoost.UpWith175Percentage:

                    float lightReach75 = LightReach * 0.75f;
                    float normalReach75 = NormalReach * 0.75f;
                    float heavyReach75 = HeavyReach * 0.75f;
                    float sphereReach75 = SphereReach * 0.75f;
                    float squareReach75 = SquareReach * 0.75f;

                    LightReach = LightReach + lightReach75;
                    NormalReach = NormalReach + normalReach75;
                    HeavyReach = HeavyReach + heavyReach75;
                    SphereReach = SphereReach + sphereReach75;
                    SquareReach = SquareReach + squareReach75;

                    UpdateMovementValues(LightReach,
                                         NormalReach,
                                         HeavyReach,
                                         SphereReach,
                                         SquareReach);
                    activated = false;
                    break;

                case PercentageUpdatePsyBoost.UpWith200Percentage:

                    float lightReach100 = LightReach * 0.100f;
                    float normalReach100 = NormalReach * 0.100f;
                    float heavyReach100 = HeavyReach * 0.100f;
                    float sphereReach100 = SphereReach * 0.100f;
                    float squareReach100 = SquareReach * 0.100f;

                    LightReach = LightReach + lightReach100;
                    NormalReach = NormalReach + normalReach100;
                    HeavyReach = HeavyReach + heavyReach100;
                    SphereReach = SphereReach + sphereReach100;
                    SquareReach = SquareReach + squareReach100;

                    UpdateMovementValues(LightReach,
                                         NormalReach,
                                         HeavyReach,
                                         SphereReach,
                                         SquareReach);
                    activated = false;
                    break;
            }
        }
    }
}
