using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class popup : MonoBehaviour
{
    public ReadCSV db;

    // heatmap prefabs
    // public GameObject selectedPoint;

    public TextMeshProUGUI[] textFields;


    string pointId = "304284";
    string[] newText;

    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        updatePopup();

    }

 

    public void updatePopup()
    {
        Debug.Log("popup");

        foreach (Dictionary<string, string> point in db.observationsFiltered)
        {
            // getting information about selected point
            if (point["id"] == pointId)
            {
              
                  
                newText[0] = point["common_name"];
                newText[1] = "( " + point["scientific_name"] + " )";
                newText[2] = point["iconic_taxon_name"];
                newText[3] = point["id"];
                newText[4] = point["time_observed_at"];
                newText[5] = point["description"];
                newText[6] = point["place_guess"] + "( " + point["longitude"] + ", " +point["latitude"] + " )";
               
           
                Debug.Log("point " + point["id"]);

                for (int i = 0; i < textFields.Length; i++)
                {
                    textFields[i].text = newText[i];
                }

            }


        }
      
    }


}
