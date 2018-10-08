using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;

public class GestureHitboxController : MonoBehaviour {

    public static GestureHitboxController instance;
    [Tooltip("Reference to IdentifyGesture script in scene")]
    public IdentifyGesture identifyGesture; 
    [Tooltip("Time needed for gesture to get recognized")]
    public float swipeDelay = 1f;

    public bool swipingLeft = false;
    public float leftSwipeTimer = 0f;

    public bool swipingRight = false;
    public float rightSwipeTimer = 0f;

    public SteamVR_TrackedObject leftObj;
    public SteamVR_TrackedObject rightObj;

    SteamVR_Controller.Device leftController;
    SteamVR_Controller.Device rightController;

    public int leftIndex;
    public int rightIndex;

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

        if(leftController.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
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
}
