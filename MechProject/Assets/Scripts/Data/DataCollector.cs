﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataCollector : MonoBehaviour {

    //the 4 cases that the mech project tackles
    public enum PROJECT_CASE
    {
        DEFAULT,
        BLUE_PERSUADE_PILOT_BLUE,
        BLUE_PERSUADE_PILOT_RED,
        BLUE_NO_PERSUADE_PILOT_BLUE,
        BLUE_NO_PERSUADE_PILOT_RED
    }
    PROJECT_CASE blind = PROJECT_CASE.DEFAULT;
    //the scenario selected by the user
    public PROJECT_CASE scenario;
    //the input field for the user to enter their name
    public InputField inputField;
    public string dataID = "";
    static DataCollector s_instance;
    public static DataCollector Instance { get { if (!s_instance)
            {
                GameObject go = new GameObject("DataCollector");
                s_instance = go.AddComponent<DataCollector>();
            }
            return s_instance;
        }
        set { s_instance = value; }
    }
    string currentPath = "";
    public bool blind = false;
    private void Awake()
    {
        int blindoption = PlayerPrefs.GetInt("Blind", (int)blind) + 1;
        if (blindoption >= Enum.GetValues(typeof(PROJECT_CASE)).Length)
            blind = PROJECT_CASE.BLUE_PERSUADE_PILOT_BLUE;
        else
            blind = (PROJECT_CASE)blindoption;
        DontDestroyOnLoad(this);
        if(!Directory.Exists(Application.dataPath + "/Data"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Data");
        }
    }

    // Use this for initialization
    void Start () {
        Instance = this;
    }

    //edit the current file by adding the new text
    public void PushData(string text)
    {
        StreamWriter sw = File.AppendText(GetPath());
        sw.WriteLine(text);
        sw.Close();
    }

    //returns the file path being used to store the data
    private string GetPath()
    {
        if (currentPath == "")
        {
            //if the filepath already exists, create a new file with a duplicate number
            string filePath = Application.dataPath + "/Data/" + dataID + ".csv";
            int duplicateCounts = 0;
            while (true)
            {
                if (File.Exists(filePath))
                {
                    ++duplicateCounts;
                    filePath = Application.dataPath + "/Data/" + dataID + "(" + duplicateCounts.ToString() + ")" + ".csv";
                }
                else
                    break;
            }
            currentPath = filePath;
            return filePath;
        }
        else
            return currentPath;
    }

    void CreateCSV()
    {
        //create the csv file
        StreamWriter output = System.IO.File.CreateText(GetPath());
        output.WriteLine("Actions");
        output.Close();
    }

    public void OnScenarioSelect(GameObject btn)
    {
        //set the scenario based on what was selected
        string text = btn.name;
        blind = false;
        if (text == "BluePersuadePilotBlue")
        {
            scenario = PROJECT_CASE.BLUE_PERSUADE_PILOT_BLUE;
            Debug.Log("BluePersuadePilotBlue");
        }
        else if (text == "BluePersuadePilotRed")
        {
            scenario = PROJECT_CASE.BLUE_PERSUADE_PILOT_RED;
            Debug.Log("BluePersuadePilotRed");
        }
        else if (text == "BlueNoPersuadePilotBlue")
        {
            scenario = PROJECT_CASE.BLUE_NO_PERSUADE_PILOT_BLUE;
            Debug.Log("BlueNoPersuadePilotBlue");
        }
        else if (text == "BlueNoPersuadePilotRed")
        {
            scenario = PROJECT_CASE.BLUE_NO_PERSUADE_PILOT_RED;
            Debug.Log("BlueNoPersuadePilotRed");
        }
<<<<<<< HEAD
        else if (text == "Blind")
        {
            scenario = blind;
            //Array values = Enum.GetValues(typeof(PROJECT_CASE));
            //System.Random random = new System.Random();
            
            //scenario = (PROJECT_CASE)values.GetValue(random.Next(values.Length));
            //while(scenario == PROJECT_CASE.DEFAULT)
            //    scenario = (PROJECT_CASE)values.GetValue(random.Next(values.Length));
=======
        else
        {
            scenario = (PROJECT_CASE)Random.Range((int)PROJECT_CASE.BLUE_PERSUADE_PILOT_BLUE, (int)PROJECT_CASE.BLUE_NO_PERSUADE_PILOT_RED);
            blind = true;
>>>>>>> 6b62e0e283b8b29ba1c8b0102df9e98f7654facb
        }
    }

    public void OnStartPressed()
    {
        //if no text, dont let them proceed
        if (inputField.text == null)
            return;
        //if no scenario selected, dont let them proceed
        if (scenario == PROJECT_CASE.DEFAULT)
            return;
        dataID = inputField.text;
        PlayerPrefs.SetInt("Blind", (int)blind);PlayerPrefs.Save();
        CreateCSV();
        SceneChangeController.instance.ChangeScene("HangerScene");
    }
}
