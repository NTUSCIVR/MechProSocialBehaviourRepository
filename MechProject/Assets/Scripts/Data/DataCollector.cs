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
        BLUE_PERSUADE,
        BLUE_NO_PERSUADE,
        RED_PERSUADE,
        RED_NO_PERSUADE
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
        if (text == "BluePersuade")
            scenario = PROJECT_CASE.BLUE_PERSUADE;
        else if (text == "BlueNoPersuade")
            scenario = PROJECT_CASE.BLUE_NO_PERSUADE;
        else if (text == "RedPersuade")
            scenario = PROJECT_CASE.RED_PERSUADE;
        else if (text == "RedNoPersuade")
            scenario = PROJECT_CASE.RED_NO_PERSUADE;
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
