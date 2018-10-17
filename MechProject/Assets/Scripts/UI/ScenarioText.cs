using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScenarioText : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		switch(DataCollector.Instance.scenario)
        {
            case DataCollector.PROJECT_CASE.BLUE_PERSUADE_PILOT_BLUE:
                GetComponentInChildren<Text>().text = "Blue Persuade Pilot Blue Selected";
                break;
            case DataCollector.PROJECT_CASE.BLUE_PERSUADE_PILOT_RED:
                GetComponentInChildren<Text>().text = "Blue Persuade Pilot Red Selected";
                break;
            case DataCollector.PROJECT_CASE.BLUE_NO_PERSUADE_PILOT_BLUE:
                GetComponentInChildren<Text>().text = "Blue No Persuade Pilot Blue Selected";
                break;
            case DataCollector.PROJECT_CASE.BLUE_NO_PERSUADE_PILOT_RED:
                GetComponentInChildren<Text>().text = "Blue No Persuade Pilot Red Selected";
                break;
        }
	}
}
