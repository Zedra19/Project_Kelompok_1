using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideHistory : MonoBehaviour
{
    [SerializeField]private Scrollbar scrollbar;

    [SerializeField]
    [Range(0.01f, 1f)]
    private float sensitivity = 0.1f;

    private void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            float scrollAmount = Input.mouseScrollDelta.y * sensitivity;
            scrollbar.value = Mathf.Clamp01(scrollbar.value + scrollAmount);
        }
    }
}
