using Vexe.Runtime.Types;
using UnityEngine;

public class Dmg : BaseBehaviour
{
    private HpMeter hpMeter;

    [Comment("Enable this to damage until you're out of the collision area", helpButton: true)]
    public bool EnableDamageOnStay = false;

    [Comment("Set this variable to define the damage output", helpButton: true)]
    public float Damage = 1f;

    [Hide]
    public bool IsRegenDeactivated = false;

    private void Awake()
    {
        hpMeter = GameObject.Find("Game Mechanics").GetComponent<HpMeter>();
    }

    private void ContinuousDamage()
    {
        hpMeter.currentHp -= Damage;
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Project_0_Character")
        {
            switch (EnableDamageOnStay)
            {
                case true:
                    InvokeRepeating("ContinuousDamage", 1f, 1f);
                    IsRegenDeactivated = true;
                    break;

                case false:
                    hpMeter.currentHp -= Damage;
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        CancelInvoke("ContinuousDamage");
        IsRegenDeactivated = false;
    }
}
