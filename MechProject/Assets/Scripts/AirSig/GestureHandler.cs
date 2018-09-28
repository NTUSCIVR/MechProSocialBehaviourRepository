using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using My;

public class GestureHandler : MonoBehaviour
{
    // Reference to AirSigManager for setting operation mode and registering listener
    public Manager Manager;

    // Reference to the vive controllers for handing key pressing
    public SteamVR_TrackedObject leftHandControl;
    public SteamVR_TrackedObject rightHandControl;

    public ParticleSystem leftTrack;
    public ParticleSystem rightTrack;
    
    // UI for displaying current status and operation results 
    public Text textMode;
    public Text textResult;
    public GameObject instruction;
    public GameObject cHeartDown;

    protected string textToUpdate;

    protected readonly string DEFAULT_INSTRUCTION_TEXT = "Pressing trigger and write in the air\nReleasing trigger when finish";
    protected string defaultResultText;

    // Set by the callback function to run this action in the next UI call
    protected Action nextUiAction;
    protected IEnumerator uiFeedback;

    protected string GetDefaultIntructionText()
    {
        return DEFAULT_INSTRUCTION_TEXT;
    }

    protected void ToggleGestureImage(string target)
    {
        if ("All".Equals(target))
        {
            cHeartDown.SetActive(true);
            foreach (Transform child in cHeartDown.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        else if ("Heart".Equals(target))
        {
            cHeartDown.SetActive(true);
            foreach (Transform child in cHeartDown.transform)
            {
                if (child.name == "Heart")
                {
                    child.gameObject.SetActive(true);
                }
                else
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
        else if ("C".Equals(target))
        {
            cHeartDown.SetActive(true);
            foreach (Transform child in cHeartDown.transform)
            {
                if (child.name == "C")
                {
                    child.gameObject.SetActive(true);
                }
                else
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
        else if ("Down".Equals(target))
        {
            cHeartDown.SetActive(true);
            foreach (Transform child in cHeartDown.transform)
            {
                if (child.name == "Down")
                {
                    child.gameObject.SetActive(true);
                }
                else
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            cHeartDown.SetActive(false);
        }
    }

    protected IEnumerator setResultTextForSeconds(string text, float seconds, string defaultText = "")
    {
        string temp = textResult.text;
        textResult.text = text;
        yield return new WaitForSeconds(seconds);
        textResult.text = defaultText;
    }

    protected void checkDbExist()
    {
        bool isDbExist = Manager.IsDbExist;
        if (!isDbExist)
        {
            textResult.text = "<color=red>Cannot find DB files!\nMake sure\n'Assets/AirSig/StreamingAssets'\nis copied to\n'Assets/StreamingAssets'</color>";
            textMode.text = "";
            instruction.SetActive(false);
            cHeartDown.SetActive(false);
        }
    }

    protected void UpdateUIandHandleControl()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (null != textToUpdate)
        {
            if (uiFeedback != null) StopCoroutine(uiFeedback);
            uiFeedback = setResultTextForSeconds(textToUpdate, 5.0f, defaultResultText);
            StartCoroutine(uiFeedback);
            textToUpdate = null;
        }

        if (-1 != (int)leftHandControl.index)
        {
            var device = SteamVR_Controller.Input((int)leftHandControl.index);
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                leftTrack.Clear();
                leftTrack.Play();
            }
            else if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                leftTrack.Stop();
            }
        }

        if (-1 != (int)rightHandControl.index)
        {
            var device = SteamVR_Controller.Input((int)rightHandControl.index);
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                rightTrack.Clear();
                rightTrack.Play();
            }
            else if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                rightTrack.Stop();
            }
        }

        if (nextUiAction != null)
        {
            nextUiAction();
            nextUiAction = null;
        }
    }

}
