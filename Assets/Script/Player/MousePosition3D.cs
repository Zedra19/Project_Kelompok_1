using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition3D : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;

    private void Update()
    {
        // Mencari kamera utama yang aktif
        Camera mainCamera = Camera.main;

        // Memastikan kamera utama telah ditemukan
        if (mainCamera != null)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red, 1f);
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _layerMask))
            {
                transform.position = hit.point;
            }
        }
        else
        {
            Debug.LogError("Tidak ada kamera utama yang ditemukan.");
        }
    }
}
