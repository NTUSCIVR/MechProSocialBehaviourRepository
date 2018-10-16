using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class SceneChangeController : MonoBehaviour {

    static public SceneChangeController instance;

    [SerializeField]
    float fadeTime = 1f;
    float fadeTimer = 0f;

    [SerializeField]
    Image fadeOutImage;
    
    bool fadingIn = false;
    bool fadingOut = false;

    string nextScene;

	// Use this for initialization
	void Start () {
        instance = this;
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        if(fadingOut)
            FadeOut();
        if (fadingIn)
            if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName(nextScene))
                FadeIn();
	}

    void FadeOut()
    {
        if(fadeTimer <= fadeTime)
        {
            fadeTimer += Time.deltaTime;
            fadeOutImage.color = new Color(0, 0, 0, fadeTimer / fadeTime);
        }
        else
        {
            fadeOutImage.color = new Color(0, 0, 0, 1);
            fadeTimer = 0f;
            fadingOut = false;
            fadingIn = true;
            SceneManager.LoadSceneAsync(nextScene);
        }
    }

    void FadeIn()
    {
        fadeOutImage = GameObject.FindWithTag("Fade").GetComponentInChildren<Image>();
        GameObject.FindWithTag("Fade").transform.SetParent(GameObject.FindWithTag("MainCamera").transform);
        if(fadeTimer <= fadeTime)
        {
            fadeTimer += Time.deltaTime;
            fadeOutImage.color = new Color(0, 0, 0, 1 - fadeTimer / fadeTime);
        }
        else
        {
            fadeOutImage.color = new Color(0, 0, 0, 0);
            fadeTimer = 0f;
            fadingIn = false;
        }
    }

    public void ChangeScene(string sceneName)
    {
        //check if scene exists
        if (SceneManager.GetSceneByName(sceneName) != null)
        {
            fadingOut = true;
            nextScene = sceneName;
        }
        else
            Debug.LogWarning(sceneName + " does not exist");
    }
}
