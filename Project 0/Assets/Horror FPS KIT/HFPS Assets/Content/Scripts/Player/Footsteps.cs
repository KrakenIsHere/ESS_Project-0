using UnityEngine;
using System.Collections;

public class Footsteps : MonoBehaviour
{
	[Header("Main")]
    public AudioSource soundsGO;
    public CharacterController controller;
    public PlayerController playerController;

	[System.Serializable]
	public class footsteps
	{
		public string groundTag;
		public AudioClip[] footstep;
	}

	[Header("Player Footsteps")]
	[Tooltip("Element 0 is always Untagged/Concrete and 1 is ladder")]
	public footsteps[] Steps;

	public float audioVolumeCrouch = 0.1f;
	public float audioVolumeWalk = 0.2f;
	public float audioVolumeRun = 0.3f;

	public float audioStepLengthCrouch = 0.75f;
	public float audioStepLengthWalk = 0.45f;
	public float audioStepLengthRun = 0.25f;
	public float minWalkSpeed = 5;
	public float maxWalkSpeed = 9.0f;
    private bool step = true;
    private string curMat;

    void OnEnable()
    {
        step = true;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        float speed = controller.velocity.magnitude;
        curMat = hit.gameObject.tag;

		if (playerController.state == 2 || !step) return;

        if (controller.isGrounded && hit.normal.y > 0.3f)
        {
			for (int i = 0; i < Steps.Length; i++)
			{
				if (curMat == Steps[i].groundTag) 
				{
					if (speed > maxWalkSpeed) StartCoroutine(RunOnGround());
					else if (speed < maxWalkSpeed && speed > minWalkSpeed) StartCoroutine(WalkOnGround());
					else if (speed < minWalkSpeed && speed > 0.5f) StartCoroutine(CrouchOnGrouund());
				}
			}
        }
    }
	
	//Ladder footsteps
	public void PlayLadderSound()
	{
		soundsGO.PlayOneShot(Steps[1].footstep[Random.Range(0, Steps[1].footstep.Length)], audioVolumeWalk);
	}

    public IEnumerator JumpLand()
    {
        if (!soundsGO.enabled) yield break;

		for (int i = 0; i < Steps.Length; i++)
		{
			if (curMat == Steps[i].groundTag) 
			{
				soundsGO.PlayOneShot(Steps[i].footstep[Random.Range(0, Steps[i].footstep.Length)], 0.5f);
				yield return new WaitForSeconds(0.12f);
				soundsGO.PlayOneShot(Steps[i].footstep[Random.Range(0, Steps[i].footstep.Length)], 0.4f);
			}
		}
    }

	IEnumerator CrouchOnGrouund()
	{
		for (int i = 0; i < Steps.Length; i++)
		{
			if (curMat == Steps[i].groundTag)
			{
				step = false;
				soundsGO.PlayOneShot (Steps[i].footstep [Random.Range (0, Steps[i].footstep.Length)], audioVolumeCrouch);
				yield return new WaitForSeconds (audioStepLengthCrouch);
				step = true;
			}
		}
	}

	IEnumerator WalkOnGround()
	{
		for (int i = 0; i < Steps.Length; i++)
		{
			if (curMat == Steps[i].groundTag)
			{
				step = false;
				soundsGO.PlayOneShot(Steps[i].footstep[Random.Range(0, Steps[i].footstep.Length)], audioVolumeWalk);
				yield return new  WaitForSeconds (audioStepLengthWalk);
				step = true;
			}
		}
	}

	IEnumerator RunOnGround()
	{
		for (int i = 0; i < Steps.Length; i++)
		{
			if (curMat == Steps[i].groundTag)
			{
				step = false;
				soundsGO.PlayOneShot(Steps[i].footstep[Random.Range(0, Steps[i].footstep.Length)], audioVolumeRun);
				yield return new  WaitForSeconds (audioStepLengthRun);
				step = true;
			}
		}
	}
}