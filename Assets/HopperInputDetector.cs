using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HopperInputDetector : MonoBehaviour
{

    HashSet<Collider> colliders = new HashSet<Collider>();


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
        colliders.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        colliders.Remove(other);
    }

    public HashSet<Collider> GetColliders() 
    {
        return colliders; 
    }

    public void ClearColliders()
    {
        colliders.Clear();
    }
}
