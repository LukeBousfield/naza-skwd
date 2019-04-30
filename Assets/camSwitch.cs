using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camSwitch : MonoBehaviour
{
    public GameObject[] cameras;
    public int camNumber = 0;

    public void NextCam()
    {
        if (camNumber == cameras.Length - 1) camNumber = 0;
        else camNumber++;
    }
    public void PreviousCam()
    {
        if (camNumber == 0) camNumber = cameras.Length -1;
        else camNumber--;
    }
    
    void Update()
    {
        for(int i = 0; i < cameras.Length; i++)
        {
            if (camNumber != i) cameras[i].GetComponent<Camera>().enabled = false;
            else cameras[i].GetComponent<Camera>().enabled = true;
        }
    }
}
