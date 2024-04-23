using UnityEngine;
using UnityEngine.Events;

public class ShopTrigger : MonoBehaviour
{
    public UnityEvent OnDialogTriggerEnter = new UnityEvent();
    private bool hasBeenCalled = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dialog Trigger") && !hasBeenCalled)
        {
            OnDialogTriggerEnter.Invoke();
            hasBeenCalled = true;
            OnDialogTriggerEnter.RemoveAllListeners();
        }
    }
}
