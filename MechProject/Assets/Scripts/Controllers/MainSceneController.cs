﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//rip the code so dependent on each other i cant do much alr, sorry next intern peep
public class MainSceneController : MonoBehaviour {

    public enum GAME_STATE
    {
        TUTORIAL,
        GAME
    }

    public static MainSceneController instance;

    public GameObject LeftController;
    public GameObject RightController;

    public GameObject blueCirclePrefab;

    public bool leftAttached = false;
    public bool rightAttached = false;

    public Transform robotStartTransform;
    public IdentifyGesture identifyGesture;

    [Tooltip("List of objects that are placed on the left or right in a straight line, the smaller the index the closer to the start it is")]
    public List<SIDE> rubberPlacements;
    [Tooltip("Prefabs for creating the rubber objects in random")]
    public List<GameObject> rubberPrefabs;

    //how many times the robot has moved
    public int movementIndex = 0;

    GAME_STATE state = GAME_STATE.TUTORIAL;
    public bool moveBefore = false;
    public bool swipeLeftBefore = false;
    public bool swiperRightBefore = false;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
		for(int i = 0; i < rubberPlacements.Count; ++i)
        {
            if (rubberPlacements[i] != SIDE.DEFAULT)
            {
                GameObject rubber = Instantiate(rubberPrefabs[Random.Range(0, rubberPrefabs.Count - 1)]);
                Vector3 newPosition = robotStartTransform.position + robotStartTransform.forward * identifyGesture.moveDistance * i;
                if (rubberPlacements[i] == SIDE.LEFT)
                {
                    newPosition -= robotStartTransform.right;
                    rubber.GetComponent<RubberController>().side = SIDE.LEFT;
                }
                else if (rubberPlacements[i] == SIDE.RIGHT)
                {
                    newPosition += robotStartTransform.right;
                    rubber.GetComponent<RubberController>().side = SIDE.RIGHT;
                }
                newPosition += robotStartTransform.forward * 3;
                rubber.transform.position = newPosition;
                rubber.transform.Rotate(Vector3.up, Random.Range(0, 360));
                rubber.GetComponent<RubberController>().forward = robotStartTransform.forward;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AttachRing(GameObject go)
    {
        Instantiate(blueCirclePrefab, go.transform);
    }

    public bool GetMovable()
    {
        if (rubberPlacements[movementIndex] == SIDE.DEFAULT)
            return true;
        return false;
    }
}
