using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MixedReality.Toolkit.UX;

public class confirmButtonHandler : MonoBehaviour
{
    PlotPoints pp;
    PressableButton buttonScript;
    // Start is called before the first frame update
    void Start()
    {
        pp = GameObject.Find("map/Points").GetComponent<PlotPoints>();
        buttonScript = gameObject.GetComponent<PressableButton>();

    }

    // Update is called once per frame
    void Update()
    { 
        buttonScript.enabled = !pp.isPlotting;
    }
}
