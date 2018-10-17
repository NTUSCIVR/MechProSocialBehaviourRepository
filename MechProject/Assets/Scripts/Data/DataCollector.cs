using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataCollector : MonoBehaviour {

    public enum PROJECT_CASE
    {
        DEFAULT,
        BLUE_PERSUADE_PILOT_BLUE,
        BLUE_PERSUADE_PILOT_RED,
        BLUE_NO_PERSUADE_PILOT_BLUE,
        BLUE_NO_PERSUADE_PILOT_RED
    }

    public PROJECT_CASE scenario;
    public InputField inputField;
    public string dataID = "";
    public static DataCollector Instance;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Use this for initialization
    void Start () {
        Instance = this;
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void PushData(string text)
    {
        StreamWriter sw = File.AppendText(GetPath());
        sw.WriteLine(text);
        sw.Close();
    }

    private string GetPath()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/Data/" + dataID + ".csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath+dataID + ".csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+dataID + ".csv";
#else
        return Application.dataPath +"/"+dataID + ".csv";
#endif
    }

    void CreateCSV()
    {
        if(File.Exists(GetPath()))
        {
            File.Delete(GetPath());
        }
        StreamWriter output = System.IO.File.CreateText(GetPath());
        output.WriteLine("Actions");
        output.Close();
    }

    string ChangeLetters(string str, char letter, char toBeLetter)
    {
        char[] ret = str.ToCharArray();
        for(int i = 0; i < ret.Length; ++i)
        {
            if(ret[i] == letter)
            {
                ret[i] = toBeLetter;
            }
        }
        return new string(ret);
    }

    public void OnScenarioSelect(GameObject btn)
    {
        string text = btn.name;
        if (text == "BluePersuadePilotBlue")
            scenario = PROJECT_CASE.BLUE_PERSUADE_PILOT_BLUE;
        else if (text == "BluePersuadePilotRed")
            scenario = PROJECT_CASE.BLUE_PERSUADE_PILOT_RED;
        else if (text == "BlueNoPersaudePilotBlue")
            scenario = PROJECT_CASE.BLUE_NO_PERSUADE_PILOT_BLUE;
        else if (text == "BlueNoPersuadePilotRed")
            scenario = PROJECT_CASE.BLUE_NO_PERSUADE_PILOT_RED;
    }

    public void OnStartPressed()
    {
        if (inputField.text == null)
            return;
        dataID = inputField.text;
        CreateCSV();
        SceneChangeController.instance.ChangeScene("HangerScene");
    }
}
