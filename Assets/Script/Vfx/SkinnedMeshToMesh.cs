using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SkinnedMeshToMesh : MonoBehaviour
{
    // Start is called before the first frame update
    public SkinnedMeshRenderer skinnedMesh;
    public VisualEffect VFXGraph;
    public float refreshRate;
    void Start()
    {
        StartCoroutine (UpdateVFXGraph());
    }

    IEnumerator UpdateVFXGraph()
    {
        while (gameObject.activeSelf)
        {
            Mesh m = new Mesh();
            skinnedMesh.BakeMesh(m);
            VFXGraph.SetMesh("Mesh", m);

            yield return new WaitForSeconds(refreshRate);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
