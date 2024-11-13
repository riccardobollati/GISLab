using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotPoints : MonoBehaviour
{
    public ReadCSV db;

    // heatmap prefabs
    public GameObject CapsulePrefab;
    public GameObject parent;

    // Start is called before the first frame update
    private int firstframe = 0;
    void Start()
    {
        
    }

    void Update()
    {
        if (firstframe == 0)
        {
            iniitAll(db.observationsDataList);
            firstframe = 1;
        }
    }

    public void iniitAll(List<Dictionary<string, string>> observations)
    {
        int i = 0;
        foreach (Dictionary<string, string> point in observations)
        {
            i += 1;
            if (i < 30)
            {
                Debug.Log("Point id,  " + point["id"]);
                // Map the point to a grid cell
                Debug.Log("Hello world");
                double x = double.Parse(point["longitude_converted"]);
                double y = double.Parse(point["latitude_converted"]);

                GameObject caps = Instantiate(CapsulePrefab, new Vector3(0, 0, 0), Quaternion.identity);
                caps.transform.SetParent(parent.transform);
                caps.transform.localPosition = new Vector3((float)x, 0, (float)y);
            }

        }
    }
    
}
