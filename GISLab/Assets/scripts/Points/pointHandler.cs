using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class pointHandler : MonoBehaviour
{
    public GameObject hoveringParticlesPrefab;
    
    private GameObject hoveringParticles;

    public GameObject popUp;

    private Rigidbody rb;

    public float stillThreshold = 0.5f;
    private float stillTime = 0f;      
    private Vector3 lastPosition;
    private bool destroyed = false;


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
        rb = GetComponent<Rigidbody>();
        lastPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (destroyed) return;

        if (Vector3.Distance(transform.position, lastPosition) < 0.001f)
        {
            stillTime += Time.deltaTime;
            if (stillTime >= stillThreshold) 
            {
                Destroy(rb);
                destroyed = true;
            }
        }
        else
            stillTime = 0f;
        lastPosition = transform.position;
    }
}
