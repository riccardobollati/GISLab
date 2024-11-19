using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UIElements;


public class ReadCSV : MonoBehaviour
{
    public List<Dictionary<string, string>> observationsDataList = new List<Dictionary<string, string>>();

    // control point
    public GameObject sphereSW;
    public GameObject sphereSE;
    public GameObject sphereN;



    private Matrix4x4 transformationMatrix;

    void Start()
    {
        ReadCsv("prova_2.csv");
        Dictionary<string, string> prova = getObservation(5);
        Debug.Log(prova["latitude"]);
        Debug.Log(prova["longitude"]);

        Debug.Log(prova["latitude_converted"]);
        Debug.Log(prova["longitude_converted"]);

        transformationMatrix = CalculateTransformationMatrix(sphereSW, sphereSE, sphereN);
    }

    void ReadCsv(string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        Debug.Log(filePath);

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            for (int i = 1; i < lines.Length; i++)
            {

                string[] values = lines[i].Split('§');
                if (values.Length == 67)
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

    public Dictionary<string, string> obsToDictionary(string[] observation)
    {
        Vector2 convertedCoordinates = Wgs84CoordsToUnity(new Vector2(float.Parse(observation[23]), float.Parse(observation[22])));

        return new Dictionary<string, string>
            {
                { "id", observation[0] },
                { "observed_on_string", observation[1] },
                { "observed_on", observation[2] },
                { "time_observed_at", observation[3] },
                { "time_zone", observation[4] },
                { "user_id", observation[5] },
                { "user_login", observation[6] },
                { "user_name", observation[7] },
                { "created_at", observation[8] },
                { "updated_at", observation[9] },
                { "quality_grade", observation[10] },
                { "license", observation[11] },
                { "url", observation[12] },
                { "image_url", observation[13] },
                { "sound_url", observation[14] },
                { "tag_list", observation[15] },
                { "description", observation[16] },
                { "num_identification_agreements", observation[17] },
                { "num_identification_disagreements", observation[18] },
                { "captive_cultivated", observation[19] },
                { "oauth_application_id", observation[20] },
                { "place_guess", observation[21] },
                { "latitude", observation[22] },
                { "longitude", observation[23] },
                { "latitude_converted",  convertedCoordinates.y.ToString()},
                { "longitude_converted",  convertedCoordinates.x.ToString()},
                { "positional_accuracy", observation[24] },
                { "private_place_guess", observation[25] },
                { "private_latitude", observation[26] },
                { "private_longitude", observation[27] },
                { "public_positional_accuracy", observation[28] },
                { "geoprivacy", observation[29] },
                { "taxon_geoprivacy", observation[30] },
                { "coordinates_obscured", observation[31] },
                { "positioning_method", observation[32] },
                { "positioning_device", observation[33] },
                { "place_town_name", observation[34] },
                { "place_county_name", observation[35] },
                { "place_state_name", observation[36] },
                { "place_country_name", observation[37] },
                { "place_admin1_name", observation[38] },
                { "place_admin2_name", observation[39] },
                { "species_guess", observation[40] },
                { "scientific_name", observation[41] },
                { "common_name", observation[42] },
                { "iconic_taxon_name", observation[43] },
                { "taxon_id", observation[44] },
                { "taxon_kingdom_name", observation[45] },
                { "taxon_phylum_name", observation[46] },
                { "taxon_subphylum_name", observation[47] },
                { "taxon_superclass_name", observation[48] },
                { "taxon_class_name", observation[49] },
                { "taxon_subclass_name", observation[50] },
                { "taxon_superorder_name", observation[51] },
                { "taxon_order_name", observation[52] },
                { "taxon_suborder_name", observation[53] },
                { "taxon_superfamily_name", observation[54] },
                { "taxon_family_name", observation[55] },
                { "taxon_subfamily_name", observation[56] },
                { "taxon_supertribe_name", observation[57] },
                { "taxon_tribe_name", observation[58] },
                { "taxon_subtribe_name", observation[59] },
                { "taxon_genus_name", observation[60] },
                { "taxon_genushybrid_name", observation[61] },
                { "taxon_species_name", observation[62] },
                { "taxon_hybrid_name", observation[63] },
                { "taxon_subspecies_name", observation[64] },
                { "taxon_variety_name", observation[65] },
                { "taxon_form_name", observation[66] }
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

        Vector4 wgs84Vector = new Vector4(wgs84Point.x, wgs84Point.y, 1,0);
        Vector4 unityVector = transMat * wgs84Vector;
        Debug.Log(unityVector);
        return new Vector2(unityVector.x, unityVector.y);
    }


    public List<Dictionary<string, string>> GetByAttribute(string property, string value)
    {
        List<Dictionary<string, string>> filtered = new List<Dictionary<string, string>>();
        foreach(Dictionary<string, string> obs in observationsDataList)
        {
            if (obs[property] is string && (string) obs[property] == value)
            {
                filtered.Add(obs);
            }
        }
        return filtered;

    }
    enum Category
    {
        Aves,
        Amphibia,
        Reptilia,
        Mammalia,
        ,
        Mollusca,
        Arachnida,
        Insecta,
        Plantae,
        Fungi,
        Protozoa,
        Unknown
    }
    enum DateType
    {
        Any,
        FixedDate,
        Range,
    }
    public List<Dictionary<string, string>> filter(Category category, date_type, string date)
    {

    }
}
