using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.IO;

public class PopUpManager : MonoBehaviour
{
    public GameObject dbObj;
    public ReadCSV db;

    TMP_Text commonName;
    TMP_Text scientificName;
    TMP_Text iconicTaxonName;
    TMP_Text description;
    TMP_Text idText;
    TMP_Text timeObservedAt;
    Renderer ImageRenderer;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void PopulatePopUp(string id)
    {
        gameObject.name = id + "_popUp";
        // get text component
        commonName = transform.Find("content/Info/common_name").GetComponent<TMP_Text>();
        scientificName = transform.Find("content/Info/scientific_name").GetComponent<TMP_Text>();
        iconicTaxonName = transform.Find("content/Info/iconic_taxon_name").GetComponent<TMP_Text>();
        description = transform.Find("content/Info/description").GetComponent<TMP_Text>();
        idText = transform.Find("content/Info/id").GetComponent<TMP_Text>();
        timeObservedAt = transform.Find("content/time_observed_at").GetComponent<TMP_Text>();
        ImageRenderer = transform.Find("content/image").GetComponent<Renderer>();


        // get observation
        Dictionary<string, string> observation = db.getObservationByID(id);

        // set texts
        commonName.text = observation["common_name"];
        scientificName.text = observation["scientific_name"];
        iconicTaxonName.text = observation["iconic_taxon_name"];
        if(observation["description"] == "")
            description.text = "No description";
        else
            description.text = observation["description"];
            
        idText.text = observation["id"];
        if(observation["time_observed_at"] == "")
            timeObservedAt.text = "No date found";
        else
            timeObservedAt.text = observation["time_observed_at"];

        // get image
        // using network (not working on hololens)
        //StartCoroutine(DownloadAndSetImage(observation["image_url"]));

        LoadAndSetImage(observation["id"]);
    }

    public void ClosePopUp() {
        Destroy(gameObject);
    }

    private IEnumerator DownloadAndSetImage(string imageUrl)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            Debug.Log("Getting image: " + imageUrl);

            // Send the request and wait for the download to complete
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Get the downloaded texture
                Texture2D downloadedTexture = DownloadHandlerTexture.GetContent(request);

                // Apply the texture to the material
                ImageRenderer.material.SetTexture("_MainTex", downloadedTexture);
                Debug.Log("Texture successfully applied from URL.");
            }
            else
            {
                Debug.LogError("Failed to download texture: " + request.error);
            }
        }
    }

    // Method to load and set the texture from a local file
    public void LoadAndSetImage(string id)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "images", id + ".jpg");
        Debug.Log("Getting image from: " + filePath);

        if (File.Exists(filePath))
        {
            // Read the image file as a byte array
            byte[] imageData = File.ReadAllBytes(filePath);

            // Create a new Texture2D and load the image data
            Texture2D loadedTexture = new Texture2D(2, 2);
            if (loadedTexture.LoadImage(imageData))
            {
                // Apply the texture to the material
                ImageRenderer.material.SetTexture("_MainTex", loadedTexture);
                Debug.Log("Texture successfully applied from local file.");
            }
            else
            {
                Debug.LogError("Failed to load texture data.");
            }
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
        }
    }




    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180f, 0);

    }
}
