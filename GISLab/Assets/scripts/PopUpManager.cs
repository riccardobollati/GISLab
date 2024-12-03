using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpManager : MonoBehaviour
{
    public GameObject dbObj;
    public ReadCSV db;

    TMP_Text commonName;
    TMP_Text scientificName;
    TMP_Text iconicTaxonName;
    TMP_Text description;
    TMP_Text idText;
    TMP_Text timeObservedAt;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void PopulatePopUp(string id)
    {

        // get text component
        Debug.Log(transform.Find("content/Info/common_name"));
        commonName = transform.Find("content/Info/common_name").GetComponent<TMP_Text>();
        Debug.Log(commonName);
        scientificName = transform.Find("content/Info/scientific_name").GetComponent<TMP_Text>();
        iconicTaxonName = transform.Find("content/Info/iconic_taxon_name").GetComponent<TMP_Text>();
        description = transform.Find("content/Info/description").GetComponent<TMP_Text>();
        idText = transform.Find("content/Info/id").GetComponent<TMP_Text>();
        timeObservedAt = transform.Find("content/time_observed_at").GetComponent<TMP_Text>();

        // get observation
        Dictionary<string, string> observation = db.getObservationByID(id);

        // set texts
        commonName.text = observation["common_name"];
        scientificName.text = observation["scientific_name"];
        iconicTaxonName.text = observation["iconic_taxon_name"];
        description.text = observation["description"];
        idText.text = observation["id"];
        timeObservedAt.text = observation["time_observed_at"];
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180f, 0);

    }
}
