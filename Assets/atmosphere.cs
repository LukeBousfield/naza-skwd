using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class atmosphere : MonoBehaviour
{
    public Color col1, col2, col3, currentColor;
    public float height1, height2, seaLevel;

    public float percentBetween;

    void Start()
    {
        
    }

    void Update()
    {
        //GetComponent<Rigidbody>().MovePosition(transform.position + transform.up * Time.deltaTime * 10);
        if(transform.position.y < height1)
        {
            percentBetween = (Mathf.Abs(transform.position.y)/(Mathf.Abs(height1) - Mathf.Abs(seaLevel)));
            currentColor = new Color((col2.r * percentBetween) +(col1.r * (1 - percentBetween)), (col2.g * percentBetween) + (col1.g * (1 - percentBetween)), (col2.b * percentBetween) + (col1.b * (1 - percentBetween)), 100);
        }
        else if(transform.position.y < height2)
        {
            percentBetween = ((Mathf.Abs(transform.position.y)  - Mathf.Abs(height1))/ (Mathf.Abs(height2) - Mathf.Abs(seaLevel) - Mathf.Abs(height1)));
            currentColor = new Color((col3.r * percentBetween) + (col2.r * (1 - percentBetween)), (col3.g * percentBetween) + (col2.g * (1 - percentBetween)), (col3.b * percentBetween) + (col2.b * (1 - percentBetween)), 100);
        }
        GetComponent<Camera>().backgroundColor = currentColor;
    }
}
