using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
public class NormalizationTimerUI : MonoBehaviour
{
    [SerializeField] public TMP_Text timerText;
    [SerializeField] private Normalization normalizationScript;

    void Update()
    {
        timerText.text = "Time: " + normalizationScript.timerCurrentTime;
    }
}
