using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemAdapter : MonoBehaviour
{

    Vector3 newPos;
    Vector3 pos;
    Vector3 pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9;

    float alt;
    float alt1, alt2, alt3, alt4, alt5, alt6, alt7, alt8, alt9;

    public GameObject velcoityMeter;
    public GameObject altVelocityMeter;
    public GameObject verticalAccelerationMeter;
    public GameObject gForceMeter;
    public double v;

    public void UpdateSystem(SystemState s)
    {
        //breaks at loop 2425
        //breaks at loop 3765

        //transform rotation everytime new data set in inputed
        newPos = new Vector3(
            ((float)s.Position.Longitude + 1.405486f) * 20902464f*-1, //converts radians to feet by taking difference in radians from initial and multiplying it by converter
            (float)s.Position.Altitude + 100, //adds 400 so bottom of ship starts on surface
            ((float)s.Position.Latitude - 0.49669439f) * 20902464f); //converts radians to feet by taking difference in radians from initial and multiplying it by converter

        //position changers
        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * 3f);
        //transform.position = newPos;

        //rotation changers
        transform.rotation = s.Rotation * Quaternion.Euler(-101.6f, 0.9f, -.9f);
        //transform.rotation = Quaternion.Slerp(transform.rotation, s.Rotation * Quaternion.Euler(-101.7f, 0.12f, 0.06f), Time.deltaTime * 2f);//* Quaternion.Euler(-100.312f, 0, 0)

        //Get position velocity change values
        pos9 = pos8; pos8 = pos7; pos7 = pos6;
        pos6 = pos5; pos5 = pos4; pos4 = pos3;
        pos3 = pos2; pos2 = pos1; pos1 = pos;

        //update pos value
        pos = new Vector3(
            ((float)s.Position.Longitude + 1.405486f) * 20902464f * -1,
            (float)s.Position.Altitude + 100,
            ((float)s.Position.Latitude - 0.49669439f) * 20902464f);

        //Get altitude velocity change values
        alt9 = alt8; alt8 = alt7; alt7 = alt6;
        alt6 = alt5; alt5 = alt4; alt4 = alt3;
        alt3 = alt2; alt2 = alt1; alt1 = alt;
       
        //update altitude value
        alt = (float)s.Position.Altitude + 100;

        //Debug.Log(alt);
        //Debug.Log(alt1);
        //Debug.Log(alt2);

        //Overall velocity readout
        Vector3 posDiff = newPos - pos9;
            double instVel = System.Math.Abs(System.Math.Round(posDiff.magnitude * (40 / 3)));
            if (velcoityMeter != null)
            {
                velcoityMeter.GetComponent<gauge>().value = (float)instVel;
            }

        //Altitude velocity readout
            float altDiff = System.Math.Abs(alt) - System.Math.Abs(alt9);

            double altVel = System.Math.Round(altDiff * (40 / 3));
            if (altVelocityMeter != null)
            {
                altVelocityMeter.GetComponent<gauge>().value = (float)altVel;
            }

        //Altitude acceleration readout
            float v1 = (alt - alt3) * 40;
            float v2 = (alt6 - alt9) * 40;

            double accel = System.Math.Round((v1 - v2) * 20 * 10) / 10;
            if (verticalAccelerationMeter != null)
            {
                verticalAccelerationMeter.GetComponent<gauge>().value = (float)accel;
            }

        //Altitude acceleration readout
            double gForce = 1 + System.Math.Round((((v1 - v2) * 20) * 0.3048 / 9.81 * 10)) / 10;
            if (gForceMeter != null)
            {
                gForceMeter.GetComponent<gauge>().value = (float)System.Math.Abs(gForce);
            }



        //transform.rotation = s.Rotation;// * Quaternion.Euler(-100.312f, 0, 0); //changes rotation, quaternion changes initial rotation
        //Debug.Log("TEST QUATERNION: " + s.Rotation * Quaternion.Euler(-100.312f, 0, 0));

    /*
    //get velocity change values
    if (count == 1)
    {
    pos3 = pos2; pos2 = pos1; pos1 = pos;
    alt3 = alt2; alt2 = alt1; alt1 = alt;
    alt = (float)s.Position.Altitude + 100;
    pos = new Vector3(
    ((float)s.Position.Longitude + 1.405486f) * 20902464f * -1, //converts radians to feet by taking difference in radians from initial and multiplying it by converter
    (float)s.Position.Altitude + 100, //adds 400 so bottom of ship starts on surface
    ((float)s.Position.Latitude - 0.49669439f) * 20902464f); //converts radians to feet by taking difference in radians from initial and multiplying it by converter

    Debug.Log("0" + pos);
    Debug.Log("1" + pos3);
    }

    if (count == 1) count = 2;
    if (count == 2) count = 3;
    if (count == 3) count = 4;
    if (count == 4) count = 5;
    if (count == 5) count = 6;
    if (count == 6) count = 7;
    if (count == 7) count = 8;
    if (count == 8) count = 9;
    if (count == 9) count = 1;
    */
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
