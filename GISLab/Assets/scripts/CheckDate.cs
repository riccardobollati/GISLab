using MixedReality.Toolkit.Accessibility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDate : MonoBehaviour
{

    public InfoPanel infoPanel;
    public ReadCSV db;
    private DateTime newDate;
    private bool isValid = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void checkDate(string date)
    {
        Debug.Log(date);
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
            isValid = false;
        } else
        {
            isValid = true;
            newDate = DateTime.ParseExact(date, "yyyy-mm-dd", System.Globalization.CultureInfo.InvariantCulture);
            db.AddFixedDate(newDate);

        }

    }
}
