using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGame_Controller : MonoBehaviour
{
    public string levelToLoad = "Tutorial";

    public void NewGameButton_Click()
    {
        SceneManager.LoadSceneAsync(levelToLoad);
    }

}
