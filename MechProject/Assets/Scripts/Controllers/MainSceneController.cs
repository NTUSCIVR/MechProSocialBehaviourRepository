using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneController : MonoBehaviour {

    public static MainSceneController instance;

    public GameObject LeftController;
    public GameObject RightController;

    public GameObject blueCirclePrefab;

    public bool leftAttached = false;
    public bool rightAttached = false;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AttachRing(GameObject go)
    {
        Instantiate(blueCirclePrefab, go.transform);
    }
}
