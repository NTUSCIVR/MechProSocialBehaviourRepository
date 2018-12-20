using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

 
public class MainSceneController : MonoBehaviour {

    //the 2 states of main scene, the tutorial for the user and the game
    public enum GAME_STATE
    {
        TUTORIAL,
        GAME,
        END
    }

    public static MainSceneController instance;

    //both controllers
    public GameObject LeftController;
    public GameObject RightController;

    //the ring for after it has been attached to the player
    public GameObject blueCirclePrefab;
    public GameObject redCirclePrefab;

    //boolean for whether the player has attached his left and right hand
    public bool leftAttached = false;
    public bool rightAttached = false;

    //the starting transform of the robot
    public Transform robotStartTransform;
    //the blue mech
    public GameObject blueBot;
    //the rendertexture for the camera of the blue mech
    public RenderTexture blueBotEye;
    //the red mech
    public GameObject redBot;
    //the rendertexture for the camera of the red mech
    public RenderTexture redBotEye;
    //the quad responsible for showing the rendertextures of the mech
    public GameObject cockpitDisplay;
    //reference to the current bot used in the scenario
    GameObject primaryBot;
    //identify gesture script for running animations
    public IdentifyGesture identifyGesture;

    [Tooltip("List of objects that are placed on the left or right in a straight line, the smaller the index the closer to the start it is")]
    public List<SIDE> rubberPlacements;
    [Tooltip("Prefabs for creating the rubber objects in random")]
    public List<GameObject> rubberPrefabs;

    //how many times the robot has moved
    public int movementIndex = 0;

    public GAME_STATE state = GAME_STATE.TUTORIAL;
    //tutorial booleans, teach to swipe left -> swipe right -> move forward
    public bool moveBefore = false;
    public bool swipeLeftBefore = false;
    public bool swipeRightBefore = false;

    //some timers
    public float timeForTransition = 1f;
    float timerForTransition = 0f;
    public float fadeTime = 1f;
    float fadeTimer = 0f;
    bool faded = false;
    bool stopFade = false;

    float timer = 0f;

    //just a black image to manipulate alpha for fade effect
    Image fadeImage;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        //setting the used identify gesture to the respective scenarios
        //also activate the correct robot for use in the scene
        if(DataCollector.Instance.scenario == DataCollector.PROJECT_CASE.BLUE_NO_PERSUADE_PILOT_BLUE || DataCollector.Instance.scenario == DataCollector.PROJECT_CASE.BLUE_PERSUADE_PILOT_BLUE)
        {
            identifyGesture = blueBot.GetComponent<IdentifyGesture>();
            Debug.Log("Setting identify gesture to blue bot's");
            GetComponent<GestureHitboxController>().identifyGesture = blueBot.GetComponent<IdentifyGesture>();
            Debug.Log("Setting gesture hitbox controller to use blue bot's identify gestures");
            blueBot.SetActive(true);
            Debug.Log("Enabling blue bot");
            redBot.SetActive(false);
            Debug.Log("Disabling red bot");
            primaryBot = blueBot;
            Debug.Log("Setting primary bot to blue bot");
            cockpitDisplay.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", blueBotEye);
            Debug.Log("Assigning cockpit display render texture");
        }
        else
        {
            identifyGesture = redBot.GetComponent<IdentifyGesture>();
            Debug.Log("Setting identify gesture to red bot's");
            GetComponent<GestureHitboxController>().identifyGesture = redBot.GetComponent<IdentifyGesture>();
            Debug.Log("Setting gesture hitbox controller to use red bot's identify gestures");
            redBot.SetActive(true);
            Debug.Log("Enabling red bot");
            blueBot.SetActive(false);
            Debug.Log("Disabling blue bot");
            primaryBot = redBot;
            Debug.Log("Setting primary bot to red bot");
            cockpitDisplay.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", redBotEye);
            Debug.Log("Assigning cockpit display render texture");
        }
        //creation of the rubbers infront of the start robot transfer
        //this is based on the list of enums in the inspector
		for(int i = 0; i < rubberPlacements.Count; ++i)
        {
            if (rubberPlacements[i] != SIDE.DEFAULT)
            {
                GameObject rubber = Instantiate(rubberPrefabs[Random.Range(0, rubberPrefabs.Count - 1)]);
                Vector3 newPosition = robotStartTransform.position + robotStartTransform.forward * identifyGesture.moveDistance * i;
                if (rubberPlacements[i] == SIDE.LEFT)
                {
                    newPosition -= robotStartTransform.right * 2;
                    rubber.GetComponent<RubberController>().side = SIDE.LEFT;
                }
                else if (rubberPlacements[i] == SIDE.RIGHT)
                {
                    newPosition += robotStartTransform.right * 2;
                    rubber.GetComponent<RubberController>().side = SIDE.RIGHT;
                }
                //if the side is default, dont place anything down
                newPosition += robotStartTransform.forward * 3;
                rubber.transform.position = newPosition;
                //randomize the rotation of the debris
                rubber.transform.Rotate(Vector3.up, Random.Range(0, 360));
                rubber.GetComponent<RubberController>().forward = robotStartTransform.forward;
            }
        }
        fadeImage = GameObject.FindWithTag("Fade").GetComponentInChildren<Image>();

