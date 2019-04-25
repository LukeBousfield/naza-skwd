using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemAdapter : MonoBehaviour
{

    Vector3 newPos;// = new Vector3(0,0,0); // used for calculating velocity
    Vector3 pos1;
    Vector3 pos2;
    Vector3 pos3;
    Vector3 pos4;
    Vector3 pos5;
    Vector3 pos6;
    Vector3 pos7;
    Vector3 pos8;
    Vector3 pos9;
    public GameObject velcoityMeter;
    public double v;

    public void UpdateSystem(SystemState s)
    {
        //breaks at loop 2425
        //breaks at loop 3765

        pos9 = pos8;
        pos8 = pos7;
        pos7 = pos6;
        pos6 = pos5;
        pos5 = pos4;
        pos4 = pos3;
        pos3 = pos2;
        pos2 = pos1;
        pos1 = newPos;

        //transform rotation everytime new data set in inputed
        newPos = new Vector3(
            ((float)s.Position.Longitude + 1.405486f) * 20902464f*-1, //converts radians to feet by taking difference in radians from initial and multiplying it by converter
            (float)s.Position.Altitude + 100, //adds 400 so bottom of ship starts on surface
            ((float)s.Position.Latitude - 0.49669439f) * 20902464f); //converts radians to feet by taking difference in radians from initial and multiplying it by converter


        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * .7f);
        transform.rotation = Quaternion.Slerp(transform.rotation, s.Rotation * Quaternion.Euler(-100.312f, 0, 0), Time.deltaTime * .7f);//* Quaternion.Euler(-100.312f, 0, 0)


        float posDiff = newPos.magnitude - pos9.magnitude;
        double instVel = System.Math.Abs(System.Math.Round(posDiff * (40/3)));
        if (velcoityMeter != null)
        {
            velcoityMeter.GetComponent<gauge>().value = (float)instVel;
        }

        double accel = ((System.Math.Abs(newPos.magnitude - pos3.magnitude) * 40) - (System.Math.Abs(pos6.magnitude - pos9.magnitude) * 40))*20;
        //Debug.Log(accel);


        //transform.rotation = s.Rotation;// * Quaternion.Euler(-100.312f, 0, 0); //changes rotation, quaternion changes initial rotation
        //Debug.Log("TEST QUATERNION: " + s.Rotation * Quaternion.Euler(-100.312f, 0, 0));
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Updateis called once per frame
    void Update()
    {

    }
}













/*
if (hasLastPos && Time.time - lastTime > updateInterval && newPos != lastPos)
{
    if(lastPos5 != new Vector3(0,0,0) && v < (newPos - lastPos5).magnitude/(((Time.time - lastTime) * 5)))
    {
        Vector3 posDiff = newPos - lastPos5;
        double timeDiff = Time.time - lastTime;
        double instVel = System.Math.Round(posDiff.magnitude / (timeDiff*5));
        if (velcoityMeter != null) velcoityMeter.GetComponent<gauge>().value = (float)instVel;
        v = System.Math.Round(posDiff.magnitude/(timeDiff * 5));
    }
    else
    {
        double instVel = v;
        if (velcoityMeter != null) velcoityMeter.GetComponent<gauge>().value = (float)instVel;
    }

    hasLastPos = true;
    lastPos5 = lastPos4;
    lastPos4 = lastPos3;
    lastPos3 = lastPos2;
    lastPos2 = lastPos;
    lastPos = newPos;
    lastTime = Time.time;

} else if (Time.time - lastTime > updateInterval)
{
    hasLastPos = true;
    lastPos5 = lastPos4;
    lastPos4 = lastPos3;
    lastPos3 = lastPos2;
    lastPos2 = lastPos;
    lastPos = newPos;
    lastTime = Time.time;
}*/
