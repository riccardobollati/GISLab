using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MixedReality.Toolkit.UX;

public class heatMap2 : MonoBehaviour
{

    // data
    public ReadCSV db;

    // heatmap prefabs
    public GameObject heatMapPrefab;
    public GameObject heatMapCubes;

    public Slider gran_slider;
    // min and max number of cubes
    public int maxCubes;
    public int minCubes;
    public float gap = 0.01f;
    public GameObject parent;
    

    private int nRows;
    private int nCols;
    private int[,] heatMapData;
    

    private double boxWidth;
    private double boxHeight;

    private GameObject heatMapObj;

    // Start is called before the first frame update
    void Start()
    {
        //initialize the heat map
        int ncubes =(int) (maxCubes + minCubes) / 2;
        nRows = ncubes;
        nCols = ncubes;
        heatMapData = new int[nRows, nCols];
 
        gran_slider.OnValueUpdated.AddListener(setGran);
        Debug.Log(heatMapData);
    }

    void setGran(SliderEventData eventData)
    {
        //int newGran = Mathf.RoundToInt(Mathf.Lerp(minCubes, maxCubes, eventData.NewValue));
        //nRows = newGran;
        //nCols = newGran;
        //populateHeatMap(db.observationsDataList);
        //createMap(0.05);
    }
    public void run()
    {
        //populateHeatMap(db.observationsDataList);
        //PrintHeatMapData(heatMapData);
        //createMap(0.05);
    }

    private static void PrintHeatMapData(int[,] heatMapData)
    {
        int nRows = heatMapData.GetLength(0);
        int nCols = heatMapData.GetLength(1);

        string result = "";

        for (int row = 0; row < nRows; row++)
        {
            for (int col = 0; col < nCols; col++)
            {
                result += heatMapData[row, col] + " ";
            }

            result += "\n";
        }

        Debug.Log(result);
    }

    public void populateHeatMap(List<Dictionary<string, string>> observations)
    {

        double maxLat = 7;//GetMaxCoord(observations, "latitude");
        double minLat = -7; //GetMinCoord(observations, "latitude");

        double maxLng = 7;// GetMaxCoord(observations, "longitude");
        double minLng = -7; //GetMinCoord(observations, "longitude");

        double gridWidth = maxLng - minLng;
        double gridHeight = maxLat - minLat;

        boxWidth = (gridWidth - ((nCols-1)*gap))/nCols;
        boxHeight = (gridHeight- ((nRows-1)*gap))/nRows;



        foreach (Dictionary<string, string> point in observations)
        {
            // Map the point to a grid cell
            int col = (int)((double.Parse(point["longitude_converted"]) - minLng) / boxWidth);
            int row = (int)((double.Parse(point["latitude_converted"]) - minLat) / boxHeight);

            // Ensure the point is within the bounds of the grid
            if (col >= 0 && col < nCols && row >= 0 && row < nRows)
            {
                heatMapData[row, col]++;
            }
        }
    }

    private static double GetMaxCoord(List<Dictionary<string, string>> observations, string coord)
    {
        return observations
            .Where(obs => obs.ContainsKey(coord) && double.TryParse(obs[coord], out _))
            .Max(obs => double.Parse(obs[coord]));
    }
    private static double GetMinCoord(List<Dictionary<string, string>> observations, string coord)
    {
        return observations
            .Where(obs => obs.ContainsKey(coord) && double.TryParse(obs[coord], out _))
            .Min(obs => double.Parse(obs[coord]));
    }

    public void createMap(double baseline)
    {
        if (heatMapObj != null) {
            Destroy(heatMapObj);
        }
        heatMapObj = Instantiate(heatMapPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        heatMapObj.transform.SetParent(parent.transform);
        heatMapPrefabScript heatmapPrefabScript = heatMapObj.GetComponent<heatMapPrefabScript>();
        PrintHeatMapData(heatMapData);


        //initialize the heat map
        heatmapPrefabScript.Initialize(
            heatMapData,
            heatMapCubes,
            boxWidth,
            boxHeight,
            nRows,
            nCols
            );

    }

    // Update is called once per frame
    void Update()
    {

    }

}
