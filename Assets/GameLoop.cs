using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

using ChemicalComposition = Chemical.ChemicalComposition;
using ChemicalType = Chemical.ChemicalType;

public class GameLoop : MonoBehaviour
{
    const int MAX_DIFFICULTY = 7;
    public DisplayController displayController;
    public DynamicHopper outputHopper;
    public List<GameObject> chemicalPrefabs;
    ChemicalComposition compositionRequired = new ChemicalComposition();
    int score = 0;
    int resourcesLeft = 50;

    public AudioSource scoreAudio;
    public AudioSource deniedAudio;

    public TMP_Text uiText;
    public GameObject player;
    public GameObject endScreen;

    // Start is called before the first frame update
    void Start()
    {
        displayController.chemicalPrefabs = chemicalPrefabs;
    }

    // Update is called once per frame
    void Update()
    {
        uiText.SetText("Score: {0}\tResources left: {1}", score, resourcesLeft);

        if (Input.GetButton("Cancel")) {
            Application.Quit();
        }
    }

    public void RequestWork()
    {
        Debug.Log("Requested work");

        if (resourcesLeft <= 0) {
            player.active = false;

            // Hide all ui elements first
            for (int i = 0; i < endScreen.transform.parent.childCount; i++) {
                endScreen.transform.parent.GetChild(i).gameObject.active = false;
            }
            endScreen.active = true;
            TMP_Text textObject = endScreen.GetComponentInChildren<TMP_Text>();
            textObject.text = textObject.text.Replace("{score}", score.ToString());

            uiText.gameObject.active = false;
            Debug.Log("too bad, you lose");
        }

        compositionRequired.Clear();
        int maxChems = Mathf.Min(score / 5, MAX_DIFFICULTY);
        int count = Random.Range(2, 3 + maxChems);
        int chemCount = Chemical.ChemicalType.GetValues(typeof(ChemicalType)).Length;
        for (int i = 0; i < count; i++) {
            ChemicalType chem = (ChemicalType)Random.Range(0,  chemCount - 1);
            compositionRequired.Add(chem);
        }
        displayController.SetRequestedComposition(compositionRequired);
    }

    public void SubmitWork()
    {
        Debug.Log("Submitted work");
        bool compositionMatched = false;

        foreach (Collider col in outputHopper.GetInputDetector().GetColliders()) {
            Chemical chem = Chemical.GetChemicalComponent(col.transform);
            ChemicalComposition comp = chem.GetComposition();
            if (compositionRequired.Count > 0) {
                bool equal = (compositionRequired.Count == comp.Count) && !comp.Except(compositionRequired).Any();
                if (equal) {
                    int addScore = 0;
                    comp.ForEach(item => addScore += (int)item);
                    score += addScore;
                    resourcesLeft += addScore;
                    Debug.Log("Matching composition received");
                    compositionMatched = true;
                }
            }
            Destroy(chem.gameObject);
        }
        outputHopper.GetInputDetector().ClearColliders();

        if (compositionMatched) {
            compositionRequired.Clear();
            displayController.SetRequestedComposition(compositionRequired);
//            scoreAudio.Play();
        }
        else {
            deniedAudio.Play();
        }

        Debug.Log("Score is now " + score);
    }

    ChemicalType GetChemicalType(GameObject go)
    {
        return go.GetComponent<Chemical>().composition[0];
    }

    public void ConsumeChemicals(ChemicalComposition composition)
    {
        foreach(ChemicalType ct in composition) {
            Debug.Log("Consuming chemicals " + ct);
            resourcesLeft -= (int)ct;
        }
    }
}
