using Vexe.Runtime.Types;
using UnityEngine;

public class DmgOverTime : BaseBehaviour
{
    private HpMeter hpMeter;

    [Comment("Set this variable to define the damage output per second", helpButton: true)]
    public float Damage = 1f;
    
    [Hide]
    public bool activated = false;

    [Hide]
    public bool IsRegenDeactivated = false;

    public float DmgTimer = 30f;
    private float defaultDmgTimer;

    private void Awake()
    {
        hpMeter = GameObject.Find("Game Mechanics").GetComponent<HpMeter>();
        defaultDmgTimer = DmgTimer;
    }

    private void Update()
    {
        switch (activated)
        {
            case true:
                if (DmgTimer <= 0)
                {
                    activated = false;
                    DmgTimer = defaultDmgTimer;
                    hpMeter.IsRegenDeactivated = false;
                    IsRegenDeactivated = false;
                    CancelInvoke();
                }
                break;
        }
    }

    private void DamageTimer()
    {
        Mathf.Round(DmgTimer--);
    }

    private void TakeDamage()
    {
        hpMeter.currentHp -= Damage;
    }

    private void OnTriggerEnter (Collider col)
    {
        if (col.gameObject.name == "Project_0_Character")
        {
            activated = true;
            hpMeter.IsRegenDeactivated = true;
            IsRegenDeactivated = true;

            switch (activated)
            {
                case true:
                    if (DmgTimer > 0)
                    {
                        InvokeRepeating("DamageTimer", 1f, 1f);
                        InvokeRepeating("TakeDamage", 1f, 1f);
                    }
                    break;
            }
        }
    }
}
