using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SIDE
{
    LEFT,
    RIGHT
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

    //placed in enter as if controller just enter the righthitbox, should immediately check
    //if user alr had swiped from left to right
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<SteamVR_TrackedObject>())
        {
            if ((int)other.GetComponent<SteamVR_TrackedObject>().index == GestureHitboxController.instance.leftIndex)
            {
                if (side == SIDE.LEFT)
                {
                    GestureHitboxController.instance.OnSwipeLeft(side);
                }
            }
        }
    }

    //placed in exit as the timer should only countdown when user start to swipe from left to right
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<SteamVR_TrackedObject>())
        {
            if ((int)other.GetComponent<SteamVR_TrackedObject>().index == GestureHitboxController.instance.leftIndex)
            {
                if (side == SIDE.RIGHT)
                {
                    GestureHitboxController.instance.OnSwipeLeft(side);
                }
            }
        }
    }
}
