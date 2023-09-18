using System.Collections;
using System.Collections.Generic;
using Godot;

public partial class LoadingScreenController : Node
{
    private static LoadingScreenController Instance;
    [Export] 
    private Panel darkScreen;
    [Export] 
    private Panel loadingScreen;
    [Export] 
    private float fadeLength;
    [Export] 
    private float waitTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public static void LoadScene(int sceneToLoad)
    {
        // Instance.StartCoroutine(Instance.SceneTransition(sceneToLoad));
    }

    IEnumerator SceneTransition(int sceneName)
    {
        // darkScreen.alpha = 0;
        darkScreen.Show();
        float timer = 0f;
        // while(timer < fadeLength)
        // {
            // timer = Mathf.Clamp(timer + Time.deltaTime, 0f, fadeLength);
            // darkScreen.alpha = timer / fadeLength;
            yield return null;
        // }
        loadingScreen.Show();
        timer = fadeLength;
        // while (timer > 0)
        // {
            // timer = Mathf.Clamp(timer - Time.deltaTime, 0f, fadeLength);
            // darkScreen.alpha = timer / fadeLength;
            // yield return null;
        // }
        darkScreen.Hide();
        // yield return new WaitForSeconds(waitTime);
        // var operation = SceneManager.LoadSceneAsync(sceneName);
        // while(!operation.isDone)
        // {
            // yield return null;
        // }
        // darkScreen.alpha = 0;
        darkScreen.Show();
        timer = 0f;
        // while (timer < fadeLength)
        // {
            // timer = Mathf.Clamp(timer + Time.deltaTime, 0f, fadeLength);
            // darkScreen.alpha = timer / fadeLength;
            // yield return null;
        // }
        loadingScreen.Hide();
        timer = fadeLength;
        // while (timer > 0)
        // {
            // timer = Mathf.Clamp(timer - Time.deltaTime, 0f, fadeLength);
            // darkScreen.alpha = timer / fadeLength;
            // yield return null;
        // }
        darkScreen.Hide();
    }
}
