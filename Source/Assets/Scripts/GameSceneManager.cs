using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Non-persistant game scene manager that fires events telling the persistent game manager to change state
/// </summary>
public class GameSceneManager : MonoBehaviour
{
    public delegate void LeaveSceneEventHandler();
    public delegate void EvidenceFoundEventHandler(EvidenceKey key);

    public event LeaveSceneEventHandler SceneLeft;
    public event EvidenceFoundEventHandler FoundEvidence;

    private Door Door;

    // Start is called before the first frame update
    void Start()
    {
        this.Door = FindObjectOfType<Door>();
        if (!this.Door)
        {
            Debug.LogWarning("This scene does not have a door");
        }

        Door.DoorOpened += LeaveScene;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LeaveScene()
    {
        SceneLeft.Invoke();
    }

    public void EvidenceFound(EvidenceKey key)
    {
        FoundEvidence.Invoke(key);
    }
}
