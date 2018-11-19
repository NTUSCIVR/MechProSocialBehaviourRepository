using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientAtStart : MonoBehaviour {

    public Transform trans;

	// Use this for initialization
	void Start () {
        Orient();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Orient()
    {
        Vector3 newPosition = new Vector3(trans.position.x - 2, trans.position.y, trans.position.z);
        Vector3 newEulerRotation = new Vector3(0, trans.rotation.eulerAngles.y, 0);
        transform.position = newPosition;
        transform.rotation = Quaternion.Euler(newEulerRotation);
    }
}
