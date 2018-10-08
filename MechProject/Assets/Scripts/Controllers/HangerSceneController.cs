using UnityEngine;
using UnityEngine.UI;

public class HangerSceneController : MonoBehaviour
{
    public GameObject eyeLight;
    public Canvas CanvasObject;
    public string TextToShow = "This is a default sentence one.This is a default sentence two.This is a default sentence three";
    public float DisplayRate = 1.0f;
    public char[] Delimiters = { '.' };

    private Text TextObject;
    private float timer = 0.0f;
    private string[] TextsThatAreShowing;
    private int SentenceCounter = 1;
    private bool EndOfShowing = false;

    private void Start()
    {
        // Get Text Under Canvas
        TextObject = CanvasObject.transform.Find("Text").GetComponent<Text>();
        // Split TextToShow with Delimiters into strings
        TextsThatAreShowing = TextToShow.Split(Delimiters);

        // Robot Eye Shine
        eyeLight.SetActive(true);
    }

    // Update is called once per frame
    void Update ()
    {
        // Stop Running Update if finish showing
        if (EndOfShowing)
            return;

        timer += Time.deltaTime;
        if (timer >= DisplayRate)
        {
            // Only try to show when Counter is within valid range
            if (SentenceCounter <= TextsThatAreShowing.Length)
            {
                // Replaces the texts
                TextObject.text = TextsThatAreShowing[SentenceCounter - 1];
                TextObject.text += ".";
                // Increment Counter & Reset Timer 
                SentenceCounter++;
                timer = 0.0f;
            }
            else
            {
                // Finish Showing texts
                CanvasObject.gameObject.SetActive(false);
                EndOfShowing = true;
            }
        }
	}

    public void ChangeToNextScene()
    {

    }
}
