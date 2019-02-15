using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubberController : MonoBehaviour {

    //boolean for the rubber to start moving
    bool startMoving = false;
    //the side of road the rubber will be on, left, right and default being no rubber
    public SIDE side;
    //the forward vector of the robot
    public Vector3 forward;

    //how fast the rubber will move
    public float speed = 1f;
    //how far the rubber shld move
    public float distanceToTravel = 1f;
    //how far the rubber traveled
    float distanceTraveled = 0f;

    Vector3 left = Vector3.zero;
    Vector3 right = Vector3.zero;

    public float delayBeforeMoving = 2f;
    float timer = 0f;
	
	// Update is called once per frame
	void Update () {
		if(startMoving)
        {
            if (timer > delayBeforeMoving)
            {
                if (side == SIDE.LEFT)
                {
                    //rotate the forward vector to the left
                    if (left == Vector3.zero)
                    {
                        Quaternion leftRotation = Quaternion.AngleAxis(-90, Vector3.up);
                        left = leftRotation * forward;
                    }
                    //move it left
                    transform.position += left * speed * Time.deltaTime;
                    distanceTraveled += left.magnitude * speed * Time.deltaTime;
                    if (distanceTraveled > distanceToTravel)
                    {
                        startMoving = false;
                    }

                }
                else
                {
                    if (right == Vector3.zero)
                    {
                        Quaternion rightRotation = Quaternion.AngleAxis(90, Vector3.up);
                        right = rightRotation * forward;
                    }
                    transform.position += right * speed * Time.deltaTime;
                    distanceTraveled += right.magnitude * speed * Time.deltaTime;
                    if (distanceTraveled > distanceToTravel)
                    {
                        startMoving = false;
                    }
                }
            }
            else
                timer += Time.deltaTime;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if(side == SIDE.LEFT)
        {
            if (other.tag == "LeftArm")
            {
                //if it a arm triggers it, start moving
                startMoving = true;
                MainSceneController.instance.rubberPlacements[MainSceneController.instance.movementIndex] = SIDE.DEFAULT;
            }
        }
        else
        {
            if (other.tag == "RightArm")
            {
                //if it a arm triggers it, start moving
                startMoving = true;
                MainSceneController.instance.rubberPlacements[MainSceneController.instance.movementIndex] = SIDE.DEFAULT;
            }
        }
        
    }
}
