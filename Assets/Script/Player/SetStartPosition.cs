using UnityEngine;

public class SetStartPosition : MonoBehaviour
{
    public GameObject TargetObject;
    public float HeightOffset = 10.0f;

    void Start()
    {
        if (TargetObject != null)
        {
            Vector3 targetPosition = TargetObject.transform.position;
            transform.position = new Vector3(targetPosition.x, targetPosition.y + HeightOffset, targetPosition.z);
        }
        else
        {
            Debug.LogError("Target object is not assigned!");
        }
    }
}
