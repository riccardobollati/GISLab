using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointHandler : MonoBehaviour
{
    public GameObject hoveringParticlesPrefab;
    
    private GameObject hoveringParticles;

    public GameObject popUp;

    public void HoverEntered()
    {
        Debug.Log("entered hovering------------------------");
        hoveringParticles = Instantiate(hoveringParticlesPrefab, transform);
    }
    
    public void HoverExited()
    {
        Debug.Log("exited hovering------------------------");
        DestroyImmediate(hoveringParticles, true);
    }

    public void OnCLicked()
    {
        Vector3 spawnPosition = transform.position + new Vector3(0, 1, 0);
        popUp = Instantiate(popUp, spawnPosition, Quaternion.identity);
        PopUpManager popUpMen = popUp.GetComponent<PopUpManager>();
        popUpMen.dbObj = GameObject.Find("DB");  // or however you find the db object
        popUpMen.db = popUpMen.dbObj.GetComponent<ReadCSV>();
        //popUpMen.PopulatePopUp(int.Parse(transform.name));
        popUpMen.PopulatePopUp(10);


    }
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
