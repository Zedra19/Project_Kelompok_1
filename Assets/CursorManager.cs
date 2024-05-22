using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    public Texture2D defaultCursor;
    public Texture2D hoverCursor;
    public Texture2D sliderCursor;
    public Texture2D scrollbarCursor;
    public Vector2 cursorHotspot = Vector2.zero;
    public bool isHovering = false;

    void Start()
    {
        Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.Auto);   

        DetectButton();
        DetectSlider();
    }

    void Update()
    {
        DetectButton();
        DetectSlider();
        if (Input.GetMouseButtonDown(0))
        {
            SetDefaultCursor();
        }
    }

    public void SetDefaultCursor()
    {
        if (!isHovering)
        {
            Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.Auto);
        }
    }

    private void OnButtonClick()
    {
        isHovering = false;
        SetDefaultCursor();
    }

    public void OnHoverEnter()
    {
        isHovering = true;
        Cursor.SetCursor(hoverCursor, cursorHotspot, CursorMode.Auto);
    }

    public void OnHoverExit()
    {
        isHovering = false;
        Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.Auto);
    }

    public void OnSliderEnter()
    {
        isHovering = true;
        Cursor.SetCursor(sliderCursor, cursorHotspot, CursorMode.Auto);
    }

    public void OnSliderClick()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            if (EventSystem.current.currentSelectedGameObject.CompareTag("Slider"))
            {
                isHovering = true;
                Cursor.SetCursor(sliderCursor, cursorHotspot, CursorMode.Auto);
            }
            else
            {
                SetDefaultCursor();
            }
        }
        else
        {
            SetDefaultCursor();
        }
    }


    public void DetectButton()
    {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Button");
        foreach (GameObject button in buttons)
        {
            EventTrigger trigger = button.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = button.AddComponent<EventTrigger>();
            }

            EventTrigger.Entry enterEntry = new EventTrigger.Entry();
            enterEntry.eventID = EventTriggerType.PointerEnter;
            enterEntry.callback.AddListener((eventData) => { OnHoverEnter(); });
            trigger.triggers.Add(enterEntry);

            EventTrigger.Entry exitEntry = new EventTrigger.Entry();
            exitEntry.eventID = EventTriggerType.PointerExit;
            exitEntry.callback.AddListener((eventData) => { OnHoverExit(); });
            trigger.triggers.Add(exitEntry);

            Button btn = button.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(OnButtonClick);
            }
        }
    }

    public void DetectSlider()
    {
        GameObject[] sliders = GameObject.FindGameObjectsWithTag("Slider");
        foreach (GameObject slider in sliders)
        {
            EventTrigger trigger = slider.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = slider.AddComponent<EventTrigger>();
            }

            EventTrigger.Entry enterEntry = new EventTrigger.Entry();
            enterEntry.eventID = EventTriggerType.PointerEnter;
            enterEntry.callback.AddListener((eventData) => { OnSliderEnter(); });
            trigger.triggers.Add(enterEntry);

            EventTrigger.Entry clickEntry = new EventTrigger.Entry();
            clickEntry.eventID = EventTriggerType.PointerDown;
            clickEntry.callback.AddListener((eventData) => { OnSliderClick(); });
            trigger.triggers.Add(clickEntry);

            EventTrigger.Entry exitEntry = new EventTrigger.Entry();
            exitEntry.eventID = EventTriggerType.PointerExit;
            exitEntry.callback.AddListener((eventData) => { OnHoverExit(); });
            trigger.triggers.Add(exitEntry);
        }
    }
}
