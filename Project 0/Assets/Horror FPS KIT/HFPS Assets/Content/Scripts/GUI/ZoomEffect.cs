using UnityEngine;
using System.Collections;

public class ZoomEffect : MonoBehaviour {

    private InputManager inputManager;

    [Header("FOV Settings")]
    public float ZoomSpeed = 5f;
    public float NormalFOV;
    public float ZoomFOV;

    private KeyCode ZoomKey;
    private Camera MainCamera;

	void Start () {
        MainCamera = Camera.main;
        inputManager = GetComponent<ScriptManager>().inputManager;
    }
	
	void Update () {
        if (inputManager.DictCount() > 0)
        {
            ZoomKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), inputManager.GetInput("Zoom"));
        }

        if(Input.GetKey(ZoomKey))
        {
            MainCamera.fieldOfView = Mathf.Lerp(MainCamera.fieldOfView, ZoomFOV, ZoomSpeed * Time.deltaTime);
        }
        else
        {
            MainCamera.fieldOfView = Mathf.Lerp(MainCamera.fieldOfView, NormalFOV, ZoomSpeed * Time.deltaTime);
        }
    }
}
