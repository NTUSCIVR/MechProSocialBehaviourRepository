using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//right to left swipe
public class RightSwipe : MonoBehaviour {

    public SIDE side;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //placed in enter as if controller just enter the lefthitbox, should immediately check
    //if user alr had swiped from right to left
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<SteamVR_TrackedObject>())
        {
            if (other.gameObject == MainSceneController.instance.RightController)
            {
                if (side == SIDE.RIGHT)
                {
                    GestureHitboxController.instance.OnSwipeRight(side);
                }
            }
        }
    }

    //placed in exit as the timer should only countdown when user start to swipe from left to right
    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<SteamVR_TrackedObject>())
        {
            if (other.gameObject == MainSceneController.instance.RightController)
            {
                if (side == SIDE.LEFT)
                {
                    GestureHitboxController.instance.OnSwipeRight(side);
                }
            }
        }
    }
}
