using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartWhenVoid : MonoBehaviour
{
    public Transform startPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.transform.position = startPoint.position;
        }
    }
}
