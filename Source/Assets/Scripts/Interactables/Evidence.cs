using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    }

    
    public void StartInspect(Vector3 pos)
    {
        transform.DOMove(pos, 1f);
    }

    
    public void StopInspect()
    {
        transform.DOMove(originalPosition, 1f);
        transform.DORotate(originalRotation.eulerAngles, 1f);
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
