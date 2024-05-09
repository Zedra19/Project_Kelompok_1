using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SpawnPrajurit : MonoBehaviour
{
    public Transform _pointer; // Referensi ke transformasi pemain

    void Update()
    {
        // Pastikan ada referensi ke transformasi pemain
        if (_pointer != null)
        {
        transform.LookAt(_pointer);        
        }
    }
}

