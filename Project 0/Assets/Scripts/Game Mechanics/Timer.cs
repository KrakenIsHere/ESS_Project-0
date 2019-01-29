using UnityEngine.UI;
using Vexe.Runtime.Types;
using UnityEngine;
using System.Collections;

public class Timer : BaseBehaviour
{
    // Classes
    public Mechanics mechanics;
    public SpeedBoost speedBoost;
    public JumpBoost jumpBoost;
    public StrengthBoost strengthBoost;
    public PsyBoost psyBoost;

    // Components
    public Text text;

    // Bools
    public bool buffTimerActivated;

    // Floats
    [fMax(50f)]
    [fMin(10f)]
    [Comment("Time buff will be active, counts in seconds." + "\n" +"(Default is 30 Seconds)", helpButton: true)]
    public float buffDuration = 30.0f;

    [Hide]
    public float currBuffDuration;

    private void Awake()
    {
        text = gameObject.GetComponent<Text>();
    }

    public IEnumerator StartCountdown(float duration, string name)
    {
        currBuffDuration = duration;
        while (currBuffDuration > -1)
        {
            text.enabled = true;
            text.text = name + currBuffDuration;
            yield return new WaitForSeconds(1.0f);
            currBuffDuration--;
        }
        if (currBuffDuration <= 0)
        {
            text.enabled = false;

            buffTimerActivated = false;

            mechanics.tag = null;

            mechanics.JumpBoostActivated = false;
            mechanics.SpeedBoostActivated = false;
            mechanics.StrengthBoostActivated = false;
            mechanics.PsyBoostActivated = false;

            speedBoost.activated = false;
            jumpBoost.activated = false;
            strengthBoost.activated = false;
            psyBoost.activated = false;
        }
    }
}
