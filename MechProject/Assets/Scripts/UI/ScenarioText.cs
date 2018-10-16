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
            case DataCollector.PROJECT_CASE.BLUE_PERSUADE:
                GetComponentInChildren<Text>().text = "Blue Persuade Selected";
                break;
            case DataCollector.PROJECT_CASE.BLUE_NO_PERSUADE:
                GetComponentInChildren<Text>().text = "Blue No Persuade Selected";
                break;
            case DataCollector.PROJECT_CASE.RED_PERSUADE:
                GetComponentInChildren<Text>().text = "Red Persuade Selected";
                break;
            case DataCollector.PROJECT_CASE.RED_NO_PERSUADE:
                GetComponentInChildren<Text>().text = "Red No Persuade Selected";
                break;
        }
	}
}
