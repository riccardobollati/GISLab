using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMrotation : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject cube;
    public Quaternion getRotation()
    {
        return cube.transform.localRotation;
    }

    public void Show(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }
}
