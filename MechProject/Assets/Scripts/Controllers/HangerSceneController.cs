using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangerSceneController : MonoBehaviour {

    public GameObject eyeLight;
    float timer = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if(timer > 1f)
        {
            timer = 0f;
            eyeLight.SetActive(!eyeLight.activeSelf);
        }
	}

    public void ChangeToNextScene()
    {

    }
}
