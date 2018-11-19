﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockController : MonoBehaviour {

    [Tooltip("The side of the ring lock")]
    public SIDE side;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (side == SIDE.LEFT)
            Debug.Log(other.name + ": left");
        else
            Debug.Log(other.name + ": right");
        if(other.tag == "Controller")
        {
            //if the object is the left controller
            if(other.gameObject == MainSceneController.instance.LeftController)
            {
                //if the left ring has not been locked yet
                if(!MainSceneController.instance.leftAttached)
                {
                    //if the ring is the left ring
                    if (side == SIDE.LEFT)
                    {
                        MainSceneController.instance.leftAttached = true;
                        //attach to left hand
                        MainSceneController.instance.AttachRing(other.gameObject);
                        Destroy(gameObject);
                    }
                }
            }
            else
            {
                //if the right ring is not locked yet
                if(!MainSceneController.instance.rightAttached)
                {
                    //if the ring is the right ring
                    if (side == SIDE.RIGHT)
                    {
                        MainSceneController.instance.rightAttached = true;
                        //attach to right hand
                        MainSceneController.instance.AttachRing(other.gameObject);
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
