using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    public GameObject mainCamera;
    Vector3 startWorldPosition;
    float y;

	// Use this for initialization
	void Start () {
        startWorldPosition = transform.position;
        y = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 newPos = new Vector3(mainCamera.transform.position.x, y, mainCamera.transform.position.z);
        transform.position = newPos;

        //adjust height oso cuz i lazy
        if(Input.GetKeyDown(KeyCode.Return))
        {
            y = mainCamera.transform.position.y;
        }
	}
}
