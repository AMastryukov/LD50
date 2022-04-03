using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
public class Evidence : Interactable
{
    [Header("Evidence")]
    public EvidenceData evidenceData;

    private new string name;
    private string description;
    
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private PlayerInteractor interactor;
    
    //description, sound, other effects...
    private void Awake()
    {
        interactor = FindObjectOfType<PlayerInteractor>();
    }

    private void Start()
    {
        if (evidenceData != null)
        {
            name = evidenceData.EvidenceName;
            description = evidenceData.Description;
        }else
            Debug.LogWarning("EvidenceData not found");
        
        
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }
    
    
    public override void Interact()
    {
        interactor.StartInspect(id);
    }

    
    public void StartInspect(Vector3 pos)
    {
        transform.DOMove(pos, 0.5f);
        //Debug.Log(pos);
        //transform.position = pos;
    }

    
    public void StopInspect()
    {
        transform.DOMove(originalPosition, 0.5f);
        transform.DORotate(originalRotation.eulerAngles, 0.5f);
    }

    
    public override string GetName()
    {
        return name;
    }

    
    public string GetDescription()
    {
        return description;
    }
}
