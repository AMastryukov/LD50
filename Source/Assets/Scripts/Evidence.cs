using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Evidence : Interactable
{
    [Header("Evidence")]
    [SerializeField] private Vector3 originalPosition;
    [SerializeField] private Quaternion originalRotation;
    [SerializeField] private PlayerInteractor interactor;

    private void Awake()
    {
        interactor = FindObjectOfType<PlayerInteractor>();
    }

    private void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }
    
    
    public override void Interact()
    {
        interactor.StartInspect(id);
    }

    
    public void StartInspect(Vector3 pos)
    {
        Debug.Log(pos);
        transform.position = pos;
    }

    
    public void StopInspect()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }

    
    public override string GetDescription()
    {
        return name;
    }
}
