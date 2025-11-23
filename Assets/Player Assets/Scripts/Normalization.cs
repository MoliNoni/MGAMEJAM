using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normalization : MonoBehaviour
{
    private GameObject[] objs;

    void Start()
    {
        objs = GameObject.FindGameObjectsWithTag("Normalizable");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            foreach (GameObject obj in objs)
            {
                bool state = obj.activeSelf;
                obj.SetActive(!state);
            }
        }
    }
}
