using UnityEngine.SceneManagement;
using UnityEngine;

public class CustomPauseMenu : MonoBehaviour
{
    public string MenuName;

    [Tooltip("Makes it so that certain code doesnt run if the script is in a menu (Ex. Main Menu)")]
    public bool isInMenu;

    GameObject go;
    GameSettings gameSettings;


    public GameSettings.GameState CurrentGameState;

    bool IsPaused;
    bool LockCursor;

    public GameObject uiBackGround;
    public GameObject settingsMenuUI;
    public GameObject pauseMenuUI;

    GameObject StaminaBar;
    GameObject PsyBar;
    GameObject HPBar;

    private void Awake()
    {
            SetVariablesOnStartup();
    }

    void Update ()
    {
        // Updates Variables And Send them To The GameSettings Script. 
        UpdateGameSettings();

        SetGameState();
        PauseState();

        // Enables Or Disables Pause Menu Overlay Depending On Current State.
        if (Input.GetButtonDown("Pause"))
        {
            switch (IsPaused)
            {
                case true:
                    Resume();
                    break;

                case false:
                    Pause();
                    break;
            }
        }
	}

    // Sets The Variables On Startup.
    void SetVariablesOnStartup()
    {
        go = GameObject.Find("Settings");
        gameSettings = go.GetComponent<GameSettings>();

        StaminaBar = GameObject.Find("STA Bar");
        PsyBar = GameObject.Find("PSY Bar");
        HPBar = GameObject.Find("HP Bar");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        LockCursor = true;

        IsPaused = false;
        CurrentGameState = GameSettings.GameState.Unpaused;

        CheckGameStateOnStartUp();
    }

    // Checks If Everything Is As It Should be From The Start.
    public void CheckGameStateOnStartUp()
    {
        switch (!LockCursor)
        {
            case true:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                LockCursor = true;
                break;
        }

        switch (Time.timeScale == 0f)
        {
            case true:
                Time.timeScale = 1f;
                break;
        }

        switch (CurrentGameState)
        {
            case GameSettings.GameState.Paused:
                CurrentGameState = GameSettings.GameState.Unpaused;
                break;
        }
    }

    // Changes The GameState Depending On What The Current GameState Is When Pressing Escape.
    public void SetGameState()
    {
        if (Input.GetButtonDown("Pause") && Time.timeScale == 0f)
        {
            Time.timeScale = 1f;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            LockCursor = true;

            CurrentGameState = GameSettings.GameState.Unpaused;
        }
        else if (Input.GetButtonDown("Pause") && Time.timeScale == 1f)
        {
            Time.timeScale = 0f;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            LockCursor = false;

            CurrentGameState = GameSettings.GameState.Paused;
        }
    }

    // Checks If The Game Is Paused Or Not.
    public void PauseState()
    {
        switch(Time.timeScale == 1f && LockCursor)
        {
            case true:
                CurrentGameState = GameSettings.GameState.Unpaused;
                gameSettings.GameIsPaused = false;
                break;

            case false:
                CurrentGameState = GameSettings.GameState.Paused;
                gameSettings.GameIsPaused = true;
                break;
        }
    }

    // Resume Game function. 
    public void Resume()
    {
        StaminaBar.SetActive(true);
        PsyBar.SetActive(true);
        HPBar.SetActive(true);

        uiBackGround.SetActive(false);
        settingsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        CurrentGameState = GameSettings.GameState.Unpaused;
        IsPaused = false;

        LockCursor = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Pause Game Function.
    void Pause()
    {
        StaminaBar.SetActive(false);
        PsyBar.SetActive(false);
        HPBar.SetActive(false);

        uiBackGround.SetActive(true);
        settingsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        CurrentGameState = GameSettings.GameState.Paused;
        IsPaused = true;

        LockCursor = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    // Load Main Menu Function.
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(MenuName);
    }

    // Quit To Desktop Function.
    public void QuitMenu()
    {
        Application.Quit();
    }

    public void UpdateGameSettings()
    {
            gameSettings.CurrentGameState = CurrentGameState;
            gameSettings.GameIsPaused = IsPaused;
            gameSettings.CursorLock = LockCursor;
    }
}
