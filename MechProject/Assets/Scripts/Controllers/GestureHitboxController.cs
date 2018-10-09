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

    public SteamVR_TrackedObject leftObj;
    public SteamVR_TrackedObject rightObj;

    SteamVR_Controller.Device leftController;
    SteamVR_Controller.Device rightController;

    public int leftIndex;
    public int rightIndex;

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
        leftIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);
        rightIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost); ;
    }

    // Use this for initialization
    void Start () { 
        leftController = SteamVR_Controller.Input((int)leftObj.index);
        rightController = SteamVR_Controller.Input((int)rightObj.index);
    }
	
	// Update is called once per frame
	void Update () {
        if (leftController == null)
            leftController = SteamVR_Controller.Input((int)leftObj.index);
        if (rightController == null)
            rightController = SteamVR_Controller.Input((int)rightObj.index);

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

        if (leftController.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            Debug.Log("L trigger pressed");
        if (rightController.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            Debug.Log("R trigger pressed");
	}
    
    public void OnSwipeLeft(SIDE side)
    {
        //if (MainSceneController.instance != null)
            //if (MainSceneController.instance.leftAttached)
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
                        leftSwipeTimer = 0f;
                    }
                }
            }
    }

    public void OnSwipeRight(SIDE side)
    {
        //if (MainSceneController.instance != null)
            //if (MainSceneController.instance.rightAttached)
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

    public void OnSwipeIn(FORWARD_SIDE side)
    {
        if(side == FORWARD_SIDE.FORWARD)
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

    public void OnSwipeOut(FORWARD_SIDE side)
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

    public void CheckForwardGesture()
    {
        if(swipingBackwardIn && swipingForwardIn && swipingForwardOut && swipingBackwardOut)
        {
            identifyGesture.PlayBobAnimation();
            swipingBackwardIn = swipingBackwardOut = swipingForwardIn = swipingForwardOut = false;
            backwardInSwipeTimer = backwardOutSwipeTimer = forwardInSwipeTimer = forwardOutSwipeTimer = 0f;
        }
    }
}
