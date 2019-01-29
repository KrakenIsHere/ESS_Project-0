using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum currentState
{
    Normal,
    Hover,
    Pressed
}

public class UIState : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    Image image;
    currentState cS = new currentState();
    
    public Sprite normal;
    public Sprite hover;
    public Sprite pressed;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    void Update ()
    {
        switch (cS)
        {
            case currentState.Normal:
                image.sprite = normal;
                break;

            case currentState.Hover:
                image.sprite = hover;
                break;

            case currentState.Pressed:
                image.sprite = pressed;
                break;
        }
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
        cS = currentState.Hover;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cS = currentState.Normal;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        cS = currentState.Pressed;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        cS = currentState.Normal;
    }
}
