using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbulanceController : MonoBehaviour {

    bool toDrive = false;

    public float distanceToTravel;
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
            if(MainSceneController.instance.movementIndex == MainSceneController.instance.rubberPlacements.Count - 1)
            {
                toDrive = true;
                GetComponent<AudioSource>().Play();
            }
        }
        else
        {
            if ((startPos - transform.position).magnitude < distanceToTravel)
                transform.position += transform.up * speed * Time.deltaTime;
            else
            {
                Debug.Log("Change scene to EndScene");
                SceneChangeController.instance.ChangeScene("EndScene");
                Destroy(this);
            }
        }
	}
}
