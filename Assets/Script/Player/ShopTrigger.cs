using UnityEngine;
using UnityEngine.Events;

public class ShopTrigger : MonoBehaviour
{
    public UnityEvent OnDialogTriggerEnter = new UnityEvent();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dialog Trigger"))
        {
            OnDialogTriggerEnter.Invoke();

            //OnDialogTriggerEnter.RemoveAllListeners();
        }
    }
}
