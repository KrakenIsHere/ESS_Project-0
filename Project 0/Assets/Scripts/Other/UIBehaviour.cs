using UnityEngine;
using UnityEngine.UI;


public class UIBehaviour : MonoBehaviour
{
    public Image image;
    public Sprite Sprite;

    public void ChangeButtonSprite()
    {
        image.sprite = Sprite;
        ConsoleProDebug.LogToFilter("Test", " Control entered!");
    }
}
