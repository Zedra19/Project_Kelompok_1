using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillCount : MonoBehaviour
{
    public int killCount = 0;
    public int killCountForBossSpawn = 5;

    [SerializeField] private Slider bossHealthSlider; // Reference to the UI Slider
    [SerializeField] private Text bossName; // Reference to the Boss Name
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //activate boss health here
        /*
        if (killCount >= killCountForBossSpawn)
        {
            bossHealthSlider.gameObject.SetActive(true);
            bossName.gameObject.SetActive(true);
        }
        */
    }
}
