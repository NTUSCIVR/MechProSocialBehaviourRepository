using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockpitController : MonoBehaviour {

    public Color redColor;  
    public Texture redPanel;
    public Color blueColor;
    public Texture bluePanel;

    public GameObject tutorialScreen;
    public Texture swipeRightToLeftText;
    public Texture swipeLeftToRightText;
    public Texture swingArmsForwardAndBackText;
    public Texture turnOnText;

    public Light cockpitLight;

	// Use this for initialization
	void Start () {
        if (DataCollector.Instance.scenario == DataCollector.PROJECT_CASE.BLUE_NO_PERSUADE_PILOT_BLUE
            || DataCollector.Instance.scenario == DataCollector.PROJECT_CASE.BLUE_PERSUADE_PILOT_BLUE)
        {
            cockpitLight.color = blueColor;
            cockpitLight.intensity = 1;
            GetComponent<MeshRenderer>().materials[3].SetTexture("_MainTex", bluePanel);
        }
        else
        {
            cockpitLight.color = redColor;
            cockpitLight.intensity = 2;
            GetComponent<MeshRenderer>().materials[3].SetTexture("_MainTex", redPanel);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if(!MainSceneController.instance.leftAttached || !MainSceneController.instance.rightAttached)
        {
            tutorialScreen.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", turnOnText);
        }
		else if(!MainSceneController.instance.swipeLeftBefore)
        {
            tutorialScreen.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", swipeRightToLeftText);
        }
        else if(!MainSceneController.instance.swipeRightBefore)
        {
            tutorialScreen.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", swipeLeftToRightText);
        }
        else if(!MainSceneController.instance.moveBefore)
        {
            tutorialScreen.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", swingArmsForwardAndBackText);
        }
        else
        {
            if (MainSceneController.instance.movementIndex < MainSceneController.instance.rubberPlacements.Count)
            {
                if (MainSceneController.instance.rubberPlacements[MainSceneController.instance.movementIndex] == SIDE.DEFAULT)
                {
                    tutorialScreen.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", swingArmsForwardAndBackText);
                }
                else if (MainSceneController.instance.rubberPlacements[MainSceneController.instance.movementIndex] == SIDE.LEFT)
                {
                    tutorialScreen.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", swipeRightToLeftText);
                }
                else if (MainSceneController.instance.rubberPlacements[MainSceneController.instance.movementIndex] == SIDE.RIGHT)
                {
                    tutorialScreen.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", swipeLeftToRightText);
                }
            }
        }
	}
}
