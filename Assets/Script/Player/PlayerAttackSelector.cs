using UnityEngine;

public class PlayerAttackSelector : MonoBehaviour
{
    [SerializeField] private string _playerKsatriaName = "Player-Ksatria Variant(Clone)";
    [SerializeField] private string _playerDukunName = "Player_Dukun(Clone)";
    [SerializeField] private string _playerPrajuritName = "Player_Prajurit(Clone)";
    [SerializeField] private string _playerPetaniName = "Player_Petani(Clone)";

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
        else if (objectName == _playerPrajuritName)
        {
            gameObject.GetComponent<PlayerAttack_Prajurit>();
        }
        else if (objectName == _playerPetaniName)
        {
            gameObject.GetComponent<PlayerAttack_Petani>();
        }
        else
        {
            Debug.LogError("Unknown player type!");
        }
    }
}
