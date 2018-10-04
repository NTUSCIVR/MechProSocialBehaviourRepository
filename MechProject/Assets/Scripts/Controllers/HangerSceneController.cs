using UnityEngine;
using UnityEngine.UI;

public class HangerSceneController : MonoBehaviour
{

    public GameObject eyeLight;
    public Text TextObject;
    public string TextToShow = "This is a default sentence.";
    public float DisplayRate = 0.1f;

    private float timer = 0.0f;
    private string TextThatIsShowing = "";
    
	// Update is called once per frame
	void Update ()
    {
        if (TextToShow.Length >= 1)
        {
            timer += Time.deltaTime;
            if (timer >= DisplayRate)
            {
                // Robot Eye Blinks
                eyeLight.SetActive(!eyeLight.activeSelf);

                // Add First Character of TextToShow into the Text UI
                TextThatIsShowing += TextToShow[0];
                TextToShow = TextToShow.Remove(0, 1);
                TextObject.text = TextThatIsShowing;
                timer = 0.0f;
            }
        }
        else
            eyeLight.SetActive(false);
	}

    public void ChangeToNextScene()
    {

    }
}
