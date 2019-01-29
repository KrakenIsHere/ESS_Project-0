using UnityEngine;
using UnityEngine.UI;
using Vexe.Runtime.Types;

[RequireComponent(typeof(Transform))]
public class HpMeter : BaseBehaviour
{
    private GameSettings.GameState currentGameState;
    
    private GameSettings gameSettings;
    private Dmg[] dmg;
    private DmgOverTime[] DOT;

    public Transform hpMeter;

    public Text uiHealthText;
    public float healthTimer;

    public int dmgCount;

    public float currentHp = 50f;
    public float maxHp = 50f;
    public float hpRegen = .05f;

    private float defaultRegen;
    
    public float hpRegenAlt;

    public int timeLimit = 30;
    public float timeSpent = 0;
    private float timer = 0f;

    public bool IsRegenDeactivated = false;
    public bool OtherRegenSource = false;
    
    private void Awake()
    {
        gameSettings = GameObject.Find("Settings").GetComponent<GameSettings>();
        dmg = FindObjectsOfType<Dmg>();
        DOT = FindObjectsOfType<DmgOverTime>();

        dmgCount = dmg.Length;

        defaultRegen = hpRegen;
        currentGameState = gameSettings.CurrentGameState;
    }

    private void Update()
    {
        UpdateGameSettings();

        ConsoleProDebug.Watch("Timespent Value", timeSpent.ToString());

        if (currentHp < maxHp)
        {
            switch (currentGameState)
            {
                case GameSettings.GameState.Paused:

                    break;

                case GameSettings.GameState.Unpaused:

                    if (IsRegenDeactivated)
                    {
                        hpRegen = 0f;
                        hpRegenAlt = 0f;
                    }
                    else
                    {
                        hpRegen = defaultRegen;
                    }

                    if (OtherRegenSource)
                    {
                        Regen(hpRegenAlt, true);
                        TimerStart(true);
                    }
                    else
                    {
                        Regen(hpRegen, true);
                        TimerStart(false);
                    }
                    break;
            }
        }
        else
        {
            currentHp = 50f;
        }

        hpMeter.GetComponent<RectTransform>().localScale = new Vector3(currentHp / maxHp, 1, 1);
    }

    private void TimerStart(bool HOT)
    {
        switch (HOT)
        {
            case true:
                if (timeSpent < timeLimit)
                {
                    if (healthTimer > 0)
                    {
                        ConsoleProDebug.LogToFilter("Greater than", "Test");
                        uiHealthText.enabled = true;
                        healthTimer -= Time.deltaTime;
                        uiHealthText.text = healthTimer.ToString();
                    }
                    else if (healthTimer <= 0)
                    {
                        ConsoleProDebug.LogToFilter("Less than", "Test");
                        uiHealthText.enabled = false;
                        healthTimer = 0f;
                        timeSpent = 0f;
                    }
                }
                break;
        }
    }

    private void Regen(float regenvalue, bool activated)
    {
        switch (activated)
        {
            case true:
                switch ((int)timer)
                {
                    case 1:
                        currentHp += regenvalue;
                        timeSpent += 1f;
                        timer = 0;
                        break;
                }
                break;
        }
    }

    private bool DeactivateRegenDOT()
    {
        foreach (DmgOverTime dot in DOT)
        {
            if (dot.IsRegenDeactivated)
            {
                IsRegenDeactivated = true;
            }
            else if (!dot.IsRegenDeactivated)
            {
                IsRegenDeactivated = false;
            }
        }

        return IsRegenDeactivated;
    }

    private bool DeactivateRegenDMG()
    {
        foreach (Dmg damage in dmg)
        {
            if (damage.IsRegenDeactivated)
            {
                IsRegenDeactivated = true;
            }
            else if (!damage.IsRegenDeactivated)
            {
                IsRegenDeactivated = false;
            }
        }

        return IsRegenDeactivated;
    }

    private void UpdateGameSettings()
    {
        currentGameState = gameSettings.CurrentGameState;
        
        switch(DeactivateRegenDMG() || DeactivateRegenDOT())
        {
            case true:
                IsRegenDeactivated = true;
                break;

            case false:
                IsRegenDeactivated = false;
                break;
        }

    }
}
