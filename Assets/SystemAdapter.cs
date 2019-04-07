using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemAdapter : MonoBehaviour
{
    //public Quaternion rot;
    //float time;

    public void UpdateSystem(SystemState s)
    {
        //transform rotation evertime new data set in inputed
        transform.position = new Vector3(
            ((float)s.Position.Longitude + 1.405486f) * 20902464f * -1, //converts radians to feet by taking difference in radians from initial and multiplying it by converter
            (float)s.Position.Altitude + 400, //adds 400 so bottom of ship starts on surface
            ((float)s.Position.Latitude - 0.49669439f) * 20902464f); //converts radians to feet by taking difference in radians from initial and multiplying it by converter

        transform.rotation = s.Rotation * Quaternion.Euler(-100.312f, 0, 0); //changes rotation, quaternion changes initial rotation

        //float timeGap = Time.time - time;
        //rot = s.Rotation * Quaternion.Euler(-100.312f, 0, 0);
        //time = Time.time;
    }

    // Start is called before the first frame update
    void Start()
    {
        //time = Time.time;
    }

    // Updateis called once per fram e
    void Update()
    {
        //GetComponent<Rigidbody>().MoveRotation(rot);
    }
}
