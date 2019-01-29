using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndObjectController : MonoBehaviour {

    [Tooltip("The name on the Level/Scene you wish to load")]
    public string levelToLoad;

	private IEnumerator ChangeLevel()
    {


        //int currentLevel = SceneManager.GetActiveScene().buildIndex;
        //int nextLevel = currentLevel + 1;
        //string levelToUnload = "Level" + currentLevel;
        //string levelToLoad = "Level" + nextLevel;


        float timeToFade = GameObject.Find("_GameController").GetComponent<EndLevelFadeController>().FadeBegin(1);
        yield return new WaitForSeconds(0.6f);

        SceneManager.LoadSceneAsync(levelToLoad);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(ChangeLevel());
        }
    }
}
