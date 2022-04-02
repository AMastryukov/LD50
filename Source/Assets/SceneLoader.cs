using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    
    [SerializeField] private Animator transition;
    private float transitionTime = 1f;
    
    
    public void ChangeScene(int levelIndex)
    {
        StartCoroutine(LoadScene(levelIndex));
    }

    
    private IEnumerator LoadScene(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
