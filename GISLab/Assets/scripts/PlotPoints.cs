using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotPoints : MonoBehaviour
{

    // heatmap prefabs
    public GameObject CapsulePrefab;
    public GameObject parent;

    // this parameter controll the max number of points that will be rendered
    public int pointsMax = 1000;

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

    int firstFrame = 0;




    

    // to keep track of rendered points
    HashSet<string> displayed = new HashSet<string>();
    HashSet<string> newDisplayed = new HashSet<string>();
    Dictionary<string, GameObject> pointsMap = new();

    public void Destroy()
    {
        foreach (string pointId in displayed)
        {
            Destroy(pointsMap[pointId]);
        }
    }
    public void plot(List<Dictionary<string, string>> data)
    {
        Debug.Log("creating points...");
        Debug.Log("points to scan: " + data.Count);
        
        int i = 0;
        newDisplayed = new HashSet<string>();
        foreach (Dictionary<string, string> point in data)
        {
            i += 1;
            if (i < pointsMax)
            {
                newDisplayed.Add(point["id"]);
                // if the point is not rendered yet
                if (!displayed.Contains(point["id"]))
                {
                    // Map the point to a grid cell
                    double x = double.Parse(point["longitude_converted"]);
                    double y = double.Parse(point["latitude_converted"]);
                    string name = "point" + point["id"];
                    string taxon = point["iconic_taxon_name"];

                    GameObject caps = Instantiate(CapsulePrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    caps.transform.SetParent(parent.transform);
                    caps.transform.localPosition = new Vector3((float)x, 0, (float)y);
                    caps.name = name;
                    caps.layer = 7;

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
                    displayed.Add(point["id"]);
                    pointsMap[point["id"]] = caps;
                }

            }


        }
        Debug.Log(displayed.Count);
        displayed.ExceptWith(newDisplayed);
        Debug.Log(displayed.Count);
        foreach (string pointId in displayed)
        {
            Destroy(pointsMap[pointId]);
        }
        displayed = newDisplayed;

        StartCoroutine(DisableGravity());



    }
    IEnumerator DisableGravity()
    {
        // Wait for 1 seconds
        yield return new WaitForSeconds(1.5f);

        // Now call the function after the wait
        foreach(KeyValuePair<string, GameObject> point in pointsMap)
        {
            GameObject go = point.Value;

            // delete rigid body
            Rigidbody rb = go.GetComponent<Rigidbody>();
            if (rb != null)
                Destroy(rb);
        }
    }

}
