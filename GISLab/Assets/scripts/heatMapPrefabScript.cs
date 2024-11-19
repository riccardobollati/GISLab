using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heatMapPrefabScript : MonoBehaviour
{
    public GameObject cubePrefab;
    public double boxWidth;
    public double boxHeight;
    public int nRows;
    public int nCols;
    public float gap = 0.01f;
    public float baseline = 0.2f;
    public int[,] heatMapData;
    public float origin_x = -7;
    public float origin_z = -7;


    // assets
    public GameObject[,] heatMapCubes;
    public Material cubesMaterial;


    public void Initialize(
            int[,] HeatMapData,
            GameObject CubePrefab,
            double BoxWidth,
            double BoxHeight,
            int NRows,
            int NCols
        )
    {
        heatMapData = HeatMapData;
        cubePrefab = CubePrefab;
        boxWidth = BoxWidth;
        boxHeight = BoxHeight;
        nRows = NRows;
        nCols = NCols;

        InitializeHeatMap();
    }

    // Start is called before the first frame update
    void InitializeHeatMap()
    {
        // initialize the heatmap cubes
        heatMapCubes = new GameObject[nRows, nCols];

        int max = GetMaxFrom2DArray(heatMapData);

        if (boxWidth > boxHeight)
        {
            boxHeight = boxHeight / boxWidth;
            boxWidth = 1;
        } else 
        {
            boxWidth = boxWidth / boxHeight;
            boxHeight = 1;
        }

        Vector3 cubePosition = new Vector3(origin_x, 0, origin_z);

        for(int row = 0; row < nRows; row++)
        {
            for(int col = 0; col < nCols; col++)
            {
                float height = (float)heatMapData[row, col] / (float)max;
                heatMapCubes[row, col] = Instantiate(cubePrefab, cubePosition, Quaternion.identity);
                heatMapCubes[row, col].transform.parent = transform;
                heatMapCubes[row, col].transform.localScale = new Vector3((float)boxWidth, baseline + height*2f, (float)boxHeight);
                Vector3 newPosition = cubePosition;
                newPosition.y = (baseline + height * 2f) / 2f;
                heatMapCubes[row, col].transform.position = newPosition;
                cubePosition = new Vector3(cubePosition.x + (float)boxWidth + gap, cubePosition.y, cubePosition.z);

                // change cube color based on the density of the region
                Renderer renderer = heatMapCubes[row, col].GetComponent<Renderer>();
                Color currentColor = renderer.material.color;
                currentColor.a =  height*170/255;
                renderer.material.color = currentColor;

            }
            cubePosition = new Vector3(origin_x, cubePosition.y , cubePosition.z + (float)boxHeight + gap);
        }

        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

    }
    public static int GetMaxFrom2DArray(int[,] array)
    {
        int max = int.MinValue;

        for (int row = 0; row < array.GetLength(0); row++)
        {
            for (int col = 0; col < array.GetLength(1); col++)
            {
                if (array[row, col] > max)
                {
                    max = array[row, col];
                }
            }
        }

        return max;
    }


    void Start()
    { 
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
