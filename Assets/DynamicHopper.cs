using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ChemicalComposition = Chemical.ChemicalComposition;
using ChemicalType = Chemical.ChemicalType;

public class DynamicHopper : HopperController
{
    
    HopperInputDetector inputDetector;
    
    public AudioSource deniedAudio;

    // Start is called before the first frame update
    void Start()
    {
        inputDetector = GetComponentInChildren<HopperInputDetector>();
    }

    public override void PerformAction()
    {
        if (objectPrefab) {
            ChemicalComposition composition = new ChemicalComposition();
            foreach (Collider col in inputDetector.GetColliders()) {
                Chemical chem = Chemical.GetChemicalComponent(col.transform);
                if (chem) {
                    composition.AddRange(chem.composition);
                }
                Destroy(col.gameObject);
            }
            inputDetector.ClearColliders();

            if (composition.Count > 0) {
                GameObject go = Instantiate(objectPrefab, spawnPoint.position, Quaternion.identity);
                go.GetComponent<Chemical>().composition = composition;
            }
            else {
                if (deniedAudio) {
                    deniedAudio.Play();
                }
            }
        }
    }

    public HopperInputDetector GetInputDetector()
    {
        return inputDetector;
    }
}
