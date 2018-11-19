using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockpitController : MonoBehaviour {

    [Tooltip("Color for the pointlight in the cockpit in blue MECH")]
    public Color redColor;  
    [Tooltip("The red MECH UI aesthetics in the cockpit")]
    public Texture redPanel;
    [Tooltip("Color for the pointlight in the cockpit in red MECH")]
    public Color blueColor;
    [Tooltip("The blue MECH UI aesthetics in the cockpit")]
    public Texture bluePanel;

    [Tooltip("The screen with the tutorial instruction on it")]
    public GameObject tutorialScreen;
    [Header("RED MECH")]
    public Texture red_swipeRightToLeftText;
    public Texture red_swipeLeftToRightText;
    public Texture red_swingArmsForwardAndBackText;
    public Texture red_leftHandEnterRing;
    public Texture red_rightHandEnterRing;
    public Texture red_end;
    [Header("BLUE MECH")]
    public Texture blue_swipeRightToLeftText;
    public Texture blue_swipeLeftToRightText;
    public Texture blue_swingArmsForwardAndBackText;
    public Texture blue_leftHandEnterRing;
    public Texture blue_rightHandEnterRing;
    public Texture blue_end;

    //this will just be assinged the corresponding textures so i dont need check everytime
    Texture leftHandEnterRing;
    Texture rightHandEnterRing;
    Texture swipeRightToLeftText;
    Texture swipeLeftToRightText;
    Texture swingArmsForwardAndBackText;
    Texture end;

    [Tooltip("The cockpit light inside the cockpit")]
    public Light cockpitLight;

	// Use this for initialization
	void Start () {
        if (DataCollector.Instance.scenario == DataCollector.PROJECT_CASE.BLUE_NO_PERSUADE_PILOT_BLUE
            || DataCollector.Instance.scenario == DataCollector.PROJECT_CASE.BLUE_PERSUADE_PILOT_BLUE)
        {
            //if the mech selected is blue, use blue mech textures and color
            cockpitLight.color = blueColor;
            cockpitLight.intensity = 1;
            GetComponent<MeshRenderer>().materials[3].SetTexture("_MainTex", bluePanel);
            leftHandEnterRing = blue_leftHandEnterRing;
            rightHandEnterRing = blue_rightHandEnterRing;
            swipeRightToLeftText = blue_swipeRightToLeftText;
            swipeLeftToRightText = blue_swipeLeftToRightText;
            swingArmsForwardAndBackText = blue_swingArmsForwardAndBackText;
            end = blue_end;
        }
        else
        {
            //if the mech selected is red, use red mech textures and color
            cockpitLight.color = redColor;
            cockpitLight.intensity = 2;
            GetComponent<MeshRenderer>().materials[3].SetTexture("_MainTex", redPanel);
            leftHandEnterRing = red_leftHandEnterRing;
            rightHandEnterRing = red_rightHandEnterRing;
            swipeRightToLeftText = red_swipeRightToLeftText;
            swipeLeftToRightText = red_swipeLeftToRightText;
            swingArmsForwardAndBackText = red_swingArmsForwardAndBackText;
            end = red_end;
        }
        //switch off the light at the start
        cockpitLight.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        //change text based on what part of the tutorial the user is at
        if (!MainSceneController.instance.leftAttached)
        {
            tutorialScreen.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", leftHandEnterRing);
        }
        else if (!MainSceneController.instance.rightAttached)
        {
            tutorialScreen.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", rightHandEnterRing);
        }
        else if (!MainSceneController.instance.swipeLeftBefore)
        {
            //turn on the light once the user "on" the mech
            cockpitLight.enabled = true;
            tutorialScreen.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", swipeRightToLeftText);
        }
        else if (!MainSceneController.instance.swipeRightBefore)
        {
            tutorialScreen.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", swipeLeftToRightText);
        }
        else if (!MainSceneController.instance.moveBefore)
        {
            tutorialScreen.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", swingArmsForwardAndBackText);
        }
        else
        {
            if (MainSceneController.instance.state == MainSceneController.GAME_STATE.GAME)
            {
                //give instructions when the user is moving through the drebris
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
            else if (MainSceneController.instance.state == MainSceneController.GAME_STATE.END)
            {
                tutorialScreen.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", end);
            }
        }
	}
}
