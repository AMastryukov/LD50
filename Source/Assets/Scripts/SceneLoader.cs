using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    
    [SerializeField] private Animator transition;
    private float transitionTime = 1f;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// Change scene with a smooth fading transition
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    public IEnumerator ChangeScene(string sceneName)
    {
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            // TODO: Eelis can you also make it fade back in?
            //transition.SetTrigger("Start");

            yield return new WaitForSeconds(transitionTime);

            yield return WaitForLoadScene(sceneName);
        }

        yield return null;
    }

    /// <summary>
    /// Use this to wait till the scene has been loaded
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    private IEnumerator WaitForLoadScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        if (asyncLoad == null)
        {
            Debug.LogError("asyncLoad is null");
        }

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
