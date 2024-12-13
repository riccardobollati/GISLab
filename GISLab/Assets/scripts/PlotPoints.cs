using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlotPoints : MonoBehaviour
{

    // heatmap prefabs
    public GameObject CapsulePrefab;
    public GameObject parent;

    // this parameter controll the max number of points that will be rendered
    public int pointsMax = 1000;
    public double displayRate = 0.1;


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

        SortDataByLatitude(ref data);
        // plot points 
        StartCoroutine(PlotPointsWithDelay(data));
    }

    private IEnumerator PlotPointsWithDelay(List<Dictionary<string, string>> data)
    {
        foreach (Dictionary<string, string> point in data)
        {
            System.Random random = new System.Random();
            int randomInt = random.Next(0, 100);
            if (randomInt <= 100 * displayRate)
            {

                newDisplayed.Add(point["id"]);
                // If the point is not rendered yet
                if (!displayed.Contains(point["id"]))
                {
                    // Map the point to a grid cell
                    double x = double.Parse(point["longitude_converted"]);
                    double y = double.Parse(point["latitude_converted"]);
                    string name = point["id"];
                    string taxon = point["iconic_taxon_name"];

                    GameObject caps = Instantiate(CapsulePrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    caps.transform.SetParent(parent.transform);
                    caps.transform.localPosition = new Vector3((float)x, 0.5f, (float)y);
                    caps.transform.localScale = parent.transform.localScale.x * CapsulePrefab.transform.localScale;
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

                    // Wait for 0.5 seconds before instantiating the next point
                    yield return new WaitForSeconds(0.0002f);


                }
                Debug.Log("Displayed point: ");
            }
        }

        // Update displayed points
        displayed.ExceptWith(newDisplayed);
        foreach (string pointId in displayed)
        {
            Destroy(pointsMap[pointId]);
        }
        displayed = newDisplayed;

        Debug.Log("Points rendering complete.");
    }
    public void SortDataByLatitude(ref List<Dictionary<string, string>> data)
{
    data = data.OrderBy(dict => double.Parse(dict["latitude_converted"])).ToList();
}
}
