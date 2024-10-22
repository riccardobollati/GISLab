using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MixedReality.Toolkit.UX;
public class HeightCubeWithSlider : MonoBehaviour
{
    public Slider slider;
    public GameObject cube;

    public float minHeight = 0.0f;
    public float maxHeight = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        slider.OnValueUpdated.AddListener(UpdateCubeHeight);
    }

    void UpdateCubeHeight(SliderEventData eventData)
    {
        float newHeight = Mathf.Lerp(minHeight, maxHeight, eventData.NewValue);
        cube.transform.localPosition = new Vector3(cube.transform.localPosition.x, newHeight, cube.transform.localPosition.z);
    }
}
