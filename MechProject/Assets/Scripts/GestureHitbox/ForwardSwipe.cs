using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FORWARD_SIDE
{
    FORWARD,
    BACKWARD
}

public class ForwardSwipe : MonoBehaviour {

    //the forward and backwards side
    public FORWARD_SIDE side;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //when the controller enters the hitbox
    private void OnTriggerEnter(Collider other)
    {
        //checks if user did walking motion
        GestureHitboxController.instance.CheckForwardGesture();
        GestureHitboxController.instance.OnSwipeIn(side);
    }

    //when the controller exits the hitbox
    private void OnTriggerExit(Collider other)
    {
        //check if suer did walking motion
        GestureHitboxController.instance.CheckForwardGesture();
        GestureHitboxController.instance.OnSwipeOut(side);
    }
}
