using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Evidence : Interactable
{
    public static Action<Evidence> OnInspect;

    [Header("Evidence")] 
    [SerializeField] public EvidenceData evidenceData;
    
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    
    private void Awake()
    {
        if (evidenceData == null)
        {
            Debug.LogError($"Evidence Data is null for {gameObject.name}");
        }
    }

    private void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }
    
    
    public override void Interact()
    {
        OnInspect?.Invoke(this);
        GameEventSystem.Instance.OnEvidenceInspected?.Invoke(evidenceData.Name);
    }

    
    public void StartInspect(Vector3 pos)
    {
        transform.position = pos;
    }

    
    public void StopInspect()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }

    
    public override string GetName()
    {
        return evidenceData.Name;
    }

    
    public string GetDescription()
    {
        return evidenceData.Description;
    }
}
