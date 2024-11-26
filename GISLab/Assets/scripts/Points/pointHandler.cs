using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointHandler : MonoBehaviour
{
    public GameObject hoveringParticlesPrefab;
    
    private GameObject hoveringParticles;

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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
