using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SIDE
{
    LEFT,
    RIGHT,
    DEFAULT
}

//right to left swipe using left hand
//<----------|
public class LeftSwipe : MonoBehaviour {

    public SIDE side;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //placed in enter as if controller just enter the lefthitbox, should immediately check
    //if user alr had swiped from left to right
    private void OnTriggerEnter(Collider other)
    {
        //check if the object is a controller
        if (other.GetComponent<SteamVR_TrackedObject>())
        {
            //check if the controller is left
            if (other.gameObject == MainSceneController.instance.LeftController)
            {
                if (side == SIDE.LEFT)
                {
                    GestureHitboxController.instance.OnSwipeLeft(side);
                }
            }
        }
    }

    //placed in exit as the timer should only countdown when user start to swipe from left to right
    //start timer when hand exits the right hitbox, so when the user enters the left hitbox,
    //it can be considered as a left to right swipe
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<SteamVR_TrackedObject>())
        {
            if (other.gameObject == MainSceneController.instance.LeftController)
            {
                if (side == SIDE.RIGHT)
                {
                    GestureHitboxController.instance.OnSwipeLeft(side);
                }
            }
        }
    }
}
