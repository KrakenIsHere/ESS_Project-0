using UnityEngine;
using UnityEngine.UI;
using Vexe.Runtime.Types;

public class TimerTest : BaseBehaviour
{
    public float timeInvoke = 0f;
    float FtimeDelta = 5f;
    public int timeDelta;
    [Space(5)]
    public float currentMana = 100;
    public float startTime = 0f;

    public void Start()
    {
        InvokeRepeating("timer", 1f, 1f);
    }

    private void timer()
    {
        timeInvoke++;
    }

    public void Update()
    {
        FtimeDelta -= Time.deltaTime;
        timeDelta = (int)FtimeDelta;
    }
}