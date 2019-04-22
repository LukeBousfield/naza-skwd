using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gauge : MonoBehaviour {

	public Text txt;
	public string gaugeName = "VELOCITY";
	public string units = "m/s";
	public GameObject circle;
	public float minValue, maxValue;
	public float value;


	void Start () {
	}

	void Update () {
		circle.GetComponent<Image>().fillAmount= (value/(maxValue - minValue));
		txt.text = (gaugeName + '\n' + value + units);
	}
}