        if(movementIndex == rubberPlacements.Count - 1)
        {
            state = GAME_STATE.END;
        }
	}
	
	// Update is called once per frame
	void Update () {
        //timer for how long it takes to transition to from tutorial to game
        //it also fades and unfades the image
        timer += Time.deltaTime;
        if (moveBefore)
        {
            timerForTransition += Time.deltaTime;
            if (timerForTransition > timeForTransition)
            {
                //check if it is time to fade to teleport the mech somewhr else
                if (timerForTransition >= (timeForTransition - fadeTime))
                {
                    if (!faded)
                    {
                        fadeTimer += Time.deltaTime;
                       // Debug.Log("fading :" + fadeTimer);
                        if (fadeTimer < fadeTime)
                            fadeImage.color = new Color(0, 0, 0, fadeTimer / fadeTime);
                        else
                        {
                            fadeImage.color = new Color(0, 0, 0, 1);
                            fadeTimer = 1f;
                            faded = true;
                            primaryBot.transform.position = robotStartTransform.position;
                        }
                    }
                    else
                    {
                        if (!stopFade)
                        {
                            fadeTimer -= Time.deltaTime;
                            if (fadeTimer > 0)
                                fadeImage.color = new Color(0, 0, 0, fadeTimer / fadeTime);
                            else
                            {
                                fadeImage.color = new Color(0, 0, 0, 0);
                                state = GAME_STATE.GAME;
                                stopFade = true;
                            }
                        }
                    }
                }
            }
        }

        //restart the game
        if(state == GAME_STATE.END)
        {
            if(Input.GetKey(KeyCode.Escape))
            {
                Destroy(DataCollector.Instance.gameObject);
                Destroy(SceneChangeController.instance.gameObject);
                UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");
            }
        }
	}

    public void AttachRing(GameObject go)
    {
        //attach the rings to the user hands
        //blue bot got blue ring, red bot got red ring
        if (DataCollector.Instance.scenario == DataCollector.PROJECT_CASE.BLUE_NO_PERSUADE_PILOT_BLUE
            || DataCollector.Instance.scenario == DataCollector.PROJECT_CASE.BLUE_PERSUADE_PILOT_BLUE)
        {
            Instantiate(blueCirclePrefab, go.transform);
        }
        else
            Instantiate(redCirclePrefab, go.transform);
    }

    public bool GetMovable()
    {
        //check if the rubber has been moved
        //if the rubber has been moved, user can move forward
        //SIDE.DEFAULT is also the state for no rubber
        if (rubberPlacements[movementIndex] == SIDE.DEFAULT)
            return true;
        return false;
    }

    public void EndScene()
    {
        //just put in the time taken to finish the tutorial and game scene
        DataCollector.Instance.PushData("Time Taken for user to finish: " + timer.ToString() + "s");
    }
}
