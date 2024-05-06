using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnerTutorial : MonoBehaviour
{

    [SerializeField] private GameObject _sSwarmerPrefab;
    [SerializeField] private GameObject _mSwarmerPrefab;
    [SerializeField] private GameObject _lSwarmerPrefab;
    [SerializeField] private GameObject _bossPrefab;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private Rigidbody _rigidbodyPlayer;
    [SerializeField] private Animator _animatorPlayer;
    [SerializeField] private PlayerAttack _playerAttack;
    private bool DialogActivated = false;
    public GameObject Prolog;
    public GameObject Dialog3;
    public int totalEnemyCount;
    public int totalTargetEnemy;
    public KillCount KillCount;
    public float spawnInterval;
    public bool isEndlessMode; // Tambahkan opsi mode endless

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private void Update()
    {
        OnEnemyKilled();
    }

    public IEnumerator SpawnEnemies()
    {
        while (totalEnemyCount > 0 || isEndlessMode) // Tambahkan kondisi isEndlessMode
        {
            yield return new WaitForSeconds(spawnInterval);

            if (totalEnemyCount % 3 == 0)
            {
                Instantiate(_sSwarmerPrefab, GetRandomPosition(), Quaternion.identity);
            }
            else if (totalEnemyCount % 3 == 1)
            {
                Instantiate(_mSwarmerPrefab, GetRandomPosition(), Quaternion.identity);
            }
            else
            {
                Instantiate(_lSwarmerPrefab, GetRandomPosition(), Quaternion.identity);
            }

            totalEnemyCount--;

            if (totalEnemyCount == 0 && !isEndlessMode) // Hentikan loop jika totalEnemyCount mencapai 0 dan bukan endless mode
            {
                Debug.Log("All enemies spawned");
                Instantiate(_bossPrefab, GetRandomPosition(), Quaternion.identity);
                yield break; // Hentikan coroutine
            }
        }
    }

    public void OnEnemyKilled()
    {
        if (KillCount.killCount == totalTargetEnemy && !DialogActivated)
        {
            _playerAttack.enabled = false;
            _animatorPlayer.SetBool("Move", false);
            _playerMovement.enabled = false;
            Prolog.SetActive(true);
            Dialog3.SetActive(true);
            DialogActivated = true;
        }
    }


    private Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-8.0f, 8.0f), 0.75f, Random.Range(-6.0f, 6.0f));
    }
}
