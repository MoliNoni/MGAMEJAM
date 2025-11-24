using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normalization : MonoBehaviour
{
    [SerializeField] private bool isNormalizationOn = false;
     [SerializeField] public float timerCurrentTime = 5f;
     [SerializeField] public float maxTimerDuration =6f;
    private GameObject[] objs;

    void Start()
    {
        objs = GameObject.FindGameObjectsWithTag("Normalizable");
    }

    void Update()
    {


        if (Input.GetKeyDown(KeyCode.R) && isNormalizationOn == false && timerCurrentTime > 0.5f)
        {
            Normalize();
            isNormalizationOn = true;
        }
        else if (Input.GetKeyDown(KeyCode.R) && isNormalizationOn == true)
        {
            Normalize();
            isNormalizationOn = false;
        }

        if (isNormalizationOn == true )
        {
            timerCurrentTime = timerCurrentTime-Time.deltaTime;

            if (timerCurrentTime<=0)    //In order to interrumpt Normalization in case it runs out of time
            {
                Normalize();
                isNormalizationOn = false;
            }
        }
        

}
    void Normalize()
    {
        foreach (GameObject obj in objs)
        {
            obj.SetActive(!obj.activeSelf);
        }
    }

    void ResumeTimer()
    {
        
    }

    void PauseTimer()
    {
        
    }

    void AddTimeToTimer(float seconds)
    {
        timerCurrentTime += seconds;
        if (timerCurrentTime > maxTimerDuration)
        {
            timerCurrentTime = maxTimerDuration;
        }
        
    }
}


