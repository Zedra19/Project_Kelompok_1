using UnityEngine;

public class SetStartPosition : MonoBehaviour
{
    public GameObject TargetObject;
    public GameObject TargetPortal;
    public float HeightOffset = 10.0f;

    public EffectManager EffectManager;

    void Start()
    {
        if (TargetObject != null)
        {
            Vector3 Portal = TargetPortal.transform.position;
            Vector3 targetPosition = TargetObject.transform.position;
            transform.position = new Vector3(targetPosition.x, targetPosition.y + HeightOffset, targetPosition.z);
            EffectManager.PlayVFX("Spawn", Portal);
        }
        else
        {
            Debug.LogError("Target object is not assigned!");
        }
    }
}
