using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using ChemicalComposition = Chemical.ChemicalComposition;
using ChemicalType = Chemical.ChemicalType;

public class DisplayController : MonoBehaviour
{

    public Transform proxy;
    public float offset;
    public List<GameObject> chemicalPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRequestedComposition(ChemicalComposition composition)
    {
        Debug.Log("Setting composition to");
        composition.ForEach(item => Debug.Log("\t " + item));
        while (proxy.childCount > 0) {
            Transform child = proxy.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }
        float length = composition.Count * offset;

        Vector3 pos = proxy.forward * -length * 0.5f;
        foreach (ChemicalType chem in composition) {
            GameObject prefab = FindChemicalObject(new ChemicalComposition {chem});
            GameObject newDisplay = Instantiate(prefab, proxy.position + pos, Quaternion.identity, proxy);
            newDisplay.GetComponent<Rigidbody>().isKinematic = true;
            pos += proxy.forward * offset;
        }
    }

    GameObject FindChemicalObject(ChemicalComposition composition)
    {
        GameObject go = null;
        Debug.Log("Looking for prefab to");
        composition.ForEach(item => Debug.Log(item));
        for (int i = 0; i < chemicalPrefabs.Count; i++) {
            GameObject prefab = chemicalPrefabs[i];
            ChemicalComposition comp = prefab.GetComponent<Chemical>()?.GetComposition();
            bool equal = (comp.Count == composition.Count) && !comp.Except(composition).Any();
            if (equal) {
                go = prefab;
                break;
            }
            Debug.Log("No match");
        }
        Debug.Log("Using prefab " + go);
        return go;
    }
}
