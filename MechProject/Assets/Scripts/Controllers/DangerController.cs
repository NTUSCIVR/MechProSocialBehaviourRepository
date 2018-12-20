using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerController : MonoBehaviour {

    public GameObject text;

	// Use this for initialization
	void Start () {
        GetComponent<SpriteRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (MainSceneController.instance.movementIndex < MainSceneController.instance.rubberPlacements.Count && MainSceneController.instance.rubberPlacements[MainSceneController.instance.movementIndex] != SIDE.DEFAULT)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            text.SetActive(true);
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = false;
            text.SetActive(false);
        }
	}
}
