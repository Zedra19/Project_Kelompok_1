using System.Collections;
using UnityEngine;

public class SpearEnemyM : MonoBehaviour
{
    public GameObject enemyM;
    EnemyM enemyMScript;
    public GameObject Trail;

    private bool isThrown = false;

    void Start()
    {
        enemyMScript = enemyM.GetComponent<EnemyM>();
    }

    void Update()
    {
        if (gameObject.tag != "Spear")
        {
            transform.position = gameObject.transform.parent.position;
            transform.eulerAngles = gameObject.transform.parent.eulerAngles + new Vector3(0, 90f, 0);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (gameObject.tag == "Spear" && collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }

    public void ThrowSpear()
    {
        isThrown = true;
        Trail.SetActive(true);
        StartCoroutine(DestroyAfterDelay(1f));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (isThrown)
        {
            Destroy(gameObject);
        }
    }
}
