using My;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IdentifyGesture : GestureHandler
{
    // Gesture index to use for training and verifying custom gesture. Valid range is between 1 and 1000
    // Beware that setting to 100 will overwrite your player signature.
    readonly int PLAYER_GESTURE_WALK = 101;
    readonly int PLAYER_GESTURE_SWIPE = 102;

    [Tooltip("Distance to travel but movement action")]
    public float moveDistance = 1f;

    [Header("Animation stuffs")]
    [Tooltip("Coloured_War_Robot")]
    public GameObject Robot;
    [Tooltip("Animator Component of Head in Camera. \nUnder Coloured_War_Robot")]
    public Animator HeadAnimator;
    [Tooltip("Animator Component of Left Arm in Bone015(mirrored). \nUnder Coloured_War_Robot -> Root -> robot_rotation_controller -> Jump_controller -> Torso_HUB -> connectBone002")]
    public Animator LeftArmAnimator;
    [Tooltip("Animator Component of Right Arm in Bone015. \nUnder Coloured_War_Robot -> Root -> robot_rotation_controller -> Jump_controller -> Torso_HUB -> connectBone003")]
    public Animator RightArmAnimator;
    [Tooltip("Animator Component of the cockpit")]
    public Animator CockpitAnimator;

    [Tooltip("Bob. Under Asset/Animation")]
    public AnimationClip HeadAnimationClip;
    [Tooltip("SwipeLeft. Under Asset/Animation")]
    public AnimationClip LeftSwipeAnimationClip;
    [Tooltip("SwipeRight. Under Asset/Animation")]
    public AnimationClip RightSwipeAnimationClip;
    [Tooltip("The cockpit bobbing animation")]
    public AnimationClip CockpitAnimationClip;
    public bool bobbing = false;
    public bool swipingLeft = false;
    public bool swipingRight = false;
    
    // Callback for receiving signature/gesture progression or identification results
    Manager.OnPlayerGestureMatch playerGestureMatch;

    // Handling custom gesture match callback - This is inovked when the Mode is set to Mode.IdentifyPlayerGesture and a gesture
    // is recorded.
    // gestureId - a serial number
    // match - the index that match or -1 if no match. The match index must be one in the SetTarget()
    void HandleOnPlayerGestureMatch(long gestureId, int match)
    {
        if (gestureId != 0 &&
            (MainSceneController.instance.leftAttached ||
            MainSceneController.instance.rightAttached))
        {
            if (PLAYER_GESTURE_WALK == match)
            {
                bobbing = true;
            }
            else if (PLAYER_GESTURE_SWIPE == match)
            {
                // Find Direction of the Swipe
                FindDirection();
                // Right
                if (directionResult == 1)
                {
                    swipingRight = true;
                }
                // Left
                else if(directionResult == -1)
                {
                    swipingLeft = true;
                }
            }
        }
    }

    // Use this for initialization
    void Awake()
    {
        Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);

        // Configure by specifying target 
        playerGestureMatch = new Manager.OnPlayerGestureMatch(HandleOnPlayerGestureMatch);
        //Manager.onPlayerGestureMatch += playerGestureMatch;

        // Loads gestures from DeveloperDefined.csv
        //Manager.LoadCache();

        // Sets current mode to Identify Player Gesture
        Manager.SetPlayerGesture(new List<int> {
                PLAYER_GESTURE_WALK,
                PLAYER_GESTURE_SWIPE
            }, true);
        Manager.SetMode(Manager.Mode.IdentifyPlayerGesture);
        Manager.SetTarget(new List<int> { PLAYER_GESTURE_WALK, PLAYER_GESTURE_SWIPE });

        Manager.SetTriggerStartKeys(
            Manager.Controller.RIGHT_HAND,
            SteamVR_Controller.ButtonMask.Trigger,
            Manager.PressOrTouch.PRESS);
        
        Manager.SetTriggerStartKeys(
            Manager.Controller.LEFT_HAND,
            SteamVR_Controller.ButtonMask.Trigger,
            Manager.PressOrTouch.PRESS);
    }

    void OnDestroy()
    {
        // Unregistering callback
        //Manager.onPlayerGestureMatch -= playerGestureMatch;
    }

    void Update()
    {
        UpdateUIandHandleControl();

        // if the name of current state of HeadAnimator is not "Bob", Play Bob animation
        if (bobbing)
        {
            PlayBobAnimation();
        }
        // if the name of current state of ArmAnimators is not "Swipe", Play Swipe animation
        else if (swipingLeft)
        {
            PlaySwipeLeftAnimation();
        }
        else if (swipingRight)
        {
            PlaySwipeRightAnimation();
        }
    }

    public void PlayBobAnimation()
    {
        if (!HeadAnimator.GetCurrentAnimatorStateInfo(0).IsName("Bob") && !LeftArmAnimator.GetCurrentAnimatorStateInfo(0).IsName("Swipe") && !RightArmAnimator.GetCurrentAnimatorStateInfo(0).IsName("Swipe"))
        {
            if (MainSceneController.instance.GetMovable())
            {
                if(MainSceneController.instance.state == MainSceneController.GAME_STATE.GAME)
                    ++MainSceneController.instance.movementIndex;
                // Robot Move Forward
                Vector3 StartPoint = Robot.transform.position;
                Vector3 EndPoint = new Vector3(Robot.transform.position.x,
                    Robot.transform.position.y,
                    Robot.transform.position.z + moveDistance);
                StartCoroutine(WaitAwhile(1.0f, StartPoint, EndPoint, 0.15f));

                // Head Animation
                HeadAnimator.Rebind();
                HeadAnimator.Play("Bob");
                CockpitAnimator.Rebind();
                CockpitAnimator.Play("Cockpit_Bob");
                SteamVR_Controller.Input((int)MainSceneController.instance.LeftController.GetComponent<SteamVR_TrackedObject>().index).TriggerHapticPulse(3900);
                SteamVR_Controller.Input((int)MainSceneController.instance.RightController.GetComponent<SteamVR_TrackedObject>().index).TriggerHapticPulse(3900);
                StartCoroutine(Bobbing());
            }
        }
    }

    public void PlaySwipeLeftAnimation()
    {
        if (!HeadAnimator.GetCurrentAnimatorStateInfo(0).IsName("Bob") && !LeftArmAnimator.GetCurrentAnimatorStateInfo(0).IsName("Swipe") && !RightArmAnimator.GetCurrentAnimatorStateInfo(0).IsName("Swipe"))
        {
            LeftArmAnimator.Rebind();
            LeftArmAnimator.Play("Swipe");
            SteamVR_Controller.Input((int)MainSceneController.instance.LeftController.GetComponent<SteamVR_TrackedObject>().index).TriggerHapticPulse(2000);
            StartCoroutine(SwipingLeft());
        }
    }

    public void PlaySwipeRightAnimation()
    {
        if (!HeadAnimator.GetCurrentAnimatorStateInfo(0).IsName("Bob") && !LeftArmAnimator.GetCurrentAnimatorStateInfo(0).IsName("Swipe") && !RightArmAnimator.GetCurrentAnimatorStateInfo(0).IsName("Swipe"))
        {
            RightArmAnimator.Rebind();
            RightArmAnimator.Play("Swipe");
            SteamVR_Controller.Input((int)MainSceneController.instance.RightController.GetComponent<SteamVR_TrackedObject>().index).TriggerHapticPulse(2000);
            StartCoroutine(SwipingRight());
        }
    }

    // Used to move Robot slowly towards TargetPos
    IEnumerator WaitAwhile(float TimeToWait, Vector3 CurrPos, Vector3 TargetPos, float TimeToReach)
    {
        yield return new WaitForSecondsRealtime(TimeToWait);
        StartCoroutine(MoveToPosition(CurrPos, TargetPos, TimeToReach));
    }
    IEnumerator MoveToPosition(Vector3 CurrPos, Vector3 TargetPos, float TimeToReach)
    {
        float t = 0f;
        
        while(t < 1)
        {
            t += Time.deltaTime / TimeToReach;
            Robot.transform.position = Vector3.Lerp(CurrPos, TargetPos, t);
            yield return null;
        }
    }
    // Used to stop Animations when they finish playing
    IEnumerator Bobbing()
    {
        yield return new WaitForSeconds(HeadAnimationClip.length);
        bobbing = false;
    }
    IEnumerator SwipingLeft()
    {
        yield return new WaitForSeconds(LeftSwipeAnimationClip.length);
        swipingLeft = false;
    }
    IEnumerator SwipingRight()
    {
        yield return new WaitForSeconds(RightSwipeAnimationClip.length);
        swipingRight = false;
    }
}