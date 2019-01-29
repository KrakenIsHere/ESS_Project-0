using UnityEngine;


public class GameSettings : MonoBehaviour
{
    // Bools
    public bool GameIsPaused;
    public bool CursorLock;

    // Strings
    public string currentWeightClass;

    // Enums
    public GameState CurrentGameState;
    public CurrentlyGrabbed currentlyGrabbed;

    public enum GameState
    {
        Paused,
        Unpaused
    }

    public enum CurrentlyGrabbed
    {
        NoneGrabbed,
        LightMoveablesGrabbed,
        NormalMoveablesGrabbed,
        HeavyMoveablesGrabbed,
        SquareMoveablesGrabbed,
        SphereMoveablesGrabbed
    }

    public enum RegenType
    {
        None,
        LargePotion,
        SmallPotion
    }
}
