using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MixedReality.Toolkit.UX;

public class RotataCubeWithSlider : MonoBehaviour
{
    public Slider slider;
    public GameObject cube;
    // Start is called before the first frame update
    void Start()
    {
        slider.OnValueUpdated.AddListener(UpdateCubeRotation);
    }

    // Update is called once per frame
    void UpdateCubeRotation(SliderEventData eventData)
    {
        float newRotation = Mathf.Lerp(0, 360, eventData.NewValue);
        cube.transform.localRotation =  Quaternion.Euler(cube.transform.localRotation.x, newRotation, cube.transform.localRotation.z);
    }
}
