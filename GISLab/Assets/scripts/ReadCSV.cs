using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ReadCSV : MonoBehaviour
{
    public List<string[]> observationsDataList = new List<string[]>();

    void Start()
    {
        ReadCsv("observations.csv");
        Debug.Log(GetByAttribute(4, "Paris"));
    }

    void ReadCsv(string fileName)
    {
        string filePath = Path.Combine(Application.dataPath, "Data", fileName);
        Debug.Log(filePath);

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            for (int i = 1; i < lines.Length; i++) // Skip header line
            {
                string[] values = lines[i].Split(',');
                if (values.Length >= 2)
                {
                    observationsDataList.Add(values); // This works now with string[]
                }
            }

            Debug.Log("CSV Data Loaded:");
        }
        else
        {
            Debug.LogError($"File not found: {filePath}");
        }
    }

    public Dictionary<string, string> GetObservation(int index)
    {
        string[] observation = observationsDataList[index];
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

    public List<string[]> GetByAttribute(int property, string value)
    {
        List<string[]> filtered = new List<string[]>();
        foreach(string[] obs in observationsDataList)
        {
            Debug.Log(obs);
            foreach(string s in obs)
            {
                Debug.Log(s);
            }
            if(obs[property] == value)
            {
                filtered.Add(obs);
            }
        }
        return filtered;

    }
}
