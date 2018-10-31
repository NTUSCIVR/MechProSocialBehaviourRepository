using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;
using UnityEngine.UI;

public class HangerSceneController : MonoBehaviour
{
    public GameObject eyeLight;
    public Canvas CanvasObject;

    public GameObject leftControllerObject;
    public GameObject rightControllerObject;
    private SteamVR_Controller.Device leftController { get { return SteamVR_Controller.Input((int)leftControllerObject.GetComponent<SteamVR_TrackedObject>().index); } }
    private SteamVR_Controller.Device rightController { get { return SteamVR_Controller.Input((int)rightControllerObject.GetComponent<SteamVR_TrackedObject>().index); } }

    public List<string> persuadeLines;
    public List<string> noPersuadeLines;
    public float DisplayRate = 1.0f;
    public float timeToNextScene = 1f;

    public AudioSource robotBeep;

    Text TextObject;
    float timer = 0.0f;
    List<string> displayedLines;
    int wordIndex = 0;
    int linesIndex = 0;

    private void Awake()
    {
        
    }

    private void Start()
    {
        // Get Text Under Canvas
        TextObject = CanvasObject.transform.Find("Dialogue Text").GetComponent<Text>();
        TextObject.text = "";

        if (DataCollector.Instance)
        {
            //decide which lines to be used
            if (DataCollector.Instance.scenario == DataCollector.PROJECT_CASE.BLUE_PERSUADE_PILOT_BLUE ||
                DataCollector.Instance.scenario == DataCollector.PROJECT_CASE.BLUE_PERSUADE_PILOT_RED)
            {
                displayedLines = persuadeLines;
            }
            else
            {
                displayedLines = noPersuadeLines;
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

        if (leftController.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (linesIndex < displayedLines.Count)
                if (wordIndex == displayedLines[linesIndex].Length)
                {
                    if(linesIndex + 1 < displayedLines.Count)
                        robotBeep.Play();
                    TextObject.text = "";
                    ++linesIndex;
                    wordIndex = 0;
                }
        }
        if(rightController.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (linesIndex < displayedLines.Count)
                if (wordIndex == displayedLines[linesIndex].Length)
                {
                    if (linesIndex + 1 < displayedLines.Count)
                        robotBeep.Play();
                    TextObject.text = "";
                    ++linesIndex;
                    wordIndex = 0;
                }
        }

        if(linesIndex == displayedLines.Count)
        {
            timer += Time.deltaTime;
            if (timer > timeToNextScene)
            {
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
