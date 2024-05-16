using UnityEngine;
using UnityEngine.Events;

public class ShopTrigger : MonoBehaviour
{
    public UnityEvent OnDialogTriggerEnter = new UnityEvent();
    public UnityEvent OnDialogTriggerExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dialog Trigger"))
        {
            OnDialogTriggerEnter.Invoke();

            //OnDialogTriggerEnter.RemoveAllListeners();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Dialog Trigger"))
        {
            OnDialogTriggerExit?.Invoke(); // if null skip
        }
    }

    private void Awake()
    {
        ShopPopUp shopPopUp = FindObjectOfType<ShopPopUp>();
        if (shopPopUp != null)
        {
            StartCoroutine(shopPopUp.SetupShopTrigger());
        }
    }
}
