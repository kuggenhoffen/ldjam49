using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chemical : MonoBehaviour
{

    public enum ChemicalType {
        NotVeryDangerous,
        SomewhatVolatile,
        ExtremelyVolatile,
        Unknown
    }

    public class ChemicalComposition : List<ChemicalType> {}

    public ChemicalComposition GetComposition() 
    {
        ChemicalComposition comp = new ChemicalComposition();
        comp.AddRange(composition);
        return comp;
    }

    public List<ChemicalType> composition;

    public static Chemical GetChemicalComponent(Transform obj)
    {
        Chemical chem = obj.GetComponent<Chemical>();
        if (!chem) {
            chem = obj.GetComponentInParent<Chemical>();
        }
        return chem;
    }

}

