using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderManager : MonoBehaviourSingleton<LoaderManager>
{
    public float loadingProgress;
    public float timeLoading;
    public float minTimeToLoad = 2;
    private bool loading = false;

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene("LoadingScreen");
        StartCoroutine(AsynchronousLoad(sceneName));
    }

    IEnumerator AsynchronousLoad(string scene)
    {
        loading = true;
        loadingProgress = 0;
        timeLoading = 0;
        yield return null;
        AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            timeLoading += Time.deltaTime;
            loadingProgress = ao.progress + 0.1f;
            loadingProgress = loadingProgress * timeLoading / minTimeToLoad;

            // Loading completed
            if (loadingProgress >= 1)
            {
                ao.allowSceneActivation = true;
            }
            loading = false;
            yield return null;
        }
    }
}