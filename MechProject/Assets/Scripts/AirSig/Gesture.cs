using System.Collections.Generic;
using UnityEngine;
using System.IO;

using My;

public class Gesture : GestureHandler
{
    // Gesture index to use for training and verifying custom gesture. Valid range is between 1 and 1000
    // Beware that setting to 100 will overwrite your player signature.
    readonly int PLAYER_GESTURE_WALK = 101;
    readonly int PLAYER_GESTURE_LEFT = 102;
    readonly int PLAYER_GESTURE_RIGHT = 103;

    readonly string GESTURE_WALK = "<color=#FF00FF>Gesture #WALK</color>";
    readonly string GESTURE_LEFT = "<color=yellow>Gesture #LEFT</color>";
    readonly string GESTURE_RIGHT = "<color=#00FFFF>Gesture #RIGHT</color>";

    // How many gesture we need to collect for each gesture type
    readonly int MAX_TRAIN_COUNT = 12;

    // Use these steps to iterate gesture when train 'Smart Train' and 'Custom Gesture'
    int currentPlayerGestureTarget; // 101 = GESTURE_WALK, 102 = GESTURE_LEFT, 103 = GESTURE_RIGHT

    bool hasSetupGestureOne = false;
    bool hasSetupGestureTwo = false;
    bool hasSetupGestureThree = false;

    // Callback for receiving signature/gesture progression or identification results
    Manager.OnPlayerGestureMatch playerGestureMatch;
    Manager.OnPlayerGestureAdd playerGestureAdd;

    // Handling custom gesture match callback - This is inovked when the Mode is set to Mode.IdentifyPlayerGesture and a gesture
    // is recorded.
    // gestureId - a serial number
    // match - the index that match or -1 if no match. The match index must be one in the SetTarget()
    void HandleOnPlayerGestureMatch(long gestureId, int match)
    {
        if (gestureId == 0)
        {

        }
        else
        {
            string result = "<color=red>Cannot find closest custom gesture</color>";
            if (PLAYER_GESTURE_WALK == match)
            {
                result = string.Format("<color=#FF00FF>Closest Custom Gesture Gesture #WALK</color>");
            }
            else if (PLAYER_GESTURE_LEFT == match)
            {
                result = string.Format("<color=yellow>Closest Custom Gesture Gesture #LEFT</color>");
            }
            else if (PLAYER_GESTURE_RIGHT == match)
            {
                result = string.Format("<color=#00FFFF>Closest Custom Gesture Gesture #RIGHT</color>");
            }

            // Check whether this gesture match any custom gesture in the database
            float[] data = Manager.GetFromCache(gestureId);
            bool isExisted = Manager.IsPlayerGestureExisted(data);
            result += isExisted ? string.Format("\n<color=green>There is a similar gesture in DB!</color>") :
                string.Format("\n<color=red>There is no similar gesture in DB!</color>");

            textToUpdate = result;
        }
    }

    // Handling custom gesture adding callback - This is invoked when the Mode is set to Mode.AddPlayerGesture and a gesture is
    // recorded. Gestures are only added to a cache. You should call SetPlayerGesture() to actually set gestures to database.
    // gestureId - a serial number
    // result - return a map of all un-set custom gestures and number of gesture collected.
    void HandleOnPlayerGestureAdd(long gestureId, Dictionary<int, int> result)
    {
        int count = result[currentPlayerGestureTarget];

        // Set color command and gesture string for textToUpdate
        string color = "";
        string gesture = "";
        if (currentPlayerGestureTarget == PLAYER_GESTURE_WALK)
        {
            color = "<color=#FF00FF>";
            gesture = "Gesture #WALK";
        }
        else if (currentPlayerGestureTarget == PLAYER_GESTURE_LEFT)
        {
            color = "<color=yellow>";
            gesture = "Gesture #LEFT";
        }
        else if (currentPlayerGestureTarget == PLAYER_GESTURE_RIGHT)
        {
            color = "<color=#00FFFF>";
            gesture = "Gesture #RIGHT";
        }

        textToUpdate = string.Format("{0}{1}/{2} gesture(s) collected for {3}\nContinue to collect more samples</color>",
            color, count, MAX_TRAIN_COUNT, gesture);

        if (count >= MAX_TRAIN_COUNT && currentPlayerGestureTarget == PLAYER_GESTURE_WALK)
        {
            currentPlayerGestureTarget++;
            textToUpdate = null; // UI will be handled by next UI action
            nextUiAction = () => {
                StopCoroutine(uiFeedback);
                EnterGesture(PLAYER_GESTURE_LEFT);
                hasSetupGestureOne = true;
            };
        }
        else if (count >= MAX_TRAIN_COUNT && currentPlayerGestureTarget == PLAYER_GESTURE_LEFT)
        {
            currentPlayerGestureTarget++;
            textToUpdate = null; // UI will be handled by next UI action
            nextUiAction = () => {
                StopCoroutine(uiFeedback);
                EnterGesture(PLAYER_GESTURE_RIGHT);
                hasSetupGestureTwo = true;
            };
        }
        else if (count >= MAX_TRAIN_COUNT && currentPlayerGestureTarget >= PLAYER_GESTURE_RIGHT)
        {
            textToUpdate = null; // UI will be handled by next UI action
            nextUiAction = () => {
                SwitchToIdentify();
                Save();
                hasSetupGestureThree = true;
            };
        }
        else
        {
            Manager.SetTarget(new List<int> { currentPlayerGestureTarget });
        }
    }

