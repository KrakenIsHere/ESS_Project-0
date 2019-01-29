using Vexe.Runtime.Types;
using UnityEngine;

public class Health : BaseBehaviour
{
    private HpMeter hpMeter;

    [Comment("Set this variable to define how much this certain item should heal", helpButton: true)]
    public float Heal = 15f;

    private void Awake()
    {
        hpMeter = GameObject.Find("Game Mechanics").GetComponent<HpMeter>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Project_0_Character")
        {
            if (hpMeter.currentHp < hpMeter.maxHp)
            {
                if (hpMeter.currentHp > hpMeter.maxHp)
                {
                    hpMeter.currentHp = hpMeter.maxHp;
                }
                else
                {
                    hpMeter.currentHp += Heal;
                }

                Destroy(gameObject);
            }
            else
            {
                Debug.Log("You have full HP!");
            }
        }
    }
}
