using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemAdapter : MonoBehaviour
{

    Vector3 newPos;
    Vector3 pos;
    Vector3 pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9;

    float alt;
    float alt1, alt2, alt3, alt4, alt5, alt6, alt7, alt8, alt9, alt10, alt11, alt12;
    float alt13, alt14, alt15, alt16, alt17, alt18, alt19, alt20, alt21, alt22, alt23, alt24;

    float maxS = 0; float maxV = 0; float maxA = 0; float maxG = 0;

    int count = 0;


    private float LastDataPoint;
    private bool LastDataPointRun = false;

    public GameObject velcoityMeter;
    public GameObject altVelocityMeter;
    public GameObject verticalAccelerationMeter;
    public GameObject gForceMeter;
    public double v;
    public float DataPointWaitTime;
    public GameObject DisplayAfterText;

    float smooth;
    float xValue;

    public void UpdateSystem(SystemState s)
    {

        LastDataPoint = Time.time;
        count = count + 1;
        //breaks at loop 2425
        //breaks at loop 3765

        smooth = .9f;

        if (count < 980)
        {
            xValue = 0;
        }
        else
        {
            xValue = ((float)s.Position.Longitude + 1.405486f) * 20902464f * -1;
        }

        //if(count>951 && count<1151)
        //{
           // smooth = .75f;
        //}

        newPos = new Vector3(
            xValue, //converts radians to feet by taking difference in radians from initial and multiplying it by converter
            (float)s.Position.Altitude + (float)95.54, //adds 400 so bottom of ship starts on surface
            ((float)s.Position.Latitude - 0.49669439f) * 20902464f); //converts radians to feet by taking difference in radians from initial and multiplying it by converter

        //position changers
            transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * smooth);
        //transform.position = newPos;

        //rotation changers
        transform.rotation = s.Rotation;// * Quaternion.Euler(180f, 0f, 0f);// * Quaternion.Euler(-101.6f, 0.9f, -.9f); //Quaternion.Euler(-90.6f, -12f, 23f);
        //transform.rotation = Quaternion.Slerp(transform.rotation, s.Rotation * Quaternion.Euler(-101.7f, 0.12f, 0.06f), Time.deltaTime * 2f);//* Quaternion.Euler(-100.312f, 0, 0)

        //Get position velocity change values
        pos9 = pos8; pos8 = pos7; pos7 = pos6;
        pos6 = pos5; pos5 = pos4; pos4 = pos3;
        pos3 = pos2; pos2 = pos1; pos1 = pos;

        //update pos value
        pos = new Vector3(
            ((float)s.Position.Longitude + 1.405486f) * 20902464f * -1,
            (float)s.Position.Altitude + 40,
            ((float)s.Position.Latitude - 0.49669439f) * 20902464f);

        //Get altitude velocity change values
        alt24 = alt23; alt23 = alt22; alt22 = alt21;
        alt21 = alt20; alt20 = alt19; alt19 = alt18;
        alt18 = alt17; alt17 = alt16; alt16 = alt15;
        alt15 = alt14; alt14 = alt13; alt13 = alt12;
        alt12 = alt11; alt11 = alt10; alt10 = alt9;
        alt9 = alt8; alt8 = alt7; alt7 = alt6;
        alt6 = alt5; alt5 = alt4; alt4 = alt3;
        alt3 = alt2; alt2 = alt1; alt1 = alt;
       
        //update altitude value
        alt = (float)s.Position.Altitude + 98;

        //Debug.Log(alt);
        //Debug.Log(alt1);
        //Debug.Log(alt2);

        //Overall velocity readout
        Vector3 posDiff = pos - pos9;
            double instVel = System.Math.Abs(System.Math.Round(posDiff.magnitude * (40 / 3)));
            if (velcoityMeter != null)
            {
                velcoityMeter.GetComponent<gauge>().value = (float)instVel;
            }

        //Altitude velocity readout
            float altDiff = alt - alt9;

            double altVel = System.Math.Round(altDiff * (40 / 3));
            if (altVelocityMeter != null)
            {
                altVelocityMeter.GetComponent<gauge>().value = (float)altVel;
            }

        //Altitude acceleration readout
            float v1 = (alt - alt3) * 40;
            float v2 = (alt21 - alt24) * 40;

            double accel = System.Math.Round((v1 - v2) * (40/7) * 10) / 10;
            if (verticalAccelerationMeter != null && System.Math.Abs(accel) < 500)
            {
                verticalAccelerationMeter.GetComponent<gauge>().value = (float)accel;
            }


        //Altitude acceleration readout
        double gForce = 1 + System.Math.Round((((v1 - v2) * (40/7)) * 0.3048 / 9.81 * 10)) / 10;
            if (gForceMeter != null && System.Math.Abs(gForce) < 20)
            {
                gForceMeter.GetComponent<gauge>().value = (float)System.Math.Abs(gForce);
            }



        if (instVel > maxS && count > 45)
        {
            maxS = (float)instVel;
            //Debug.Log("Speed: " + maxS);
        }

        if (altVel > maxV && count > 45)
        {
            maxV = (float)altVel;
            //Debug.Log("Speed: " + maxS);
        }

        if (accel > maxA && count > 45 && accel < 500)
        {
            maxA = (float)accel;
            //Debug.Log("GForce: " + maxG);
        }

        if (gForce > maxG && count > 45 && gForce < 30)
        {
            maxG = (float)System.Math.Abs(gForce);
            //Debug.Log("GForce: " + maxG);
        }




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
        if (!LastDataPointRun && Time.time - LastDataPoint > DataPointWaitTime)// && count > 100)
        {
            if(count > 100)
            {
                LastDataPointRun = true;
                Debug.Log("Data points are done");
                if(DisplayAfterText != null)
                {
                    DisplayAfterText.GetComponent<Text>().text = "< Max values displayed on guages >";
                    velcoityMeter.GetComponent<gauge>().value = (float)maxS;
                    altVelocityMeter.GetComponent<gauge>().value = (float)maxV;
                    verticalAccelerationMeter.GetComponent<gauge>().value = (float)maxA;
                    gForceMeter.GetComponent<gauge>().value = (float)maxG;
                }
            }
        }
    }
}

