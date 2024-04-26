using UnityEngine;

public class PlayerAttackSelector : MonoBehaviour
{
    private void Start()
    {
        string objectName = gameObject.name;
        if (objectName == "Player-Ksatria")
        {
            gameObject.GetComponent<PlayerAttack>();
        }
        else if (objectName == "Player_Dukun")
        {
            gameObject.GetComponent<PlayerAttack_Dukun>();
        }
        else
        {
            Debug.LogError("Unknown player type!");
        }
    }
}
