using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using Unity.VisualScripting;
using MixedReality.Toolkit.UX;
using System.Security.Cryptography;
using Unity.XR.CoreUtils;
using UnityEngine.InputSystem.LowLevel;
public class ReadCSV : MonoBehaviour
{

    
    private List<Dictionary<string, string>> observationsDataList = new(); // All data
    private List<Dictionary<string, string>> observationsFiltered = new(); // Current filtered data

    // Filters Parameters
    private HashSet<string> filterCategory = new(); // Current categories
    private bool points = true;                     // Current Point mode : points or heatmap
    private int dateMode = 0;                          //0 = any date, 1 = fixed date, 2 = range date
    private DateTime? exactDate = null;       // Current Date
    private DateTime?[] dateRange = new DateTime?[2];// Current Date Range
    private float granularity = 1;



    // control point
    public GameObject sphereSW;
    public GameObject sphereSE;
    public GameObject sphereN;
    public PlotPoints pointsScript;
    public PlotHeatMap heatMapScript;
    public AroundMe arountMeScript;
    public InfoPanel infoPanel;
    public GameObject zurichAppObject;
    public AMPosition AMPosition;
    public AMrotation AMRotation;
    public GameObject zurichMap;





    private Matrix4x4 transformationMatrix;

    private bool lockAroundMePosition = false;
    private bool lockAroundMeRotation = false;

    private Vector3 capsPosition;
    private Vector3 gameMapPosition;
    private Vector3 userPosition;
    private Quaternion origineRotation;
    private Quaternion userRotation;
    private Vector3 orPos;
    private Quaternion orRot;
    private Vector3 orScale;

    public XROrigin xrOrigin;
    void Start()
    {
        // read data from the csv database
        ReadCsv("data.csv");

        // at the beginnnig we take all the observations
        observationsFiltered = observationsDataList;



        // compute transformation matrix for coordinates
        //transformationMatrix = CalculateTransformationMatrix(sphereSW, sphereSE, sphereN);

        // initialize filter params
        // filterCategory = new HashSet<string> { "Plantae", "Arachnida", "Insecta", "Fungi", "Mollusca", "Aves",
        //"Actinopterygii", "Reptilia", "Mammalia", "Amphibia", "Protozoa", "Animalia", "Chromista" };

        // filterDataRange = new List<DateTime> { new DateTime(2020, 07, 15), new DateTime(2024, 07, 15) };
        dateRange[0] = null;
        dateRange[1] = null;

        
        if (xrOrigin == null)
        {
            Debug.LogError("XROrigin not found! Ensure your MRTK scene is properly set up.");
        }
    } 

