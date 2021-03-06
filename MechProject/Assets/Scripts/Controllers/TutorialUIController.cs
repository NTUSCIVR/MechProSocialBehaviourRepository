﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//only purpose is to activate and deactivate the arrows
public class TutorialUIController : MonoBehaviour {

    public GameObject leftArrow;
    public GameObject rightArrow;
    public GameObject forwardBackwardArrow;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!MainSceneController.instance.leftAttached || !MainSceneController.instance.rightAttached)
            return;
        if (!MainSceneController.instance.swipeLeftBefore)
        {
            leftArrow.SetActive(true);
        }
        else
        {
            if (!MainSceneController.instance.swipeRightBefore)
            {
                leftArrow.SetActive(false);
                rightArrow.SetActive(true);
            }
            else
            {
                if (!MainSceneController.instance.moveBefore)
                {
                    rightArrow.SetActive(false);
                    forwardBackwardArrow.SetActive(true);
                }
                else
                {
                    forwardBackwardArrow.SetActive(false);
                }
            }
        }
	}
}
