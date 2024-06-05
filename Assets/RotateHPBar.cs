using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHPBar : MonoBehaviour
{
    private Transform Cam;

    // Start is called before the first frame update
    void Start()
    {
        Cam = FindObjectOfType<Camera>().transform; // Cari objek kamera saat runtime
    }

    // Update is called once per frame
    void Update()
    {
        if (Cam != null)
        {
            transform.LookAt(Cam);
        }
    }
}
