using MixedReality.Toolkit.UX;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class PlotHeatMap : MonoBehaviour
{

    
    // heatmap prefabs
    public GameObject cubePrefab;
    // Parent Plane
    public GameObject parent_plane;

    public float gap;
    public int maxCubes;
    public int minCubes;
    public double tall;



    // PRIVATE

    private int nRows;
    private int nCols;
    private int[,] heatMapData;
    public float baseline;

    private double boxWidth;
    private double boxHeight;

    private double maxLat = 7;//GetMaxCoord(observations, "latitude");
    private double minLat = -7; //GetMinCoord(observations, "latitude");

    private double maxLng = 7;// GetMaxCoord(observations, "longitude");
    private double minLng = -7; //GetMinCoord(observations, "longitude");



    private float origin_x = -7;
    private float origin_z = -7;


    // assets
    private GameObject[,] heatMapCubes;


    void Start()
    {
        //initialize the heat map
        int ncubes = (int)(maxCubes + minCubes) / 2;
        nRows = ncubes;
        nCols = ncubes;
        heatMapData = new int[nRows, nCols];

    }

 
    

    public void plot(List<Dictionary<string, string>> data, float granularity)
    {
        populateHeatMap(data, granularity);
        createMap();
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

    private void populateHeatMap(List<Dictionary<string, string>> data, float granularity)
    {
        int newGran = Mathf.RoundToInt(Mathf.Lerp(minCubes, maxCubes, granularity));
        nRows = newGran;
        nCols = newGran;
        heatMapData = new int[nRows, nCols];

        double gridWidth = maxLng - minLng;
        double gridHeight = maxLat - minLat;

        double gridWidx = gridWidth / nCols;
        double gridHidx = gridHeight / nRows;

        boxWidth = (gridWidth - ((nCols - 1) * gap)) / nCols;
        boxHeight = (gridHeight - ((nRows - 1) * gap)) / nRows;



        foreach (Dictionary<string, string> point in data)
        {
            // Map the point to a grid cell
            //int col = (int)((double.Parse(point["longitude_converted"]) - minLng) / gridWidx);
            //int row = (int)((double.Parse(point["latitude_converted"]) - minLat) / gridHidx);
            int col = (int)(((double.Parse(point["longitude_converted"]) - minLng) / gridWidth) * nCols);
            int row = (int)(((double.Parse(point["latitude_converted"]) - minLat) / gridHeight) * nRows);

            // Ensure the point is within the bounds of the grid
            if (col >= 0 && col < nCols && row >= 0 && row < nRows)
            {
                heatMapData[row, col]++;
            }
        }
    }

    public void Destroy()
    {
        if (heatMapCubes != null)
        {
            foreach (GameObject cube in heatMapCubes)
                Destroy(cube);
        }
    }

    private void createMap()
    {
        if (heatMapCubes != null)
        {
            foreach (GameObject cube in heatMapCubes)
                Destroy(cube);
        }
        heatMapCubes = new GameObject[nRows, nCols];
        //double max = GetMaxFrom2DArray(heatMapData);
            //if (max == 0)
            //{
            //    max = 1;
            //}
        //if (boxWidth > boxHeight)
        //{
        //    boxHeight = boxHeight / boxWidth;
        //    boxWidth = 1;
        //}
        //else
        //{
        //    boxWidth = boxWidth / boxHeight;
        //    boxHeight = 1;
        //}

        Vector3 cubePosition = new Vector3((float)(origin_x+(boxWidth/2)), 0.5f, (float)(origin_z + (boxHeight / 2)));

        for (int row = 0; row < nRows; row++)
        {
            for (int col = 0; col < nCols; col++)
            {
                float height = (float)heatMapData[row, col]; /// (float)std;
                heatMapCubes[row, col] = Instantiate(cubePrefab, cubePosition, Quaternion.identity);
                heatMapCubes[row, col].transform.parent = parent_plane.transform;
                heatMapCubes[row, col].transform.localScale = new Vector3((float)boxWidth, (float)(baseline + height * tall), (float)boxHeight);
                Vector3 newPosition = cubePosition;
                newPosition.y = (float)(baseline + height * tall)/2;

                heatMapCubes[row, col].transform.position = newPosition;
                cubePosition = new Vector3(cubePosition.x + (float)boxWidth + gap, cubePosition.y, cubePosition.z);

                // change cube color based on the density of the region
                Renderer renderer = heatMapCubes[row, col].GetComponent<Renderer>();
                Color currentColor = renderer.material.color;
                currentColor.a = 0.5f;
                renderer.material.color = currentColor;
            }
            cubePosition = new Vector3((float)(origin_x + (boxWidth / 2)), cubePosition.y, cubePosition.z + (float)boxHeight + gap);
        }

        //transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

  

    private static double GetMaxFrom2DArray(int[,] array)
    {
        double max = 0;


        for (int row = 0; row < array.GetLength(0); row++)
        {
            for (int col = 0; col < array.GetLength(1); col++)
            {
                if (max <=  (double)array[row, col])
                {
                    max = (double)array[row, col];
                }

            }
        }

        return max;
    }
}