using Vexe.Runtime.Types;
using UnityEngine;

public class ActivateAnimation : BaseBehaviour
{
    private Animation anim;
    public bool activateScript;
    public bool hasPlayed;

    private void Awake()
    {
        anim = gameObject.GetComponent<Animation>();        
    }

    private void Update()
    {
        PlayAnimation();
    }

    public void PlayAnimation()
    {
        if (hasPlayed)
        {
            activateScript = false;
        }

        if (activateScript)
        {
            anim.Play("fluidRemoval");
            hasPlayed = true;
        }
    }
}
