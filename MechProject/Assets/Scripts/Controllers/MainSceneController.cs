using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//rip the code so dependent on each other i cant do much alr, sorry next intern peep
public class MainSceneController : MonoBehaviour {

    public enum GAME_STATE
    {
        TUTORIAL,
        GAME
    }

    public static MainSceneController instance;

    public GameObject LeftController;
    public GameObject RightController;

    public GameObject blueCirclePrefab;

    public bool leftAttached = false;
    public bool rightAttached = false;

    public Transform robotStartTransform;
    public GameObject blueBot;
    public RenderTexture blueBotEye;
    public GameObject redBot;
    public RenderTexture redBotEye;
    public GameObject cockpitDisplay;
    GameObject primaryBot;
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

    public float timeForTransition = 1f;
    float timerForTransition = 0f;
    public float fadeTime = 1f;
    float fadeTimer = 0f;
    bool faded = false;
    bool stopFade = false;

    Image fadeImage;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
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
                newPosition += robotStartTransform.forward * 3;
                rubber.transform.position = newPosition;
                rubber.transform.Rotate(Vector3.up, Random.Range(0, 360));
                rubber.GetComponent<RubberController>().forward = robotStartTransform.forward;
            }
        }
        fadeImage = GameObject.FindWithTag("Fade").GetComponentInChildren<Image>();
	}
	
	// Update is called once per frame
	void Update () {
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
                        Debug.Log("fading :" + fadeTimer);
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
                            Debug.Log("unfading: " + fadeTimer);
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
	}

    public void AttachRing(GameObject go)
    {
        Instantiate(blueCirclePrefab, go.transform);
    }

    public bool GetMovable()
    {
        if (rubberPlacements[movementIndex] == SIDE.DEFAULT)
            return true;
        return false;
    }
}
