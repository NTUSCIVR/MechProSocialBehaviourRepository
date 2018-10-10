using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockController : MonoBehaviour {

    public SIDE side;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("test");
        if(other.tag == "Controller")
        {
            if(other.gameObject == MainSceneController.instance.LeftController)
            {
                if(!MainSceneController.instance.leftAttached)
                {
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
                if(!MainSceneController.instance.rightAttached)
                {
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
