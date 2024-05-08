using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawnerManager : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private int _playerIndex = 0;
    [SerializeField] private GameObject[] _playerPrefab;
    [SerializeField] private MousePosition3D _mousePosition3D;

    [Header("Portal")]
    [SerializeField] private GameObject _targetObject;
    [SerializeField] private GameObject _targetPortal;
    [SerializeField] private float _heightOffset = 5f;
    [SerializeField] private EffectManager _effectManager;
    private SetStartPosition _setStartPosition;

    [Header("Otehrs")]
    [SerializeField] private InGameDialogue _inGameDialogue;
    [SerializeField] private Image[] _healthIcons;



    // Start is called before the first frame update
    void Start()
    {
        //DEBUGGIN
        SetPlayerIndex(_playerIndex);


        GameObject player = Instantiate(_playerPrefab[StaticPlayer.CurrentPlayerIndex], transform.position, Quaternion.identity);

        //player Setup
        player.GetComponent<PlayerMovement>()._pointer = _mousePosition3D.transform;
        player.GetComponent<Health>().healthIcons = _healthIcons;


        //InGameDialogue Setup
        _inGameDialogue.PlayerAttack = player.GetComponent<PlayerAttack>();
        _inGameDialogue.PlayerMovement = player.GetComponent<PlayerMovement>();

        //SetStartPosition Setup
        _setStartPosition = player.GetComponent<SetStartPosition>();
        _setStartPosition.TargetObject = _targetObject;
        _setStartPosition.TargetPortal = _targetPortal;
        _setStartPosition.HeightOffset = _heightOffset;
        _setStartPosition.EffectManager = _effectManager;

    }

    public void SetPlayerIndex(int index)
    {
        StaticPlayer.CurrentPlayerIndex = index;
    }
}
