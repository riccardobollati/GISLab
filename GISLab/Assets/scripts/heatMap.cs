using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class heatMap : MonoBehaviour
{

    public ReadCSV db;

    public static int nRows = 10;
    public static int nCols = 10;
    public int[,] heatMapData = new int[nRows, nCols];

    // Start is called before the first frame update
    void Start()
    {
        //initialize the heat map
        for (int row = 0; row < nRows; row++)
        {
            for (int col = 0; col < nCols; col++)
            {
                heatMapData[row, col] = 0;
            }
        }

        Debug.Log(heatMapData);
    }

    public void run()
    {
        populateHeatMap(db.observationsDataList);
        PrintHeatMapData(heatMapData);
    }

    public static void PrintHeatMapData(int[,] heatMapData)
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

        double maxLat = GetMaxCoord(observations, "latitude");
        double minLat = GetMaxCoord(observations, "latitude");

        double maxLng = GetMaxCoord(observations, "longitude");
        double minLng = GetMaxCoord(observations, "longitude");

        double gridWidth = maxLng - minLng;
        double gridHeight = maxLat - minLat;

        double boxWidth = gridWidth / nCols;
        double boxHeight = gridHeight / nRows;

        Debug.Log(maxLat);

        foreach (Dictionary<string, string> point in observations)
        {
            // Map the point to a grid cell
            int col = (int)(double.Parse(point["longitude"]) - minLng / boxWidth);
            int row = (int)(double.Parse(point["latitude"]) - minLat/ boxHeight);

            // Ensure the point is within the bounds of the grid
            if (col >= 0 && col < nCols && row >= 0 && row < nRows)
            {
                heatMapData[row, col]++;
            }
        }
    }

    private static double GetMaxCoord(List<Dictionary<string, string>> observations, string coord)
    {
        Debug.Log(observations.Count);
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
