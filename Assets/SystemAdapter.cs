using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemAdapter : MonoBehaviour
{

    public void UpdateSystem(SystemState s)
    {
        transform.position = new Vector3(transform.position.x, (float)s.Position.Altitude / 10 - 2000f, transform.position.z);
        transform.rotation = s.Rotation;
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
