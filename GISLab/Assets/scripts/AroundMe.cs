using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AroundMe : MonoBehaviour
{
    public GameObject sphere;
    public float pointScale = 0.01f;
    public GameObject spherePrefab;
    public GameObject parent;

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

    public int pointsMax = 10;
    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Plot(List<Dictionary<string, string>> data)
    {
        Vector3 spherePosition = sphere.transform.localPosition;
        Vector3 sphereScale = sphere.transform.localScale;

        int i = 0;
        foreach (Dictionary<string, string> point in data)
        {
            Vector3 pointPosition = new Vector3(float.Parse(point["longitude_converted"]), 0, float.Parse(point["latitude_converted"]));
            Vector3 postitionTemp = Vector3.Normalize(pointPosition - spherePosition);
            Vector3 positionScaled = new Vector3(postitionTemp.x*sphereScale.x, postitionTemp.y*sphereScale.y, postitionTemp.z*sphereScale.z);
            Vector3 newPointPosition = positionScaled + spherePosition;

            float newPointScale = pointScale/Vector3.Distance(pointPosition,spherePosition);
            i += 1;
            if (i < pointsMax)
            {
                    
                    string name = point["id"];
                    string taxon = point["iconic_taxon_name"];

                    GameObject caps = Instantiate(spherePrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    caps.transform.SetParent(parent.transform);
                    caps.transform.localPosition = newPointPosition;
                    caps.transform.localScale = new Vector3(newPointScale, newPointScale, newPointScale);
                    caps.name = name;

                    var cubeRenderer = caps.GetComponent<Renderer>();
                    //switch (taxon)
                    //{
                    //    case "Aves":
                    //        cubeRenderer.material = materialAves;
                    //        break;
                    //    case "Amphibia":
                    //        cubeRenderer.material = materialAmphibia;
                    //        break;
                    //    case "Reptilia":
                    //        cubeRenderer.material = materialReptilia;
                    //        break;
                    //    case "Mammalia":
                    //        cubeRenderer.material = materialMammalia;
                    //        break;
                    //    case "Actinopterygii":
                    //        cubeRenderer.material = materialActinopterygii;
                    //        break;
                    //    case "Mollusca":
                    //        cubeRenderer.material = materialMollusca;
                    //        break;
                    //    case "Arachnida":
                    //        cubeRenderer.material = materialArachnida;
                    //        break;
                    //    case "Insecta":
                    //        cubeRenderer.material = materialInsecta;
                    //        break;
                    //    case "Plantae":
                    //        cubeRenderer.material = materialPlantae;
                    //        break;
                    //    case "Fungi":
                    //        cubeRenderer.material = materialFungi;
                    //        break;
                    //    case "Protozoa":
                    //        cubeRenderer.material = materialProtozoa;
                    //        break;
                    //    case "Unknown":
                    //        cubeRenderer.material = materialUnknown;
                    //        break;
                    //    default:
                    //        break;
                    //}
                    
                }

            }


        }

    }

