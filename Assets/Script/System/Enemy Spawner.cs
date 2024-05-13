using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _sSwarmerPrefab;
    [SerializeField] private GameObject _mSwarmerPrefab;
    [SerializeField] private GameObject _lSwarmerPrefab;
    [SerializeField] private GameObject _bossPrefab;

    public int totalEnemyCount;
    public int spawnThreshold;
    public float spawnInterval;
    public bool isEndlessMode;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    public IEnumerator SpawnEnemies()
    {
        while (totalEnemyCount > 0 || isEndlessMode)
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

            if (isEndlessMode)
            {

                if (totalEnemyCount == 0)
                {
                    Instantiate(_bossPrefab, GetRandomPosition(), Quaternion.identity);
                    totalEnemyCount = spawnThreshold;
                }
            }

            if (totalEnemyCount == 0 && !isEndlessMode)
            {
                Debug.Log("All enemies spawned");
                Instantiate(_bossPrefab, GetRandomPosition(), Quaternion.identity);
                yield break;
            }
        }
    }

    private Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-8.0f, 8.0f), 0.75f, Random.Range(-6.0f, 6.0f));
    }
}
