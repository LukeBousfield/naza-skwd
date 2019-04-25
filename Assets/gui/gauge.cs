using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gauge : MonoBehaviour {

	public Text txt;
	public string gaugeName = "VELOCITY";
	public string units = " ft/s";
	public GameObject circle;
	public float minValue, maxValue;
	public float value;
    float refV = 0.0f;


    void Start () {
	}

	void Update () {
        float newPosition = Mathf.SmoothDamp(circle.GetComponent<Image>().fillAmount, (value / (maxValue - minValue)), ref refV, .3f);
        circle.GetComponent<Image>().fillAmount = newPosition;
		txt.text = (gaugeName + '\n' + value + units);
	}
}
