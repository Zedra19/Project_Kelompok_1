using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EffectSceneManager;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerSpawnerManager : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private int _playerIndex = 0;
    [SerializeField] private GameObject[] _playerPrefab;
    [SerializeField] private MousePosition3D _mousePosition3D;
    [SerializeField] private Image[] _healthIcons;
    [SerializeField] private Image _staminaBar;

    [Header("Portal")]
    [SerializeField] private GameObject _targetObject;
    [SerializeField] private GameObject _targetPortal;
    [SerializeField] private float _heightOffset = 5f;
    [SerializeField] private EffectManager _effectManager;

    [HideInInspector] public InGameDialogue InGameDialogue;

    [HideInInspector] public ShopPopUp ShopPopUp;
    public enum CurrentScene
    {
        game,
        shop
    }
    [SerializeField] private CurrentScene _currentScene;

    private SetStartPosition _setStartPosition;
    private GameObject _player;
    private Vector3 _lastPlayerPosition;

#if UNITY_EDITOR

    [CustomEditor(typeof(PlayerSpawnerManager))]
    public class PlayerSpawnerManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            PlayerSpawnerManager playerSpawnerManager = (PlayerSpawnerManager)target;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Needed Component based on Scene", EditorStyles.boldLabel);

            DrawDetailPerScene(playerSpawnerManager);

        }

        private static void DrawDetailPerScene(PlayerSpawnerManager playerSpawnerManager)
        {
            if (playerSpawnerManager._currentScene == PlayerSpawnerManager.CurrentScene.game)
            {
                //ini udah cukup
                playerSpawnerManager.InGameDialogue = (InGameDialogue)EditorGUILayout.ObjectField("InGameDialogue", playerSpawnerManager.InGameDialogue, typeof(InGameDialogue), true);

                //improved
                // EditorGUILayout.BeginToggleGroup("In-Game Dialogue", playerSpawnerManager.InGameDialogue != null);
                // playerSpawnerManager.InGameDialogue = (InGameDialogue)EditorGUILayout.ObjectField("In-Game Dialogue", playerSpawnerManager.InGameDialogue, typeof(InGameDialogue), true);
                // EditorGUILayout.EndToggleGroup();
            }
            if (playerSpawnerManager._currentScene == PlayerSpawnerManager.CurrentScene.shop)
            {
                //ini udah cukup
                playerSpawnerManager.ShopPopUp = (ShopPopUp)EditorGUILayout.ObjectField("ShopPopUp", playerSpawnerManager.ShopPopUp, typeof(ShopPopUp), true);

                //improved
                // EditorGUILayout.BeginToggleGroup("Shop PopUp", playerSpawnerManager.ShopPopUp != null);
                // playerSpawnerManager.ShopPopUp = (ShopPopUp)EditorGUILayout.ObjectField("Shop PopUp", playerSpawnerManager.ShopPopUp, typeof(ShopPopUp), true);
                // EditorGUILayout.EndToggleGroup();
            }
            if (GUI.changed)
                EditorUtility.SetDirty(playerSpawnerManager);
        }
    }

#endif

    // Start is called before the first frame update
    void Start()
    {
        AssetDatabase.SaveAssets();
        SetPlayerIndex(_playerIndex);

        _player = Instantiate(_playerPrefab[StaticPlayer.CurrentPlayerIndex], transform.position, Quaternion.identity);

        SetupPlayer(_player);

    }

    private void SetupPlayer(GameObject player)
    {
        //player Setup
        player.GetComponent<PlayerMovement>()._pointer = _mousePosition3D.transform;
        player.GetComponent<Health>().healthIcons = _healthIcons;
        player.GetComponent<Stamina>().StaminaBar = _staminaBar;

        if (_currentScene == CurrentScene.game)
        {
            //InGameDialogue Setup
            InGameDialogue.PlayerAttack = player.GetComponent<PlayerAttack>();
            InGameDialogue.PlayerMovement = player.GetComponent<PlayerMovement>();
        }

        if (_currentScene == CurrentScene.shop)
        {
            player.AddComponent<ShopTrigger>();
            ShopPopUp.PlayerMovement_I = player.GetComponent<PlayerMovement>();
            ShopPopUp.PlayerAttack_I = player.GetComponent<PlayerAttack>();
        }

        //SetStartPosition Setup
        _setStartPosition = player.GetComponent<SetStartPosition>();
        _setStartPosition.TargetObject = _targetObject;
        _setStartPosition.TargetPortal = _targetPortal;
        _setStartPosition.HeightOffset = _heightOffset;
        _setStartPosition.EffectManager = _effectManager;
    }

    public void UpdatePlayer(int index)
    {
        SetPlayerIndex(index);

        _lastPlayerPosition = _player.transform.position;
        Destroy(_player);

        _player = Instantiate(_playerPrefab[StaticPlayer.CurrentPlayerIndex], _lastPlayerPosition, Quaternion.identity);
        SetupPlayer(_player);
        ShopPopUp.CloseAllDialogs();
    }

    public void SetPlayerIndex(int index)
    {
        StaticPlayer.CurrentPlayerIndex = index;
    }
}
