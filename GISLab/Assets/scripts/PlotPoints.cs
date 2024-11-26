using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotPoints : MonoBehaviour
{
    public ReadCSV db;

    // heatmap prefabs
    public GameObject CapsulePrefab;
    public GameObject parent;

    public Material materialAves;
    public Material materialAmphibia;
    public Material materialReptilia;
    public Material materialMammalia;
    public Material materialActinopterygii;
    public Material materialMollusca;
    public Material materialArachnida;
    public Material materialInsecta;
    public Material materialPlantae;
    public Material materialFungi;
    public Material materialProtozoa;
    public Material materialUnknown;

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
            if (i < 1000)
            {
                Debug.Log("Point id,  " + point["id"]);
                // Map the point to a grid cell
                double x = double.Parse(point["longitude_converted"]);
                double y = double.Parse(point["latitude_converted"]);
                string name = "point" + point["id"];
                string taxon = point["iconic_taxon_name"];

                GameObject caps = Instantiate(CapsulePrefab, new Vector3(0, 0, 0), Quaternion.identity);
                caps.transform.SetParent(parent.transform);
                caps.transform.localPosition = new Vector3((float)x, 0, (float)y);
                caps.name = name;
                Debug.Log("taxon: " + taxon);
   
                // Get the Renderer component from the new cube
                var cubeRenderer = caps.GetComponent<Renderer>();

                switch (taxon)
                {
                    case "Aves":
                        cubeRenderer.material = materialAves;
                        break;
                    case "Amphibia":
                        cubeRenderer.material = materialAmphibia;
                        break;
                    case "Reptilia":
                        cubeRenderer.material = materialReptilia;
                        break;
                    case "Mammalia":
                        cubeRenderer.material = materialMammalia;
                        break;
                    case "Actinopterygii":
                        cubeRenderer.material = materialActinopterygii;
                        break;
                    case "Mollusca":
                        cubeRenderer.material = materialMollusca;
                        break;
                    case "Arachnida":
                        cubeRenderer.material = materialArachnida;
                        break;
                    case "Insecta":
                        cubeRenderer.material = materialInsecta;
                        break;
                    case "Plantae":
                        cubeRenderer.material = materialPlantae;
                        break;
                    case "Fungi":
                        cubeRenderer.material = materialFungi;
                        break;
                    case "Protozoa":
                        cubeRenderer.material = materialProtozoa;
                        break;
                    case "Unknown":
                        cubeRenderer.material = materialUnknown;
                        break;
                    default:
                        break;
                }
            }

        }
    }
    
}
