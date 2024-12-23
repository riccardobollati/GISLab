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

    public bool isPlotting = false;


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
    private bool stop = false;
    public void Destroy()
    {
        stop = true;
        foreach (string pointId in displayed)
        {
            Destroy(pointsMap[pointId]);
        }
    }

    public void plot(List<Dictionary<string, string>> data)
    {
        Debug.Log("creating points...");
        Debug.Log("points to scan: " + data.Count);
        isPlotting = true;

        SortDataByLatitude(ref data);

        // get list of points we have to plot
        foreach (Dictionary<string, string> point in data)
        {
            newDisplayed.Add(point["id"]);
        }

        if (data.Count > pointsMax)
        {
            Debug.Log("creating random");
            float percentage = (float)pointsMax / data.Count;
            Debug.Log(percentage);
            List<Dictionary<string, string>> randomSubset = GetRandomSubset(data, percentage);
            StartCoroutine(PlotPointsWithDelay(randomSubset));
        }
        else
            StartCoroutine(PlotPointsWithDelay(data));

    }

    static List<Dictionary<string, string>> GetRandomSubset(List<Dictionary<string, string>> data, float percentage)
    {
        System.Random random = new System.Random();
        int count = (int)(data.Count * percentage);
        return data.OrderBy(x => random.Next()).Take(count).ToList();
    }

    private IEnumerator PlotPointsWithDelay(List<Dictionary<string, string>> data)
    {

        Debug.Log("plotting :" + data.Count + " points");
        // delete points we don't want
        foreach (KeyValuePair<string, GameObject> kvp in pointsMap)
        {
            if (!newDisplayed.Contains(kvp.Key))
                Destroy(pointsMap[kvp.Key]);
            displayed.Remove(kvp.Key);
        }

        // render new points
        foreach (Dictionary<string, string> point in data)
        {

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

                // Wait for before instantiating the next point to create the waive effect
                yield return new WaitForSeconds(0.002f);

            }
        }

        // reset list of points to display
        newDisplayed = new HashSet<string> { };
        Debug.Log("Points rendering complete.");
        isPlotting = false;

    }
    public void SortDataByLatitude(ref List<Dictionary<string, string>> data)
{
    data = data.OrderBy(dict => double.Parse(dict["latitude_converted"])).ToList();
}
}
