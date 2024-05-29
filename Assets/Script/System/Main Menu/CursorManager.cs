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
    private bool isDraggingSlider = false;
    private bool isDraggingScrollbar = false;

    void Start()
    {
        Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.Auto);

        DetectButton();
        DetectSlider();
        DetectScrollbar();
    }

    void Update()
    {
        DetectButton();
        DetectSlider();
        DetectScrollbar();

        if (Input.GetMouseButtonDown(0))
        {
            SetDefaultCursor();
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDraggingSlider = false;
            isDraggingScrollbar = false;
            SetDefaultCursor();
        }

        DetectScrollInput();
    }

    public void SetDefaultCursor()
    {
        if (!isHovering && !isDraggingSlider && !isDraggingScrollbar)
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
        SetDefaultCursor();
    }

    public void OnSliderEnter()
    {
        isHovering = true;
        Cursor.SetCursor(sliderCursor, cursorHotspot, CursorMode.Auto);
    }

    public void OnSliderClick()
    {
        isDraggingSlider = true;
        Cursor.SetCursor(sliderCursor, cursorHotspot, CursorMode.Auto);
    }

    public void OnScrollbarEnter()
    {
        isHovering = true;
        Cursor.SetCursor(scrollbarCursor, cursorHotspot, CursorMode.Auto);
    }

    public void OnScrollbarClick()
    {
        isDraggingScrollbar = true;
        Cursor.SetCursor(scrollbarCursor, cursorHotspot, CursorMode.Auto);
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

    public void DetectScrollbar()
    {
        GameObject[] scrollbars = GameObject.FindGameObjectsWithTag("Scrollbar");
        foreach (GameObject scrollbar in scrollbars)
        {
            EventTrigger trigger = scrollbar.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = scrollbar.AddComponent<EventTrigger>();
            }

            EventTrigger.Entry enterEntry = new EventTrigger.Entry();
            enterEntry.eventID = EventTriggerType.PointerEnter;
            enterEntry.callback.AddListener((eventData) => { OnScrollbarEnter(); });
            trigger.triggers.Add(enterEntry);

            EventTrigger.Entry clickEntry = new EventTrigger.Entry();
            clickEntry.eventID = EventTriggerType.PointerDown;
            clickEntry.callback.AddListener((eventData) => { OnScrollbarClick(); });
            trigger.triggers.Add(clickEntry);

            EventTrigger.Entry exitEntry = new EventTrigger.Entry();
            exitEntry.eventID = EventTriggerType.PointerExit;
            exitEntry.callback.AddListener((eventData) => { OnHoverExit(); });
            trigger.triggers.Add(exitEntry);
        }
    }

    public void DetectScrollInput()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, raycastResults);

            foreach (RaycastResult result in raycastResults)
            {
                if (result.gameObject.CompareTag("Scrollbar"))
                {
                    OnScrollbarEnter();
                    return;
                }
            }

            SetDefaultCursor();
        }
    }
}
