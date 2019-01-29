using Vexe.Runtime.Types;
using UnityEngine;

public class StrengthBoost : BaseBehaviour
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
    [Comment("Shows if the StrengthBoost is currently is active", helpButton: true)]
    public bool activated;

    // Objects
    private GameObject Player;

    // ObjectValues
    private float LightRate;
    private float LightChargeRate;
    private float LightMaxThrowForce;
    private float LightRepositionRate;
    private float LightRotateRate;

    private float HeavyRate;
    private float HeavyChargeRate;
    private float HeavyMaxThrowForce;
    private float HeavyRepositionRate;
    private float HeavyRotateRate;

    private float SphereRate;
    private float SphereChargeRate;
    private float SphereMaxThrowForce;
    private float SphereRepositionRate;
    private float SphereRotateRate;

    private float SquareRate;
    private float SquareChargeRate;
    private float SquareMaxThrowForce;
    private float SquareRepositionRate;
    private float SquareRotateRate;

    private float NormalRate;
    private float NormalChargeRate;
    private float NormalMaxThrowForce;
    private float NormalRepositionRate;
    private float NormalRotateRate;


    private float defaultLightRate;
    private float defaultLightChargeRate;
    private float defaultLightMaxThrowForce;
    private float defaultLightRepositionRate;
    private float defaultLightRotateRate;

    private float defaultHeavyRate;
    private float defaultHeavyChargeRate;
    private float defaultHeavyMaxThrowForce;
    private float defaultHeavyRepositionRate;
    private float defaultHeavyRotateRate;

    private float defaultSphereRate;
    private float defaultSphereChargeRate;
    private float defaultSphereMaxThrowForce;
    private float defaultSphereRepositionRate;
    private float defaultSphereRotateRate;

    private float defaultSquareRate;
    private float defaultSquareChargeRate;
    private float defaultSquareMaxThrowForce;
    private float defaultSquareRepositionRate;
    private float defaultSquareRotateRate;

    private float defaultNormalRate;
    private float defaultNormalChargeRate;
    private float defaultNormalMaxThrowForce;
    private float defaultNormalRepositionRate;
    private float defaultNormalRotateRate;

    // PercentageUpdates
    [Comment("Choose between 25%/50%/75%/100% to boost strength with", helpButton: true)]
    public PercentageUpdateStrengthBoost percentageUpdateStrengthBoost;

    void Start()
    {
        activated = false;

        Player = GameObject.Find("Project_0_Character");

        LightRate = moveLightMoveables.rate;
        HeavyRate = moveHeavyMoveables.rate;
        SphereRate = moveSphereMoveables.rate;
        SquareRate = moveSquareMoveables.rate;
        NormalRate = moveNormalMoveables.rate;

        LightChargeRate = moveLightMoveables.chargeRate;
        HeavyChargeRate = moveHeavyMoveables.chargeRate;
        SphereChargeRate = moveSphereMoveables.chargeRate;
        SquareChargeRate = moveSquareMoveables.chargeRate;
        NormalChargeRate = moveNormalMoveables.chargeRate;

        LightMaxThrowForce = moveLightMoveables.maxThrowForce;
        HeavyMaxThrowForce = moveHeavyMoveables.maxThrowForce;
        SphereMaxThrowForce = moveSphereMoveables.maxThrowForce;
        SquareMaxThrowForce = moveSquareMoveables.maxThrowForce;
        NormalMaxThrowForce = moveNormalMoveables.maxThrowForce;

        LightRepositionRate = moveLightMoveables.repositionRate;
        HeavyRepositionRate = moveHeavyMoveables.repositionRate;
        SphereRepositionRate = moveSphereMoveables.repositionRate;
        SquareRepositionRate = moveSquareMoveables.repositionRate;
        NormalRepositionRate = moveNormalMoveables.repositionRate;

        LightRotateRate = moveLightMoveables.rotateRate;
        HeavyRotateRate = moveHeavyMoveables.rotateRate;
        SphereRotateRate = moveSphereMoveables.rotateRate;
        SquareRotateRate = moveSquareMoveables.rotateRate;
        NormalRotateRate = moveNormalMoveables.rotateRate;


        defaultLightRate = moveLightMoveables.rate;
        defaultHeavyRate = moveHeavyMoveables.rate;
        defaultSphereRate = moveSphereMoveables.rate;
        defaultSquareRate = moveSquareMoveables.rate;
        defaultNormalRate = moveNormalMoveables.rate;

        defaultLightChargeRate = moveLightMoveables.chargeRate;
        defaultHeavyChargeRate = moveHeavyMoveables.chargeRate;
        defaultSphereChargeRate = moveSphereMoveables.chargeRate;
        defaultSquareChargeRate = moveSquareMoveables.chargeRate;
        defaultNormalChargeRate = moveNormalMoveables.chargeRate;

        defaultLightMaxThrowForce = moveLightMoveables.maxThrowForce;
        defaultHeavyMaxThrowForce = moveHeavyMoveables.maxThrowForce;
        defaultSphereMaxThrowForce = moveSphereMoveables.maxThrowForce;
        defaultSquareMaxThrowForce = moveSquareMoveables.maxThrowForce;
        defaultNormalMaxThrowForce = moveNormalMoveables.maxThrowForce;

        defaultLightRepositionRate = moveLightMoveables.repositionRate;
        defaultHeavyRepositionRate = moveHeavyMoveables.repositionRate;
        defaultSphereRepositionRate = moveSphereMoveables.repositionRate;
        defaultSquareRepositionRate = moveSquareMoveables.repositionRate;
        defaultNormalRepositionRate = moveNormalMoveables.repositionRate;

        defaultLightRotateRate = moveLightMoveables.rotateRate;
        defaultHeavyRotateRate = moveHeavyMoveables.rotateRate;
        defaultSphereRotateRate = moveSphereMoveables.rotateRate;
        defaultSquareRotateRate = moveSquareMoveables.rotateRate;
        defaultNormalRotateRate = moveNormalMoveables.rotateRate;
    }

    void Update()
    {
        if (mechanics.StrengthBoostActivated == true)
        {
            ActivateBoost();
            activated = mechanics.StrengthBoostActivated;
        }
        else if (mechanics.StrengthBoostActivated == false)
        {
            UpdateMovementValues(defaultLightRate, defaultHeavyRate, defaultSphereRate, defaultSquareRate, defaultNormalRate, 
                                 defaultLightChargeRate, defaultHeavyChargeRate, defaultSphereChargeRate, defaultSquareChargeRate, defaultNormalChargeRate, 
                                 defaultLightMaxThrowForce, defaultHeavyMaxThrowForce, defaultSphereMaxThrowForce, defaultSquareMaxThrowForce, defaultNormalMaxThrowForce, 
                                 defaultLightRepositionRate, defaultHeavyRepositionRate, defaultSphereRepositionRate, defaultSquareRepositionRate, defaultNormalRepositionRate, 
                                 defaultLightRotateRate, defaultHeavyRotateRate, defaultSphereRotateRate, defaultSquareRotateRate, defaultNormalRotateRate);
        }
    }

    private void UpdateMovementValues(float updatedLightRate, float updatedHeavyRate, float updatedSphereRate, float updatedSquareRate, float updatedNormalRate, float updatedLightChargeRate, float updatedHeavyChargeRate, float updatedSphereChargeRate, float updatedSquareChargeRate, float updatedNormalChargeRate, float updatedLightMaxThrowForce, float updatedHeavyMaxThrowForce, float updatedSphereMaxThrowForce, float updatedSquareMaxThrowForce, float updatedNormalMaxThrowForce, float updatedLightRepositionRate, float updatedHeavyRepositionRate, float updatedSphereRepositionRate, float updatedSquareRepositionRate, float updatedNormalRepositionRate, float updatedLightRotate, float updatedHeavyRotate, float updatedSphereRotate, float updatedSquareRotate, float updatedNormalRotate)
    {
        moveLightMoveables.rate = updatedLightRate;
        moveLightMoveables.chargeRate = updatedLightChargeRate;
        moveLightMoveables.maxThrowForce = updatedLightMaxThrowForce;
        moveLightMoveables.repositionRate = updatedLightRepositionRate;
        moveLightMoveables.rotateRate = updatedLightRotate;

        moveHeavyMoveables.rate = updatedHeavyRate;
        moveHeavyMoveables.chargeRate = updatedHeavyChargeRate;
        moveHeavyMoveables.maxThrowForce = updatedHeavyMaxThrowForce;
        moveHeavyMoveables.repositionRate = updatedHeavyRepositionRate;
        moveHeavyMoveables.rotateRate = updatedHeavyRotate;

        moveSphereMoveables.rate = updatedSphereRate;
        moveSphereMoveables.chargeRate = updatedSphereChargeRate;
        moveSphereMoveables.maxThrowForce = updatedSphereMaxThrowForce;
        moveSphereMoveables.repositionRate = updatedSphereRepositionRate;
        moveSphereMoveables.rotateRate = updatedSphereRotate;

        moveSquareMoveables.rate = updatedSquareRate; ;
        moveSquareMoveables.chargeRate = updatedSquareChargeRate;
        moveSquareMoveables.maxThrowForce = updatedSquareMaxThrowForce;
        moveSquareMoveables.repositionRate = updatedSquareRepositionRate;
        moveSquareMoveables.rotateRate = updatedSquareRotate;

        moveNormalMoveables.rate = updatedNormalRate;
        moveNormalMoveables.chargeRate = updatedNormalChargeRate;
        moveNormalMoveables.maxThrowForce = updatedNormalMaxThrowForce;
        moveNormalMoveables.repositionRate = updatedNormalRepositionRate;
        moveNormalMoveables.rotateRate = updatedNormalRotate;
    }

    private void ActivateBoost()
    {
        if (activated)
        {
            switch (percentageUpdateStrengthBoost)
            {
                case PercentageUpdateStrengthBoost.UpWith125Percentage:
                    float lightRate25 = LightRate * 0.25f;
                    float lightChargeRate25 = LightChargeRate * 0.25f;
                    float lightMaxThrowForce25 = LightMaxThrowForce * 0.25f;
                    float lightRepositionRate25 = LightRepositionRate * 0.25f;
                    float lightRotateRate25 = LightRotateRate * 0.25f;

                    float heavyRate25 = HeavyRate * 0.25f;
                    float heavyChargeRate25 = HeavyChargeRate * 0.25f;
                    float heavyMaxThrowForce25 = HeavyMaxThrowForce * 0.25f;
                    float heavyRepositionRate25 = HeavyRepositionRate * 0.25f;
                    float heavyRotateRate25 = HeavyRotateRate * 0.25f;

                    float sphereRate25 = SphereRate * 0.25f;
                    float sphereChargeRate25 = SphereChargeRate * 0.25f;
                    float sphereMaxThrowForce25 = SphereMaxThrowForce * 0.25f;
                    float sphereRepositionRate25 = SphereRepositionRate * 0.25f;
                    float sphereRotateRate25 = SphereRotateRate * 0.25f;

                    float squareRate25 = SquareRate * 0.25f;
                    float squareChargeRate25 = SquareChargeRate * 0.25f;
                    float squareMaxThrowForce25 = SquareMaxThrowForce * 0.25f;
                    float squareRepositionRate25 = SquareRepositionRate * 0.25f;
                    float squareRotateRate25 = SquareRotateRate * 0.25f;

                    float normalRate25 = NormalRate * 0.25f;
                    float normalChargeRate25 = NormalChargeRate * 0.25f;
                    float normalMaxThrowForce25 = NormalMaxThrowForce * 0.25f;
                    float normalRepositionRate25 = NormalRepositionRate * 0.25f;
                    float normalRotateRate25 = NormalRotateRate * 0.25f;

                    LightRate = LightRate + lightRate25;
                    LightChargeRate = LightChargeRate + lightChargeRate25;
                    LightMaxThrowForce = LightMaxThrowForce + lightMaxThrowForce25;
                    LightRepositionRate = LightRepositionRate + lightRepositionRate25;
                    LightRotateRate = LightRotateRate + lightRotateRate25;

                    HeavyRate = HeavyRate + heavyRate25;
                    HeavyChargeRate = HeavyChargeRate + heavyChargeRate25;
                    HeavyMaxThrowForce = HeavyMaxThrowForce + heavyMaxThrowForce25;
                    HeavyRepositionRate = HeavyRepositionRate + heavyRepositionRate25;
                    HeavyRotateRate = HeavyRotateRate + heavyRotateRate25;

                    SphereRate = SphereRate + sphereRate25;
                    SphereChargeRate = SphereChargeRate + sphereChargeRate25;
                    SphereMaxThrowForce = SphereMaxThrowForce + sphereMaxThrowForce25;
                    SphereRepositionRate = SphereRepositionRate + sphereRepositionRate25;
                    SphereRotateRate = SphereRotateRate + sphereRotateRate25;

                    SquareRate = SquareRate + squareRate25;
                    SquareChargeRate = SquareChargeRate + squareChargeRate25;
                    SquareMaxThrowForce = SquareMaxThrowForce + squareMaxThrowForce25;
                    SquareRepositionRate = SquareRepositionRate + squareRepositionRate25;
                    SquareRotateRate = SquareRotateRate + squareRotateRate25;

                    NormalRate = NormalRate + normalRate25;
                    NormalChargeRate = NormalChargeRate + normalChargeRate25;
                    NormalMaxThrowForce = NormalMaxThrowForce + normalMaxThrowForce25;
                    NormalRepositionRate = NormalRepositionRate + normalRepositionRate25;
                    NormalRotateRate = NormalRotateRate + normalRotateRate25;

                    UpdateMovementValues(LightRate, HeavyRate, SphereRate, SquareRate, NormalRate,
                                         LightChargeRate, HeavyChargeRate, SphereChargeRate, SquareChargeRate, NormalChargeRate,
                                         LightMaxThrowForce, HeavyMaxThrowForce, SphereMaxThrowForce, SquareMaxThrowForce, NormalMaxThrowForce,
                                         LightRepositionRate, HeavyRepositionRate, SphereRepositionRate, SquareRepositionRate, NormalRepositionRate,
                                         LightRotateRate, HeavyRotateRate, SphereRotateRate, SquareRotateRate, NormalRotateRate);
                    activated = false;
                    break;

                case PercentageUpdateStrengthBoost.UpWith150Percentage:
                    float lightRate50 = LightRate * 0.50f;
                    float lightChargeRate50 = LightChargeRate * 0.50f;
                    float lightMaxThrowForce50 = LightMaxThrowForce * 0.50f;
                    float lightRepositionRate50 = LightRepositionRate * 0.50f;
                    float lightRotateRate50 = LightRotateRate * 0.50f;

                    float heavyRate50 = HeavyRate * 0.50f;
                    float heavyChargeRate50 = HeavyChargeRate * 0.50f;
                    float heavyMaxThrowForce50 = HeavyMaxThrowForce * 0.50f;
                    float heavyRepositionRate50 = HeavyRepositionRate * 0.50f;
                    float heavyRotateRate50 = HeavyRotateRate * 0.50f;

                    float sphereRate50 = SphereRate * 0.50f;
                    float sphereChargeRate50 = SphereChargeRate * 0.50f;
                    float sphereMaxThrowForce50 = SphereMaxThrowForce * 0.50f;
                    float sphereRepositionRate50 = SphereRepositionRate * 0.50f;
                    float sphereRotateRate50 = SphereRotateRate * 0.50f;

                    float squareRate50 = SquareRate * 0.50f;
                    float squareChargeRate50 = SquareChargeRate * 0.50f;
                    float squareMaxThrowForce50 = SquareMaxThrowForce * 0.50f;
                    float squareRepositionRate50 = SquareRepositionRate * 0.50f;
                    float squareRotateRate50 = SquareRotateRate * 0.50f;

                    float normalRate50 = NormalRate * 0.50f;
                    float normalChargeRate50 = NormalChargeRate * 0.50f;
                    float normalMaxThrowForce50 = NormalMaxThrowForce * 0.50f;
                    float normalRepositionRate50 = NormalRepositionRate * 0.50f;
                    float normalRotateRate50 = NormalRotateRate * 0.50f;

                    LightRate = LightRate + lightRate50;
                    LightChargeRate = LightChargeRate + lightChargeRate50;
                    LightMaxThrowForce = LightMaxThrowForce + lightMaxThrowForce50;
                    LightRepositionRate = LightRepositionRate + lightRepositionRate50;
                    LightRotateRate = LightRotateRate + lightRotateRate50;

                    HeavyRate = HeavyRate + heavyRate50;
                    HeavyChargeRate = HeavyChargeRate + heavyChargeRate50;
                    HeavyMaxThrowForce = HeavyMaxThrowForce + heavyMaxThrowForce50;
                    HeavyRepositionRate = HeavyRepositionRate + heavyRepositionRate50;
                    HeavyRotateRate = HeavyRotateRate + heavyRotateRate50;

                    SphereRate = SphereRate + sphereRate50;
                    SphereChargeRate = SphereChargeRate + sphereChargeRate50;
                    SphereMaxThrowForce = SphereMaxThrowForce + sphereMaxThrowForce50;
                    SphereRepositionRate = SphereRepositionRate + sphereRepositionRate50;
                    SphereRotateRate = SphereRotateRate + sphereRotateRate50;

                    SquareRate = SquareRate + squareRate50;
                    SquareChargeRate = SquareChargeRate + squareChargeRate50;
                    SquareMaxThrowForce = SquareMaxThrowForce + squareMaxThrowForce50;
                    SquareRepositionRate = SquareRepositionRate + squareRepositionRate50;
                    SquareRotateRate = SquareRotateRate + squareRotateRate50;

                    NormalRate = NormalRate + normalRate50;
                    NormalChargeRate = NormalChargeRate + normalChargeRate50;
                    NormalMaxThrowForce = NormalMaxThrowForce + normalMaxThrowForce50;
                    NormalRepositionRate = NormalRepositionRate + normalRepositionRate50;
                    NormalRotateRate = NormalRotateRate + normalRotateRate50;

                    UpdateMovementValues(LightRate, HeavyRate, SphereRate, SquareRate, NormalRate,
                                         LightChargeRate, HeavyChargeRate, SphereChargeRate, SquareChargeRate, NormalChargeRate,
                                         LightMaxThrowForce, HeavyMaxThrowForce, SphereMaxThrowForce, SquareMaxThrowForce, NormalMaxThrowForce,
                                         LightRepositionRate, HeavyRepositionRate, SphereRepositionRate, SquareRepositionRate, NormalRepositionRate,
                                         LightRotateRate, HeavyRotateRate, SphereRotateRate, SquareRotateRate, NormalRotateRate);
                    activated = false;
                    break;

                case PercentageUpdateStrengthBoost.UpWith175Percentage:
                    float lightRate75 = LightRate * 0.75f;
                    float lightChargeRate75 = LightChargeRate * 0.75f;
                    float lightMaxThrowForce75 = LightMaxThrowForce * 0.75f;
                    float lightRepositionRate75 = LightRepositionRate * 0.75f;
                    float lightRotateRate75 = LightRotateRate * 0.75f;

                    float heavyRate75 = HeavyRate * 0.75f;
                    float heavyChargeRate75 = HeavyChargeRate * 0.75f;
                    float heavyMaxThrowForce75 = HeavyMaxThrowForce * 0.75f;
                    float heavyRepositionRate75 = HeavyRepositionRate * 0.75f;
                    float heavyRotateRate75 = HeavyRotateRate * 0.75f;

                    float sphereRate75 = SphereRate * 0.75f;
                    float sphereChargeRate75 = SphereChargeRate * 0.75f;
                    float sphereMaxThrowForce75 = SphereMaxThrowForce * 0.75f;
                    float sphereRepositionRate75 = SphereRepositionRate * 0.75f;
                    float sphereRotateRate75 = SphereRotateRate * 0.75f;

                    float squareRate75 = SquareRate * 0.75f;
                    float squareChargeRate75 = SquareChargeRate * 0.75f;
                    float squareMaxThrowForce75 = SquareMaxThrowForce * 0.75f;
                    float squareRepositionRate75 = SquareRepositionRate * 0.75f;
                    float squareRotateRate75 = SquareRotateRate * 0.75f;

                    float normalRate75 = NormalRate * 0.75f;
                    float normalChargeRate75 = NormalChargeRate * 0.75f;
                    float normalMaxThrowForce75 = NormalMaxThrowForce * 0.75f;
                    float normalRepositionRate75 = NormalRepositionRate * 0.75f;
                    float normalRotateRate75 = NormalRotateRate * 0.75f;

                    LightRate = LightRate + lightRate75;
                    LightChargeRate = LightChargeRate + lightChargeRate75;
                    LightMaxThrowForce = LightMaxThrowForce + lightMaxThrowForce75;
                    LightRepositionRate = LightRepositionRate + lightRepositionRate75;
                    LightRotateRate = LightRotateRate + lightRotateRate75;

                    HeavyRate = HeavyRate + heavyRate75;
                    HeavyChargeRate = HeavyChargeRate + heavyChargeRate75;
                    HeavyMaxThrowForce = HeavyMaxThrowForce + heavyMaxThrowForce75;
                    HeavyRepositionRate = HeavyRepositionRate + heavyRepositionRate75;
                    HeavyRotateRate = HeavyRotateRate + heavyRotateRate75;

                    SphereRate = SphereRate + sphereRate75;
                    SphereChargeRate = SphereChargeRate + sphereChargeRate75;
                    SphereMaxThrowForce = SphereMaxThrowForce + sphereMaxThrowForce75;
                    SphereRepositionRate = SphereRepositionRate + sphereRepositionRate75;
                    SphereRotateRate = SphereRotateRate + sphereRotateRate75;

                    SquareRate = SquareRate + squareRate75;
                    SquareChargeRate = SquareChargeRate + squareChargeRate75;
                    SquareMaxThrowForce = SquareMaxThrowForce + squareMaxThrowForce75;
                    SquareRepositionRate = SquareRepositionRate + squareRepositionRate75;
                    SquareRotateRate = SquareRotateRate + squareRotateRate75;

                    NormalRate = NormalRate + normalRate75;
                    NormalChargeRate = NormalChargeRate + normalChargeRate75;
                    NormalMaxThrowForce = NormalMaxThrowForce + normalMaxThrowForce75;
                    NormalRepositionRate = NormalRepositionRate + normalRepositionRate75;
                    NormalRotateRate = NormalRotateRate + normalRotateRate75;

                    UpdateMovementValues(LightRate, HeavyRate, SphereRate, SquareRate, NormalRate,
                                         LightChargeRate, HeavyChargeRate, SphereChargeRate, SquareChargeRate, NormalChargeRate,
                                         LightMaxThrowForce, HeavyMaxThrowForce, SphereMaxThrowForce, SquareMaxThrowForce, NormalMaxThrowForce,
                                         LightRepositionRate, HeavyRepositionRate, SphereRepositionRate, SquareRepositionRate, NormalRepositionRate,
                                         LightRotateRate, HeavyRotateRate, SphereRotateRate, SquareRotateRate, NormalRotateRate);
                    activated = false;
                    break;

                case PercentageUpdateStrengthBoost.UpWith200Percentage:
                    float lightRate100 = LightRate;
                    float lightChargeRate100 = LightChargeRate;
                    float lightMaxThrowForce100 = LightMaxThrowForce;
                    float lightRepositionRate100 = LightRepositionRate;
                    float lightRotateRate100 = LightRotateRate;

                    float heavyRate100 = HeavyRate;
                    float heavyChargeRate100 = HeavyChargeRate;
                    float heavyMaxThrowForce100 = HeavyMaxThrowForce;
                    float heavyRepositionRate100 = HeavyRepositionRate;
                    float heavyRotateRate100 = HeavyRotateRate;

                    float sphereRate100 = SphereRate;
                    float sphereChargeRate100 = SphereChargeRate;
                    float sphereMaxThrowForce100 = SphereMaxThrowForce;
                    float sphereRepositionRate100 = SphereRepositionRate;
                    float sphereRotateRate100 = SphereRotateRate;

                    float squareRate100 = SquareRate;
                    float squareChargeRate100 = SquareChargeRate;
                    float squareMaxThrowForce100 = SquareMaxThrowForce;
                    float squareRepositionRate100 = SquareRepositionRate;
                    float squareRotateRate100 = SquareRotateRate;

                    float normalRate100 = NormalRate;
                    float normalChargeRate100 = NormalChargeRate;
                    float normalMaxThrowForce100 = NormalMaxThrowForce;
                    float normalRepositionRate100 = NormalRepositionRate;
                    float normalRotateRate100 = NormalRotateRate;

                    LightRate = LightRate + lightRate100;
                    LightChargeRate = LightChargeRate + lightChargeRate100;
                    LightMaxThrowForce = LightMaxThrowForce + lightMaxThrowForce100;
                    LightRepositionRate = LightRepositionRate + lightRepositionRate100;
                    LightRotateRate = LightRotateRate + lightRotateRate100;

                    HeavyRate = HeavyRate + heavyRate100;
                    HeavyChargeRate = HeavyChargeRate + heavyChargeRate100;
                    HeavyMaxThrowForce = HeavyMaxThrowForce + heavyMaxThrowForce100;
                    HeavyRepositionRate = HeavyRepositionRate + heavyRepositionRate100;
                    HeavyRotateRate = HeavyRotateRate + heavyRotateRate100;

                    SphereRate = SphereRate + sphereRate100;
                    SphereChargeRate = SphereChargeRate + sphereChargeRate100;
                    SphereMaxThrowForce = SphereMaxThrowForce + sphereMaxThrowForce100;
                    SphereRepositionRate = SphereRepositionRate + sphereRepositionRate100;
                    SphereRotateRate = SphereRotateRate + sphereRotateRate100;

                    SquareRate = SquareRate + squareRate100;
                    SquareChargeRate = SquareChargeRate + squareChargeRate100;
                    SquareMaxThrowForce = SquareMaxThrowForce + squareMaxThrowForce100;
                    SquareRepositionRate = SquareRepositionRate + squareRepositionRate100;
                    SquareRotateRate = SquareRotateRate + squareRotateRate100;

                    NormalRate = NormalRate + normalRate100;
                    NormalChargeRate = NormalChargeRate + normalChargeRate100;
                    NormalMaxThrowForce = NormalMaxThrowForce + normalMaxThrowForce100;
                    NormalRepositionRate = NormalRepositionRate + normalRepositionRate100;
                    NormalRotateRate = NormalRotateRate + normalRotateRate100;

                    UpdateMovementValues(LightRate, HeavyRate, SphereRate, SquareRate, NormalRate,
                                         LightChargeRate, HeavyChargeRate, SphereChargeRate, SquareChargeRate, NormalChargeRate,
                                         LightMaxThrowForce, HeavyMaxThrowForce, SphereMaxThrowForce, SquareMaxThrowForce, NormalMaxThrowForce,
                                         LightRepositionRate, HeavyRepositionRate, SphereRepositionRate, SquareRepositionRate, NormalRepositionRate,
                                         LightRotateRate, HeavyRotateRate, SphereRotateRate, SquareRotateRate, NormalRotateRate);
                    activated = false;
                    break;
            }
        }
    }
}
