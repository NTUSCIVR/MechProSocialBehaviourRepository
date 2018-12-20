using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<SpriteRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
<<<<<<< HEAD
        if (MainSceneController.instance.movementIndex < MainSceneController.instance.rubberPlacements.Count && MainSceneController.instance.rubberPlacements[MainSceneController.instance.movementIndex] != SIDE.DEFAULT)
        {
=======
        if (MainSceneController.instance.rubberPlacements[MainSceneController.instance.movementIndex] != SIDE.DEFAULT)
>>>>>>> parent of 25c1222... File Changes
            GetComponent<SpriteRenderer>().enabled = true;
        else
            GetComponent<SpriteRenderer>().enabled = false;
	}
}
