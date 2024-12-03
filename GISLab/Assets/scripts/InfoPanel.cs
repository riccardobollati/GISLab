using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    public int nlines = 7;
    public int llength = 15;
    public TextMeshProUGUI tmp;

    // Start is called before the first frame update
    private string text = string.Empty;
    private Queue<string> lines = new Queue<string>();

    public void WriteNewLine(string line)
    {
      
        while (lines.Count() >= nlines)
        {
            lines.Dequeue();    
        }
        lines.Enqueue(line);
        text = string.Empty;
        foreach (string l in lines)
        {
            text += l;
            text += "\n";
        }

        Flush();
    }

    private void Flush()
    {
        tmp.SetText(text);
    }
}
