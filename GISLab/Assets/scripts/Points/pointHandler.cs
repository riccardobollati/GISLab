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
        // istantiate the popUp only if it doesn't exists already
        if(GameObject.Find(gameObject.name + "_popUp") == null)
        {
            Vector3 spawnPosition = transform.position + new Vector3(0, 0.5f, 0);
            popUp = Instantiate(popUp, spawnPosition, Quaternion.identity);
            PopUpManager popUpMen = popUp.GetComponent<PopUpManager>();
            popUpMen.dbObj = GameObject.Find("DB");
            popUpMen.db = popUpMen.dbObj.GetComponent<ReadCSV>();
            Debug.Log("[pointHandler] Getting data for observation: " + gameObject.name);
            popUpMen.PopulatePopUp(gameObject.name);
        }


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
