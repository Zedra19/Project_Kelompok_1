using UnityEngine;

public class PlayerAttackSelector : MonoBehaviour
{
    [SerializeField] private string _playerKsatriaName = "Player-Ksatria Variant(Clone)";
    [SerializeField] private string _playerDukunName = "Player_Dukun(Clone)";

    private void Start()
    {
        string objectName = gameObject.name;
        if (objectName == _playerKsatriaName)
        {
            gameObject.GetComponent<PlayerAttack>();
        }
        else if (objectName == _playerDukunName)
        {
            gameObject.GetComponent<PlayerAttack_Dukun>();
        }
        else
        {
            Debug.LogError("Unknown player type!");
        }
    }
}
