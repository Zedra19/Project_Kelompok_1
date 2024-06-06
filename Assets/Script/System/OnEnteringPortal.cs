using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnEnteringPortal : MonoBehaviour
{
    Score _score; //Don;t know if neccesary or not
    [SerializeField] private string _sceneName = "Env";
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _score = FindAnyObjectByType<Score>();
            StaticScore.currentScore = _score.currentScore;
            AudioManager.Instance.StopMusic("Shop");
            SceneManager.LoadScene(_sceneName);
        }
    }

    public string SceneName
    {
        get { return _sceneName; }
        set { _sceneName = value; }
    }
}
