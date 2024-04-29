using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionSetting : MonoBehaviour
{
    public Transform box;
    public CanvasGroup BG;
    public GameObject Menu;
    public GameObject Setting;

    private void OnEnable()
    {
        BG.alpha = 0;
        BG.LeanAlpha(1, 0.5f);

        box.localPosition = new Vector2(+Screen.width, 0);
        box.LeanMoveLocalX(0, 0.5f).setEaseOutExpo().delay = 0.1f;
    }

    private void Open()
    {
        BG.LeanAlpha(0, 0.5f);
        box.LeanMoveLocalX(+Screen.width, 0.5f).setEaseInExpo().delay = 0.1f;
    }

    public void OnDisable()
    {
        BG.LeanAlpha(0, 0.5f);
        box.LeanMoveLocalX(+Screen.width, 0.5f).setEaseInExpo().delay = 0.1f;
    }

    public void Close()
    {
        StartCoroutine(CloseAnimation());
    }

    private IEnumerator CloseAnimation()
    {
        BG.LeanAlpha(1, 0.5f);
        box.LeanMoveLocalX(0, 0.5f).setEaseOutExpo().delay = 0.1f;

        yield return new WaitForSeconds(0.75f);

        Setting.SetActive(false);
        Menu.SetActive(true);
    }
}

