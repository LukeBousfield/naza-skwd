using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemAdapter : MonoBehaviour
{
    public void UpdateSystem(SystemState s)
    {
        //breaks at loop 2425
        //breaks at loop 3765

        //transform rotation evertime new data set in inputed
        Vector3 newPos = new Vector3(
            ((float)s.Position.Longitude + 1.405486f) * 20902464f*-1, //converts radians to feet by taking difference in radians from initial and multiplying it by converter
            (float)s.Position.Altitude + 100, //adds 400 so bottom of ship starts on surface
            ((float)s.Position.Latitude - 0.49669439f) * 20902464f); //converts radians to feet by taking difference in radians from initial and multiplying it by converter
        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * .7f);

        //transform.rotation = s.Rotation;// * Quaternion.Euler(-100.312f, 0, 0); //changes rotation, quaternion changes initial rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, s.Rotation * Quaternion.Euler(-100.312f, 0, 0), Time.deltaTime * .7f);

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
