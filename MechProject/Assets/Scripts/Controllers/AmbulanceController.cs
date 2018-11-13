using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbulanceController : MonoBehaviour {

    bool toDrive = false;

    [Tooltip("Distance the ambulance will travel")]
    public float distanceToTravel;
    [Tooltip("Speed of ambulance in u/s")]
    public float speed;
    Vector3 startPos;

	// Use this for initialization
	void Start () {
        startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(!toDrive)
        {
            //if the robot reaches the final place, then will allow the ambulances to move
            if(MainSceneController.instance.movementIndex == MainSceneController.instance.rubberPlacements.Count)
            {
                toDrive = true;
                //start playing the ambulance audios
                GetComponent<AudioSource>().Play();
            }
        }
        else
        {
            //if the ambulance havent went past its designated distance to go
            if ((startPos - transform.position).magnitude < distanceToTravel)
                transform.position += transform.up * speed * Time.deltaTime;
            else
            {
                Debug.Log("Change scene to EndScene");
                //tell mainscenecontroller to record down time taken to finish this scene
                MainSceneController.instance.EndScene();
                //should display some kind of a end text
                SceneChangeController.instance.ChangeScene("EndScene");
                Destroy(this);
            }
        }
	}
}
