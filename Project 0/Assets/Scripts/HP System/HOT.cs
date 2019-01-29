using UnityEngine;
using Vexe.Runtime.Types;

public class HOT : BaseBehaviour
{
    private HpMeter hpMeter;

    [Comment("Set this variable to define how much this certain item should heal over time", helpButton: true)]
    public float Regen = 0f;

    [Comment("Set this variable to define how much time this certain item should heal", helpButton: true)]
    public float Timer = 0f;

    private void Start()
    {
        hpMeter = GameObject.Find("Game Mechanics").GetComponent<HpMeter>();
    }
    
    private void OnTriggerEnter(Collider col)
    {
        if (hpMeter.currentHp < hpMeter.maxHp)
        {
            hpMeter.OtherRegenSource = true;
            hpMeter.timeLimit = (int)Timer;
            hpMeter.hpRegenAlt = Regen;
            hpMeter.healthTimer = Timer;

            Destroy(gameObject);
        }
    }
}
