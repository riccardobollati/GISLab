using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MixedReality.Toolkit.UX;

public class ScaleCubeWithSlider : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;
    public GameObject cube;

    public float minScale = 0.1f;
    public float maxScale = 3.0f;
    void Start()
    {
        slider.OnValueUpdated.AddListener(UpdateCubeScale);
    }


    // Update is called once per frame
    void UpdateCubeScale(SliderEventData eventData)
    {
        float scaleValue = Mathf.Lerp(minScale, maxScale, eventData.NewValue);
        cube.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
    }
}
