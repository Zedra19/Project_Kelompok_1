using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetScrollBar : MonoBehaviour
{
    [SerializeField]
    private Scrollbar scrollbar;

    private void OnEnable()
    {
        ResetScrollbar();
    }

    private void ResetScrollbar()
    {
        if (scrollbar != null)
        {
            scrollbar.value = 1;  // Atur nilai ke 1 untuk posisi atas, atau 0 untuk posisi bawah
        }
    }
}
