using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConeDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    public void UpdateConeText(double coneCount, TextMeshProUGUI textToChange, string optionalEndText = null)
    {
        string[] suffixes = { "", "K", "M", "B", "T", "Q" };
        int index = 0;

        // Handle zero case
        if (coneCount == 0)
        {
            textToChange.text = "0" + (optionalEndText ?? "");
            return;
        }

        while (coneCount >= 1000 && index < suffixes.Length - 1)
        {
            coneCount /= 1000;
            index++;
        }

        // Format number based on the index
        string formattedText = coneCount.ToString(index == 0 ? "F0" : "F1") + suffixes[index];

        textToChange.text = formattedText + (optionalEndText ?? ""); // Combine the formatted text with optional end text
    }

}
