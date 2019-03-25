using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemAdapter : MonoBehaviour
{

    public void UpdateSystem(SystemState s)
    {
        transform.position = new Vector3((float)s.Position.Longitude, (float)s.Position.Altitude / 10 - 2000f, (float)s.Position.Latitude);
        GetComponent<Rigidbody>().rotation = s.Rotation;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
