using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class AMPosition : MonoBehaviour
{
    // Start is called before the first frame update
    // Start is called before the first frame update
    public Vector3 getPosition()
    {
        return transform.localPosition;
    }

    public void Show(bool isVisible)
    {
        gameObject.SetActive(isVisible);       
    }
}

