using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;

public class GestureHitboxController : MonoBehaviour {

    public static GestureHitboxController instance;
    [Tooltip("Reference to IdentifyGesture script in scene")]
    public IdentifyGesture identifyGesture; 
    [Tooltip("Time needed for sideways gesture to get recognized")]
    public float swipeDelay = 1f;

    //bunch of booleans and timers for checking gestures
    //if the user move their hand through a hitbox, it will start a timer
    //if the user touch the triggerbox that corresponds with the hitbox the user touched
    //it will register as a gesture
    public bool swipingLeft = false;
    float leftSwipeTimer = 0f;

    public bool swipingRight = false;
    float rightSwipeTimer = 0f;

    public bool swipingForwardIn = false;
    float forwardInSwipeTimer = 0f;
    public bool swipingBackwardIn = false;
    float backwardInSwipeTimer = 0f;
    public bool swipingForwardOut = false;
    float forwardOutSwipeTimer = 0f;
    public bool swipingBackwardOut = false;
    float backwardOutSwipeTimer = 0f;

    private void Awake()
    {
        instance = this;
        
    }

    // Use this for initialization
    void Start () { 
    }
	
	// Update is called once per frame
	void Update () {
        //all these are to update the timers for every action
        if (swipingLeft)
        {
            if(leftSwipeTimer > swipeDelay)
            {
                leftSwipeTimer = 0f;
                swipingLeft = false;
            }
            leftSwipeTimer += Time.deltaTime;
        }

        if(swipingRight)
        {
            if(rightSwipeTimer > swipeDelay)
            {
                rightSwipeTimer = 0f;
                swipingRight = false;
            }
            rightSwipeTimer += Time.deltaTime;
        }

        if (swipingForwardIn)
        {
            if (forwardInSwipeTimer > swipeDelay * 2)
            {
                forwardInSwipeTimer = 0f;
                swipingForwardIn = false;
            }
            forwardInSwipeTimer += Time.deltaTime;
        }

        if(swipingBackwardIn)
        {
            if(backwardInSwipeTimer > swipeDelay * 2)
            {
                backwardInSwipeTimer = 0f;
                swipingBackwardIn = false;
            }
            backwardInSwipeTimer += Time.deltaTime;
        }

        if (swipingForwardOut)
        {
            if (forwardOutSwipeTimer > swipeDelay * 2)
            {
                forwardOutSwipeTimer = 0f;
                swipingForwardOut = false;
            }
            forwardOutSwipeTimer += Time.deltaTime;
        }

        if (swipingBackwardOut)
        {
            if (backwardOutSwipeTimer > swipeDelay * 2)
            {
                backwardOutSwipeTimer = 0f;
                swipingBackwardOut = false;
            }
            backwardOutSwipeTimer += Time.deltaTime;
        }
	}
    
    //when the user swiped left
    public void OnSwipeLeft(SIDE side)
    {
        //if the user attached their hands to the rings
        if (MainSceneController.instance.leftAttached)
        {
            //if the user started swiping from the right side
            if (side == SIDE.RIGHT)
            {
                //starts the timer to check for the hand to enter the left box
                swipingLeft = true;
            }
            else
            {
                //if the user swiped the left box, and the right box was swiped
                if (swipingLeft)
                {
                    //user has actually swiped right to left
                    //ask the robot to perform the animation
                    identifyGesture.PlaySwipeLeftAnimation();
                    swipingLeft = false;
                    leftSwipeTimer = 0f;
                }
            }
        }
    }

    public void OnSwipeRight(SIDE side)
    {
        //same way the left swipe works
        //refer to OnSwipeLeft and just change the side
        if (MainSceneController.instance.rightAttached)
        {
            if (side == SIDE.LEFT)
            {
                swipingRight = true;
            }
            else
            {
                if (swipingRight)
                {
                    identifyGesture.PlaySwipeRightAnimation();
                    swipingRight = false;
                    rightSwipeTimer = 0f;
                }
            }
        }
    }

    //start the timer when the user move their hands into a box

    //when the user enters one of the forward hitboxes
    public void OnSwipeIn(FORWARD_SIDE side)
    {
        if (MainSceneController.instance.leftAttached && MainSceneController.instance.rightAttached)
        {
            //set the booleans to true to check for a forward swipe
            if (side == FORWARD_SIDE.FORWARD)
            {
                swipingForwardIn = true;
                forwardInSwipeTimer = 0f;
            }
            else
            {
                swipingBackwardIn = true;
                backwardInSwipeTimer = 0f;
            }
        }
    }

    public void OnSwipeOut(FORWARD_SIDE side)
    {
        if (MainSceneController.instance.leftAttached && MainSceneController.instance.rightAttached)
        {
            if (side == FORWARD_SIDE.FORWARD)
            {
                swipingForwardOut = true;
                forwardOutSwipeTimer = 0f;
            }
            else
            {
                swipingBackwardOut = true;
                backwardOutSwipeTimer = 0f;
            }
        }
    }

    public void CheckForwardGesture()
    {
        if(swipingBackwardIn && swipingForwardIn && swipingForwardOut && swipingBackwardOut)
        {
            if (MainSceneController.instance.swipeLeftBefore == true && MainSceneController.instance.swipeRightBefore == true)
            {
                identifyGesture.PlayBobAnimation();
                swipingBackwardIn = swipingBackwardOut = swipingForwardIn = swipingForwardOut = false;
                backwardInSwipeTimer = backwardOutSwipeTimer = forwardInSwipeTimer = forwardOutSwipeTimer = 0f;
                //DataCollector.Instance.PushData("Time taken for moving swipe: " + GetLowestTime(new float[] { backwardInSwipeTimer, backwardOutSwipeTimer, forwardInSwipeTimer, forwardOutSwipeTimer }));
                MainSceneController.instance.moveBefore = true;
            }
        }
    }

    float GetLowestTime(float[] times)
    {
        float lowestTime = float.MaxValue;
        foreach(float time in times)
        {
            if (time == 0)
                continue;
            if(time < lowestTime)
                lowestTime = time;
        }
        return lowestTime;
    }
}
