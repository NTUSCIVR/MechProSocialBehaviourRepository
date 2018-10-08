using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureHitboxController : MonoBehaviour {

    public static GestureHitboxController instance;
    [Tooltip("Reference to IdentifyGesture script in scene")]
    public IdentifyGesture identifyGesture; 
    [Tooltip("Time needed for gesture to get recognized")]
    public float swipeDelay = 1f;

    bool swipingLeft = false;
    float leftSwipeTimer = 0f;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    void OnSwipeLeft(SIDE side)
    {
        if(side == SIDE.LEFT)
        {
            swipingLeft = true;
        }
        else
        {
            if(swipingLeft)
            {

            }
        }
    }
}
