using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnEnteringPortal : MonoBehaviour
{
    [SerializeField] private string _sceneName = "Env";
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.StopMusic("Shop");
            SceneManager.LoadScene(_sceneName);
        }
    }
}