    void ReadCsv(string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        Debug.Log($"File path: {filePath}");

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            for (int i = 1; i < lines.Length; i++)
            {

                string[] values = lines[i].Split('§');
                if (values.Length == 39) //67
                {
                    observationsDataList.Add(obsToDictionary(values));
                }
            }

            Debug.Log($"CSV Data Loaded: {observationsDataList.Count} elemts loaded");
        }
        else
        {
            Debug.LogError($"File not found: {filePath}");
        }
    }

    public Dictionary<string, string> getObservation(int index)
    {
        return observationsDataList[index];
    }

    public Dictionary<string, string> getObservationByID(string id)
    {
        foreach(Dictionary<string, string> obs in observationsFiltered)
        {
            if(obs["id"] == id)
                return obs;       
        }

        return null;
    }

    public Dictionary<string, string> obsToDictionary(string[] observation)
    {
        Vector2 convertedCoordinates = Wgs84CoordsToUnity(new Vector2(float.Parse(observation[23]), float.Parse(observation[22])));

        return new Dictionary<string, string>
        {
            { "id", observation[0]},
            { "observed_on_string", observation[1]},
            { "observed_on", observation[2]},
            { "time_observed_at", observation[3]},
            { "time_zone", observation[4]},
            { "user_id", observation[5]},
            { "user_login", observation[6]},
            { "user_name", observation[7]},
            { "created_at", observation[8]},
            { "updated_at", observation[9]},
            { "quality_grade", observation[10]},
            { "license", observation[11]},
            { "url", observation[12]},
            { "image_url", observation[13]},
            { "sound_url", observation[14]},
            { "tag_list", observation[15]},
            { "description", observation[16]},
            { "num_identification_agreements", observation[17]},
            { "num_identification_disagreements", observation[18]},
            { "captive_cultivated", observation[19]},
            { "oauth_application_id", observation[20]},
            { "place_guess", observation[21]},
            { "latitude", observation[22]},
            { "longitude", observation[23]},
            { "latitude_converted",  convertedCoordinates.y.ToString()},
            { "longitude_converted",  convertedCoordinates.x.ToString()},
            { "positional_accuracy", observation[24]},
            { "private_place_guess", observation[25]},
            { "private_latitude", observation[26]},
            { "private_longitude", observation[27]},
            { "public_positional_accuracy", observation[28]},
            { "geoprivacy", observation[29]},
            { "taxon_geoprivacy", observation[30]},
            { "coordinates_obscured", observation[31]},
            { "positioning_method", observation[32]},
            { "positioning_device", observation[33]},
            { "species_guess", observation[34]},
            { "scientific_name", observation[35]},
            { "common_name", observation[36]},
            { "iconic_taxon_name", observation[37]},
            { "taxon_id", observation[38]}
        };


    }

    public Matrix4x4 CalculateTransformationMatrix(GameObject sphereSW, GameObject sphereSE, GameObject sphereN)
    {

        Vector2 wgs84PointSW = new Vector2(47.320222f, 8.503180f);
        Vector2 wgs84PointSE = new Vector2(47.354664f, 8.625453f);
        Vector2 wgs84PointN = new Vector2(47.434685f, 8.502075f);

        Vector2 unityPointSW = new Vector2(sphereSW.transform.position.x, sphereSW.transform.position.y);
        Vector2 unityPointSE = new Vector2(sphereSW.transform.position.x, sphereSW.transform.position.y);
        Vector2 unityPointN = new Vector2(sphereSW.transform.position.x, sphereSW.transform.position.y);

        Matrix4x4 matrixP = Matrix4x4.identity;
        matrixP.SetRow(0, new Vector4(wgs84PointSW.x, wgs84PointSW.y, 1, 0));
        matrixP.SetRow(1, new Vector4(wgs84PointSE.x, wgs84PointSE.y, 1, 0));
        matrixP.SetRow(2, new Vector4(wgs84PointN.x, wgs84PointN.y, 1, 0));

        Matrix4x4 matrixQ = Matrix4x4.identity;
        matrixQ.SetRow(0, new Vector4(unityPointSW.x, unityPointSW.y, 1, 0));
        matrixQ.SetRow(1, new Vector4(unityPointSE.x, unityPointSE.y, 1, 0));
        matrixQ.SetRow(2, new Vector4(unityPointN.x, unityPointN.y, 1, 0));

        Matrix4x4 transformation = matrixP.inverse * matrixQ;

        return transformation;
    }

    public Vector2 Wgs84CoordsToUnity(Vector2 wgs84Point)
    {
        //Vector4 wgs84Vector = new Vector4(wgs84Point.x, wgs84Point.y, 1, 0);
        //Vector4 unityVector = transformationMatrix * wgs84Vector;

        //return new Vector2(unityVector.x, unityVector.y);

        Matrix4x4 transMat = Matrix4x4.identity;
        transMat.SetRow(0, new Vector4(-86.14170713f, 19.24295351f, -176.65586266f, 0));
        transMat.SetRow(1, new Vector4(-19.24295351f, -86.14170713f, 4244.80097565f, 0));

        Vector4 wgs84Vector = new Vector4(wgs84Point.x, wgs84Point.y, 1, 0);
        Vector4 unityVector = transMat * wgs84Vector;
        return new Vector2(unityVector.x, unityVector.y);
    }

    public List<Dictionary<string, string>> GetByAttribute(string property, string value)
    {
        List<Dictionary<string, string>> filtered = new List<Dictionary<string, string>>();
        foreach (Dictionary<string, string> obs in observationsDataList)
        {
            if (obs[property] is string && (string)obs[property] == value)
            {
                filtered.Add(obs);
            }
        }
        return filtered;

    }

    public void FilterData()
    {
        observationsFiltered = observationsDataList;
        foreach (string c in filterCategory)
            Debug.Log(c);

        if (dateMode == 0) // Any date
        {
            observationsFiltered = observationsFiltered.Where(obs => {
              return filterCategory.Contains(obs["iconic_taxon_name"]);
               }).ToList();
        }
        else if (dateMode == 1) // Fixed Date
        {
                observationsFiltered = observationsFiltered.Where(obs => {
                    return filterCategory.Contains(obs["iconic_taxon_name"]) && DateTime.ParseExact(obs["observed_on"], "yyyy-mm-dd", System.Globalization.CultureInfo.InvariantCulture).Date == exactDate;
                }).ToList();
        }
        else if (dateMode == 2) // Range date
        {
            observationsFiltered = observationsFiltered.Where(obs => {
                return filterCategory.Contains(obs["iconic_taxon_name"]) && DateTime.ParseExact(obs["observed_on"], "yyyy-mm-dd",System.Globalization.CultureInfo.InvariantCulture).Date >= dateRange[0] 
                && DateTime.ParseExact(obs["observed_on"], "yyyy-mm-dd", System.Globalization.CultureInfo.InvariantCulture).Date <= dateRange[1];
            }).ToList();
        } else // Error
        {
            Debug.Log("Incorrect date mode.");
        }
        if (filterCategory.Count < 13)
            observationsFiltered = observationsFiltered.Where(obs => {
                return filterCategory.Contains(obs["iconic_taxon_name"]);
            }).ToList();
        //// filter date
        //if(filterExactDate != null)
        //    observationsFiltered = observationsFiltered.Where(obs => DateTime.Parse(obs["observed_on"]) == filterExactDate).ToList();
        //else if(filterDataRange != null && filterDataRange.Count > 1)
        //    observationsFiltered = observationsFiltered.Where(obs => (
        //    DateTime.Parse(obs["observed_on"]) >= filterDataRange[0] && 
        //    DateTime.Parse(obs["observed_on"]) <= filterDataRange[1]
        //    )).ToList();

        Debug.Log($"data filtered, results: {observationsFiltered.Count}");
    }


    public void SetPointMode(bool pointMode)
    {
        points = pointMode;
        if (pointMode)
        {
            infoPanel.WriteNewLine("Point mode");
        } else
        {
            infoPanel.WriteNewLine("Heatmap mode");
        }
    }

    public void SetAroundMeMode()
    {
        points = true;
        lockAroundMePosition = true;
        heatMapScript.Destroy();
        pointsScript.Destroy();
        infoPanel.WriteNewLine("Around Me mode");
        infoPanel.WriteNewLine("Please, place the blue beacon where you are");
        infoPanel.WriteNewLine("to indicte your current location. Then press 'Validate'.");
        AMPosition.Show(true);
    }

    public void exitArounMeMode()
    {

        infoPanel.WriteNewLine("Exited Around Me mode");
        AMRotation.Show(false);
        AMPosition.Show(false);
        pointsScript.Destroy();
        heatMapScript.Destroy();

        zurichAppObject.transform.position = orPos;
        zurichAppObject.transform.rotation = orRot;
        zurichAppObject.transform.localScale = orScale;
        zurichMap.SetActive(true);
        lockAroundMePosition = false;
        lockAroundMeRotation = false;
    }

    private void AroundMeRotation()
    {
        infoPanel.WriteNewLine("Please, now rotate the compas to point north");
        infoPanel.WriteNewLine("Then press 'Validate'.");
        origineRotation = AMRotation.getRotation();
        AMRotation.Show(true);
        
        zurichMap.SetActive(false);
    }
    public void AddFixedDate(String newDate)
    {
        dateMode = 1;
        if (checkDate(newDate))
        {
            exactDate = DateTime.ParseExact(newDate, "yyyy-mm-dd", System.Globalization.CultureInfo.InvariantCulture).Date;
            infoPanel.WriteNewLine($"Exact date: {((DateTime)exactDate).ToString("yyyy-MM-dd")}");
        }

    }

    public void AddRangeDateFrom(String newDate)
    {
        dateMode = 2;
        if (checkDate(newDate))
        {
            dateRange[0] = DateTime.ParseExact(newDate, "yyyy-mm-dd", System.Globalization.CultureInfo.InvariantCulture).Date;
            if (dateRange[1] != null) { infoPanel.WriteNewLine($"Date range: from {((DateTime)dateRange[0]).ToString("yyyy-MM-dd")} to {((DateTime)dateRange[1]).ToString("yyyy-MM-dd")}"); }
            else
            {
                infoPanel.WriteNewLine($"Date range: from {((DateTime)dateRange[0]).ToString("yyyy-MM-dd")} to ?");
            }
        }
    }

    public void AddRangeDateTo(String newDate)
    {
        dateMode = 2;
        if (checkDate(newDate))
        {
            dateRange[1] = DateTime.ParseExact(newDate, "yyyy-mm-dd", System.Globalization.CultureInfo.InvariantCulture).Date;
            if (dateRange[0] != null) {infoPanel.WriteNewLine($"Date range: from {((DateTime)dateRange[0]).ToString("yyyy-MM-dd")} to {((DateTime)dateRange[1]).ToString("yyyy-MM-dd")}"); }
            else
            {
                infoPanel.WriteNewLine($"Date range: from ? to {((DateTime)dateRange[1]).ToString("yyyy-MM-dd")}");
            }
        }

    }

    public void SetAnyDate()
    {
        dateMode = 0;
        infoPanel.WriteNewLine("Any date selected");
    }

    public void AddCategory(string category)
    {
        filterCategory.Add(category);
        infoPanel.WriteNewLine($"Category Added: {category}");
    }

    public void RemoveCategory(string category)
    {
        filterCategory.Remove(category);
        infoPanel.WriteNewLine($"Category removed: {category}");
    }

    public void setGranularity(SliderEventData eventData)
    {
        granularity = eventData.NewValue;
        infoPanel.WriteNewLine($"Granularity changed: {eventData.NewValue}");

    }

    public void Confirm()
    {
        if (lockAroundMePosition)
        {
            capsPosition = AMPosition.getPosition();
            userPosition = xrOrigin.Camera.transform.position;
            gameMapPosition = zurichAppObject.transform.position;
   
            lockAroundMePosition = false;
            lockAroundMeRotation = true;
            AMPosition.Show(false);
            AroundMeRotation();

            return;
        }
        if (lockAroundMeRotation)
        {
             userRotation = AMRotation.getRotation();

            

            lockAroundMeRotation = false;
            AMRotation.Show(false);
            infoPanel.WriteNewLine("You can now apply the filter normally.");
            //hide zurich
            orPos = zurichAppObject.transform.position;
            orRot = zurichAppObject.transform.rotation;
            orScale = zurichAppObject.transform.localScale;
            zurichAppObject.transform.position = userPosition - capsPosition;
            zurichAppObject.transform.rotation = Quaternion.Inverse(origineRotation) * userRotation;
            zurichAppObject.transform.localScale= 10 * Vector3.one;
            return;
        }
        if ((dateMode == 1 && exactDate == null) || (dateMode == 2 && dateRange[0] == null) || (dateMode == 2 && dateRange[1] == null))
        {
            infoPanel.WriteNewLine("Incorrect date format.");
            return;
        }
        if (dateMode == 2 && dateRange[1] < dateRange[0])
        {
            infoPanel.WriteNewLine("To date smaller than from date.");
            return;
        }
        
        FilterData();
        if (observationsFiltered.Count == 0)
        {
            infoPanel.WriteNewLine("No datapoints found for this filter.");

        } else if (points)
        {
            
            heatMapScript.Destroy();
            pointsScript.plot(observationsFiltered);
            infoPanel.WriteNewLine("Points plotted.");

        } else
        {
            pointsScript.Destroy();
            heatMapScript.plot(observationsFiltered, granularity);
            infoPanel.WriteNewLine("HeatMap plotted.");

        }
    }

    public bool checkDate(string date)
    {
        var flag = true;
        try
        {
            DateTime.ParseExact(date,
                "yyyy-mm-dd",
                System.Globalization.CultureInfo.InvariantCulture);

        }
        catch
        {
            flag = false;
        }

        if (flag == false)
        {
            infoPanel.WriteNewLine("Incorrect date format.");
            return false;
        }
        else
        {
            return true;

        }

    }

    public void plotShpere()
    {
        arountMeScript.Plot(observationsFiltered);
    }
    
}