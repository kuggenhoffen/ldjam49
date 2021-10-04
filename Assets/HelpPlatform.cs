using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpPlatform : MonoBehaviour
{

    public GameObject helpObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            helpObject.active = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") {
            helpObject.active = false;
        }        
    }
}
