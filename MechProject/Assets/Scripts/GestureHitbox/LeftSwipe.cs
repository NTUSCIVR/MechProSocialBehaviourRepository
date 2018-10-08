using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SIDE
{
    LEFT,
    RIGHT
}

public class LeftSwipe : MonoBehaviour {

    public SIDE side;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(side == SIDE.LEFT)
        {

        }
        else
        {

        }
    }
}
