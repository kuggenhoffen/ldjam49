using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HopperController : IActivatable
{
    public bool infinite;
    public GameObject objectPrefab;
    public Transform spawnPoint;
    public GameLoop gameLoop;

    // Start is called before the first frame update
    void Start()
    {
    }

    public override void PerformAction()
    {
        if (objectPrefab) {
            Instantiate(objectPrefab, spawnPoint.position, Quaternion.identity);
            gameLoop.ConsumeChemicals(objectPrefab.GetComponent<Chemical>().GetComposition());
        }
        else {
            
        }
    }
}
