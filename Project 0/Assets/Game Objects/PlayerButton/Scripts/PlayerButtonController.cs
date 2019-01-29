using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerButtonController : MonoBehaviour
{
    public Animation anim;

    [Header("Bools:")]
    [Tooltip("Shows if the button is being ativated")]
    public bool isBeingActivated;
    [Tooltip("Shows if the button is active")]
    public bool isActive;

    [Header("Timer:")]
    [Tooltip("Sets the time (in seconds) that the button is active")]
    public float TimerDuration;
    [Header("Don't touch:")]
    [Tooltip("Shows the time left on the timer")]
    public float TimeLeft;
    float Duration;

    [Header("Force Field:")]
    [Tooltip("if true this will use the timer instead of activating the FFC permanently")]
    public bool UnlockForceFieldUsingTimer;
    [Tooltip("Sets the FFC")]
    public ForceFieldsController FFC;

    [Header("Button:")]
    [Tooltip("if true this will use the timer instead of activating the BC2 permanently")]
    public bool UnlockButtonUsingTimer;
    [Tooltip("Sets the BC2")]
    public ButtonController_2 BC2;

    [Header("Blocks:")]
    [Tooltip("if true this will use the timer instead of activating the BRC permanently")]
    public bool RemoveBlocksUsingTimer;
    [Tooltip("Sets the BRC")]
    public BlockRemovalController[] BRC;

    private void Update()
    {
        switch (isBeingActivated)
        {
            case true:
                {
                    Duration = TimerDuration;
                    isActive = true;
                    anim.Play();
                    Activate();
                    break;
                }
            case false:
                {
                    Activate();
                    break;
                }
        }
    }

    void Activate()
    {
        isBeingActivated = false;

        switch (isActive)
        {
            case true:
                {
                    if (UnlockForceFieldUsingTimer && FFC != null || !UnlockForceFieldUsingTimer && FFC != null)
                    {
                        FFCActivate();
                    }
                    if (UnlockButtonUsingTimer && BC2 != null || !UnlockButtonUsingTimer && BC2 != null)
                    {
                        BC2Active();
                    }
                    if (RemoveBlocksUsingTimer && BRC != null || !RemoveBlocksUsingTimer && BRC != null)
                    {
                        BRCActivate();
                    }

                    break;
                }
            case false:
                {
                    if (UnlockForceFieldUsingTimer && !isActive)
                    {
                        FFClock();
                    }
                    if (UnlockButtonUsingTimer && !isActive)
                    {
                        BC2lock();
                    }
                    if (RemoveBlocksUsingTimer && !isActive)
                    {
                        BRClock();
                    }
                    break;
                }
        }
    }

    void BRCActivate()
    {
        if (RemoveBlocksUsingTimer && isActive)
        {
            Timer();
            BRCunlock();
        }
        else if (!RemoveBlocksUsingTimer && isActive)
        {
            BRCunlock();
        }
    }

    void FFCActivate()
    {
        if (UnlockForceFieldUsingTimer && isActive)
        {
            Timer();
            FFCunlock();
        }
        else if (!UnlockForceFieldUsingTimer && isActive)
        {
            FFCunlock();
        }
    }

    void BC2Active()
    {
        if (UnlockButtonUsingTimer && isActive)
        {
            Timer();
            BC2unlock();
        }
        else if (!UnlockButtonUsingTimer && isActive)
        {
            BC2unlock();
        }
    }

    void FFCunlock()
    {

        switch (isActive)
        {
            case true:
                {
                    FFC.playerButtonOverride = true;
                    break;
                }
            case false:
                {
                    break;
                }
        }

    }

    void FFClock()
    {
        switch (isActive)
        {
            case true:
                {
                    break;
                }
            case false:
                {
                    FFC.playerButtonOverride = false;
                    break;
                }
        }

    }

    void BC2unlock()
    {

        switch (isActive)
        {
            case true:
                {
                    BC2.isLocked = false;

                    break;
                }
            case false:
                {
                    break;
                }
        }

    }

    void BC2lock()
    {

        switch (isActive)
        {
            case true:
                {
                    break;
                }
            case false:
                {
                    BC2.isLocked = true;
                    break;
                }


        }
    }

    void BRCunlock()
    {

        switch (isActive)
        {
            case true:
                {
                    for (int i = 0; i <= BRC.Length - 1; i++)
                    {
                        BRC[i].gameObject.SetActive(false);
                    }
                    break;
                }
            case false:
                {
                    break;
                }
        }

    }

    void BRClock()
    {
        switch (isActive)
        {
            case true:
                {
                    break;
                }
            case false:
                {
                    for (int i = 0; i <= BRC.Length - 1; i++)
                    {
                        BRC[i].gameObject.SetActive(true);
                    }
                    break;
                }
        }

    }

    void Timer()
    {
        if (Duration < 0)
        {
            isActive = false;
        }
        else
        {
            Duration -= Time.deltaTime;
            TimeLeft = (int)Mathf.RoundToInt(Duration);
        }
    }
}