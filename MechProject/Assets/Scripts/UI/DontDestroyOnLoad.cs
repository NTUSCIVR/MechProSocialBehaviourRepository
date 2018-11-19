using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//only purpose is to give objects dont destroy on load if needed
public class DontDestroyOnLoad : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
