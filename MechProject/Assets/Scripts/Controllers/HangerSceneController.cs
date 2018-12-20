﻿using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;
using UnityEngine.UI;

public class HangerSceneController : MonoBehaviour
{
    [Tooltip("The eyelight of the robot")]
    public GameObject eyeLight;
    [Tooltip("The text object to display the robot words")]
    public Canvas CanvasObject;

    [Tooltip("The left controller of the player")]
    public GameObject leftControllerObject;
    [Tooltip("The right controller of the player")]
    public GameObject rightControllerObject;
    //the steam vr controller objects
    private SteamVR_Controller.Device leftController
    { get
        {
            int controllerIndex = (int)leftControllerObject.GetComponent<SteamVR_TrackedObject>().index;
            if (controllerIndex < 0)
                return null;
            return SteamVR_Controller.Input(controllerIndex);
        }
    }
    private SteamVR_Controller.Device rightController
    {
        get
        {
            int controllerIndex = (int)rightControllerObject.GetComponent<SteamVR_TrackedObject>().index;
            if (controllerIndex < 0)
                return null;
            return SteamVR_Controller.Input(controllerIndex);
        }
    }

    [Tooltip("The lines that the robot will tell you regardless of red or blue")]
    public List<string> startLines;
    [Tooltip("Lines for persuasion scenario")]
    public List<string> persuadeLines;
    [Tooltip("Lines for no persuasion scenario")]
    public List<string> noPersuadeLines;
    [Tooltip("Lines for when the red bot is selected")]
    public List<string> redBotLines;
    [Tooltip("Lines for when the blue bot is selected")]
    public List<string> blueBotLines;
    [Tooltip("Speed for how fast each letter appear, in word per second")]
    public float DisplayRate = 1.0f;
    [Tooltip("How long it takes to enter the next scene after the final text")]
    public float timeToNextScene = 1f;

    [Tooltip("Audio for the robot speaking")]
    public AudioSource robotBeep;

    //text object inside the canvas object
    Text TextObject;
    //timer for the display rate
    float timer = 0.0f;
    //the list of all lines used in the current scenario
    List<string> displayedLines;
    //the index of the letter in the current line
    int wordIndex = 0;
    //the index of the current line shown
    int linesIndex = 0;

    private void Start()
    {
        // Get Text Under Canvas
        TextObject = CanvasObject.transform.Find("Dialogue Text").GetComponent<Text>();
        TextObject.text = "";

        if (DataCollector.Instance)
        {
            //check wat instance was selected, then add the appropriate lines into
            //the displayed lines list
            if(DataCollector.Instance.blind)
            {
                DataCollector.Instance.PushData("Blind Choice");
            }
            switch (DataCollector.Instance.scenario)
            {
                case DataCollector.PROJECT_CASE.BLUE_NO_PERSUADE_PILOT_BLUE:
                    displayedLines = new List<string>(startLines.Count +
                        noPersuadeLines.Count +
                        blueBotLines.Count);
                    displayedLines.AddRange(startLines);
                    displayedLines.AddRange(noPersuadeLines);
                    displayedLines.AddRange(blueBotLines);
                    DataCollector.Instance.PushData("No Persuasion, Pilot Blue");
                    break;
                case DataCollector.PROJECT_CASE.BLUE_NO_PERSUADE_PILOT_RED:
                    displayedLines = new List<string>(startLines.Count +
                        noPersuadeLines.Count +
                        redBotLines.Count);
                    displayedLines.AddRange(startLines);
                    displayedLines.AddRange(noPersuadeLines);
                    displayedLines.AddRange(redBotLines);
                    DataCollector.Instance.PushData("No Persuasion, Pilot Red");
                    break;
                case DataCollector.PROJECT_CASE.BLUE_PERSUADE_PILOT_BLUE:
                    displayedLines = new List<string>(startLines.Count +
                        persuadeLines.Count +
                        blueBotLines.Count);
                    displayedLines.AddRange(startLines);
                    displayedLines.AddRange(persuadeLines);
                    displayedLines.AddRange(blueBotLines);
                    DataCollector.Instance.PushData("Persuasion, Pilot Blue");
                    break;
                case DataCollector.PROJECT_CASE.BLUE_PERSUADE_PILOT_RED:
                    displayedLines = new List<string>(startLines.Count +
                        persuadeLines.Count +
                        redBotLines.Count);
                    displayedLines.AddRange(startLines);
                    displayedLines.AddRange(persuadeLines);
                    displayedLines.AddRange(redBotLines);
                    DataCollector.Instance.PushData("Persuasion, Pilot Red");
                    break;
            }
        }
        else
            displayedLines = persuadeLines;

        // Robot Eye Shine
        eyeLight.SetActive(true);
    }

    // Update is called once per frame
    void Update ()
    {
        //add letters into text object at display rate and stop beeping once done
        timer += Time.deltaTime;
        if(timer > DisplayRate)
        {
            if (linesIndex < displayedLines.Count)
            {
                if (wordIndex < displayedLines[linesIndex].Length)
                {
                    TextObject.text += displayedLines[linesIndex][wordIndex];
                    ++wordIndex;
                    timer = 0f;
                }
                else
                {
                    robotBeep.Stop();
                }
            }
        }

        if (leftControllerObject.GetComponent<SteamVR_TrackedObject>().index > SteamVR_TrackedObject.EIndex.None ||
            rightControllerObject.GetComponent<SteamVR_TrackedObject>().index > SteamVR_TrackedObject.EIndex.None)
        {
            //if the controller trigger was pressed
            if (leftController.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) ||
            rightController.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                //if there are still more line sto display
                if (linesIndex < displayedLines.Count)
                    if (wordIndex == displayedLines[linesIndex].Length)
                    {
                        //if the current line has been displayed finished
                        //play the robot beep sound
                        if (linesIndex + 1 < displayedLines.Count)
                            robotBeep.Play();
                        //empty the current displayed text
                        TextObject.text = "";
                        ++linesIndex;
                        //start playing the next line
                        wordIndex = 0;
                    }
            }
        }

        //when the user is at the final line
        if(linesIndex == displayedLines.Count)
        {
            timer += Time.deltaTime;
            if (timer > timeToNextScene)
            {
                //wait to change scene to main scene
                ChangeToNextScene();
                Destroy(this);
            }
        }
    }

    public void ChangeToNextScene()
    {
        SceneChangeController.instance.ChangeScene("MainScene");
    }
}
