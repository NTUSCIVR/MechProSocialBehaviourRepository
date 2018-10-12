using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubberController : MonoBehaviour {

    bool startMoving = false;
    public SIDE side;
    public Vector3 forward;

    public float speed = 1f;
    public float distanceToTravel = 1f;
    float distanceTraveled = 0f;

    Vector3 left = Vector3.zero;
    Vector3 right = Vector3.zero;

    float delayBeforeMoving = 2f;
    float timer = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(startMoving)
        {
            if (timer > delayBeforeMoving)
            {
                if (side == SIDE.LEFT)
                {
                    if (left == Vector3.zero)
                    {
                        Quaternion leftRotation = Quaternion.AngleAxis(-90, Vector3.up);
                        left = leftRotation * forward;
                    }
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
        if (other.tag == "Arm")
        {
            startMoving = true;
            MainSceneController.instance.rubberPlacements[MainSceneController.instance.movementIndex] = SIDE.DEFAULT;
        }
    }
}
