using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpOnClick : MonoBehaviour
{
    public GameObject Close;
    public GameObject Close2;
    public GameObject PopUp;
    public GameObject PopUp2;

    private Animator animator;

    void Start()
    {
        animator = PopUp.GetComponent<Animator>();
    }

    public void Click()
    {
        StartCoroutine(Delay());
    }

    public void Click2()
    {
        StartCoroutine(Delay2());
    }

    public void ClosePopUp()
    {
        StartCoroutine(DelayClose());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.5f);
        Close.SetActive(false);
        PopUp.SetActive(true);
    }

    private IEnumerator Delay2()
    {
        yield return new WaitForSeconds(0.5f);
        Close.SetActive(false);
        Close2.SetActive(false);
        PopUp.SetActive(true);
    }

    private IEnumerator DelayClose()
    {
        yield return new WaitForSeconds(0.5f);
        Close.SetActive(false);
        PopUp.SetActive(true);
        PopUp2.SetActive(true);
    }
}