    void SwitchToIdentify()
    {
        StopCoroutine(uiFeedback);
        Manager.SetPlayerGesture(new List<int> {
                PLAYER_GESTURE_WALK,
                PLAYER_GESTURE_LEFT,
                PLAYER_GESTURE_RIGHT
            }, true);
        textResult.text = defaultResultText = string.Format("Write gestures you just trained\nin AddPlayerGesture.\nPress the Application key to reset");
        textMode.text = string.Format("Mode: {0}", Manager.Mode.IdentifyPlayerGesture.ToString());
        Manager.SetMode(Manager.Mode.IdentifyPlayerGesture);
        Manager.SetTarget(new List<int> { PLAYER_GESTURE_WALK, PLAYER_GESTURE_LEFT, PLAYER_GESTURE_RIGHT });
    }

    void Save()
    {
        // Delete if found Existing file
        if (File.Exists(Manager.datapath))
        {
            File.Delete(Manager.datapath);
        }
        // Create new file & Open it
        StreamWriter output = File.CreateText(Manager.datapath);
        // Write Data Header
        output.WriteLine("ID, Data[]");
        // Write down all recorded gestures
        foreach (var s in Manager.lines)
        {
            output.WriteLine(s);
        }
        // Close file
        output.Close();
    }

    void EnterGesture(int target)
    {
        // Set up string for textResult
        string gestureTarget = "";
        if (target == PLAYER_GESTURE_WALK)
            gestureTarget = GESTURE_WALK;
        else if (target == PLAYER_GESTURE_LEFT)
            gestureTarget = GESTURE_LEFT;
        else if (target == PLAYER_GESTURE_RIGHT)
            gestureTarget = GESTURE_RIGHT;

        textResult.text = defaultResultText = string.Format("Think of a gesture\nWrite it 12 times~\n{0}",
            gestureTarget);
        textMode.text = string.Format("Mode: {0}", Manager.Mode.AddPlayerGesture.ToString());
        Manager.SetMode(Manager.Mode.AddPlayerGesture);
        Manager.SetTarget(new List<int> { target });
        currentPlayerGestureTarget = target;

        if (hasSetupGestureOne)
        {
            Debug.Log("Delete Gesture Walk");
            Manager.DeletePlayerRecord(PLAYER_GESTURE_WALK);
            hasSetupGestureOne = false;
        }
        if (hasSetupGestureTwo)
        {
            Debug.Log("Delete Gesture Left");
            Manager.DeletePlayerRecord(PLAYER_GESTURE_LEFT);
            hasSetupGestureTwo = false;
        }
        if (hasSetupGestureThree)
        {
            Debug.Log("Delete Gesture Right");
            Manager.DeletePlayerRecord(PLAYER_GESTURE_RIGHT);
            hasSetupGestureThree = false;
        }
    }

    // Use this for initialization
    void Awake()
    {
        Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);

        // Update the display text
        textResult.alignment = TextAnchor.UpperCenter;

        // Configure by specifying target 
        playerGestureAdd = new Manager.OnPlayerGestureAdd(HandleOnPlayerGestureAdd);
        Manager.onPlayerGestureAdd += playerGestureAdd;
        playerGestureMatch = new Manager.OnPlayerGestureMatch(HandleOnPlayerGestureMatch);
        Manager.onPlayerGestureMatch += playerGestureMatch;

        EnterGesture(PLAYER_GESTURE_WALK);

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
        Manager.onPlayerGestureAdd -= playerGestureAdd;
        Manager.onPlayerGestureMatch -= playerGestureMatch;
    }

    void Update()
    {
        UpdateUIandHandleControl();

        if (-1 != (int)leftHandControl.index)
        {
            var device = SteamVR_Controller.Input((int)leftHandControl.index);
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu))
            {
                EnterGesture(PLAYER_GESTURE_WALK);
                Manager.lines.Clear();
            }
        }
        if (-1 != (int)rightHandControl.index)
        {
            var device = SteamVR_Controller.Input((int)rightHandControl.index);
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu))
            {
                EnterGesture(PLAYER_GESTURE_WALK);
                Manager.lines.Clear();
            }
        }
    }
}