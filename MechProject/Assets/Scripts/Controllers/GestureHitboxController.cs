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
    
    public void OnSwipeLeft(SIDE side)
    {
        //if (MainSceneController.instance != null)
        if (MainSceneController.instance.leftAttached)
        {
            if (side == SIDE.RIGHT)
            {
                swipingLeft = true;
            }
            else
            {
                if (swipingLeft)
                {
                    identifyGesture.PlaySwipeLeftAnimation();
                    swipingLeft = false;
                    DataCollector.Instance.PushData("Time taken for swiping left: " + leftSwipeTimer);
                    leftSwipeTimer = 0f;
                }
            }
        }
    }

    public void OnSwipeRight(SIDE side)
    {
        //if (MainSceneController.instance != null)
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
                    DataCollector.Instance.PushData("Time taken for swiping right: " + rightSwipeTimer);
                    rightSwipeTimer = 0f;
                }
            }
        }
    }

    public void OnSwipeIn(FORWARD_SIDE side)
    {
        if (MainSceneController.instance.leftAttached && MainSceneController.instance.rightAttached)
        {
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
                DataCollector.Instance.PushData("Time taken for moving swipe: " + GetLowestTime(new float[] { backwardInSwipeTimer, backwardOutSwipeTimer, forwardInSwipeTimer, forwardOutSwipeTimer }));
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
