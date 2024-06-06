using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SceneUpdatorManager : MonoBehaviour
{

    private void OnEnable()
    {
        PatternPatih.OnPatihDestroyed += LevelSuccessWithDuration;
        Senopati.OnSenopatiDestroyed += LevelSuccessWithDuration;
    }

    private void OnDisable()
    {
        PatternPatih.OnPatihDestroyed -= LevelSuccessWithDuration;
        Senopati.OnSenopatiDestroyed -= LevelSuccessWithDuration;
    }

    [SerializeField] private float _waitAfterCharacterDestroyed = 5f;
    [SerializeField] private string _shopSceneName = "Shop";
    [SerializeField] private string _level1SceneName = "Env";
    [HideInInspector] public OnEnteringPortal OnEnteringPortal;
    // [HideInInspector] public PlayerSpawnerManager PlayerSpawnerManager;
    [HideInInspector] public string InGameNextSceneName;
    public enum CurrentScene
    {
        game,
        shop
    }
    [SerializeField] private CurrentScene _currentScene;

#if UNITY_EDITOR

    [CustomEditor(typeof(SceneUpdatorManager))]
    public class SceneUpdatorManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SceneUpdatorManager SceneUpdatorManager = (SceneUpdatorManager)target;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Needed Values based on Scene", EditorStyles.boldLabel);

            DrawDetailPerScene(SceneUpdatorManager);
        }

        private static void DrawDetailPerScene(SceneUpdatorManager SceneUpdatorManager)
        {
            if (SceneUpdatorManager._currentScene == SceneUpdatorManager.CurrentScene.game)
            {
                SceneUpdatorManager.InGameNextSceneName = EditorGUILayout.TextField("In Game Next Scene Name", SceneUpdatorManager.InGameNextSceneName);
            }
            if (SceneUpdatorManager._currentScene == SceneUpdatorManager.CurrentScene.shop)
            {
                SceneUpdatorManager.OnEnteringPortal = (OnEnteringPortal)EditorGUILayout.ObjectField("On Entering Portal", SceneUpdatorManager.OnEnteringPortal, typeof(OnEnteringPortal), true);
                // SceneUpdatorManager.PlayerSpawnerManager = (PlayerSpawnerManager)EditorGUILayout.ObjectField("Player Spawner Manager", SceneUpdatorManager.PlayerSpawnerManager, typeof(PlayerSpawnerManager), true);
            }
            if (GUI.changed)
                EditorUtility.SetDirty(SceneUpdatorManager);
        }
    }

#endif

    void Start()
    {
        if (_currentScene == CurrentScene.shop)
        {
            if (PlayerPrefs.GetInt("isLevelSuccess") == 1)
            {
                // PlayerSpawnerManager.SpawnPlayer();
                OnEnteringPortal.SceneName = PlayerPrefs.GetString("nextScene");
                PlayerPrefs.SetInt("isLevelSuccess", 0);
            }
            if (PlayerPrefs.GetString("nextScene") == _level1SceneName)
            {
                OnEnteringPortal.SceneName = _level1SceneName;
            }
        }
        if (_currentScene == CurrentScene.game)
        {
            //setting up shop in level 1 if lost
            if (SceneManager.GetActiveScene().name == _level1SceneName)
            {
                PlayerPrefs.SetString("nextScene", _level1SceneName);
            }
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            LevelSuccessNextScene();
        }
    }

    private void LevelSuccessWithDuration()
    {
        Invoke(nameof(LevelSuccessNextScene), _waitAfterCharacterDestroyed);
    }

    /// <summary>
    /// call this function in game scene, not need in shop scene
    /// </summary>
    public void LevelSuccessNextScene()
    {
        // Time.timeScale = 1;
        PlayerPrefs.SetInt("isLevelSuccess", 1);
        PlayerPrefs.SetString("nextScene", InGameNextSceneName);
        SceneManager.LoadScene(_shopSceneName);
    }
}
