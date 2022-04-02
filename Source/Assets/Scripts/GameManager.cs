using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private bool FirstEvidenceFound;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        StartCoroutine(GameSequence());
    }

    private IEnumerator GameSequence()
    {
        Debug.Log("GameSequence");
        yield return IntroSequence();

        yield return Sequence1();
        //yield return Sequence2();
        //yield return Sequence3();

        yield return GameWinSequence();

        yield return null;
    }

    private IEnumerator IntroSequence()
    {
        Debug.Log("IntroSequence");
        yield return null;
    }

    private IEnumerator Sequence1()
    {
        HashSet<EvidenceKey> evidenceFound = new HashSet<EvidenceKey>();
        Debug.Log("Sequence1");
        //TODO: Add event delegate for evidence being found

        while (evidenceFound.Count < 3)
        {
            yield return CrimeScene1();
            yield return Interrogation1();
        }

        yield return null;
    }

    private IEnumerator CrimeScene1()
    {
        Debug.Log("CrimeScene1");
        SceneManager.LoadScene("CrimeScene1");

        bool leaveScene = false;
        //TODO: Add event delegate for leave event

        while (!leaveScene)
        {
            yield return null;
        }

        yield return null;
    }

    private IEnumerator Interrogation1()
    {
        Debug.Log("Interrogation1");
        SceneManager.LoadScene("GameLoopTest 1");

        bool leaveInterrogation = false;
        //TODO: Add event delegate for leave event

        while (!leaveInterrogation)
        {
            yield return null;
        }

        yield return null;
    }

    private IEnumerator GameWinSequence()
    {
        yield return null;
    }


}
