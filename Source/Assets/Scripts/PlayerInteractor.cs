using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{ 
    
   [Header("Interaction")] 
   [SerializeField] private float interactionDistance = 2.5f;
   [SerializeField] private LayerMask interactableLayerMask;
   [SerializeField] private Transform cameraTransform;
   [Space]
   [SerializeField] private Canvas interactionUI;
   [SerializeField] private TextMeshProUGUI interactionText;
   
   
   
   [Space]
   [Header("Inspection")]
   public float inspectionDistance = 1f;
   public bool isInspecting = false;
   
   
   private Vector2 inspectionObjectRotation = Vector2.zero;
   private Interactable currentlyInspected;
   private Interactable interactable;
   
   
   private void Update()
   {
       if (isInspecting)
       {
           interactionUI.enabled = false;
           RotateInspectedObject();
           if(Input.GetMouseButtonDown(0))
               EndInspect();
       }
       else
       {
           if (interactable != null)
           {
               if (Input.GetKeyDown(KeyCode.E))
               {
                   currentlyInspected = interactable;
                   currentlyInspected.onInteract.Invoke();
               }
           }
       }
   }

   
   private void FixedUpdate()
    {
        if (!isInspecting) //Or pause or anything else
        {
            Interactor();
        }
    }
    
   
   /// <summary>
   /// Had some bugs with the raycast in normal update so changed it to fixed, might have been my unity since I had some other math problems until i restarted.
   /// But this works right now just fine.
   ///
   /// </summary>
    private void Interactor()
    {
        RaycastHit hit;
        bool showInteractToolTip = false;
        
        if(Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, interactionDistance, interactableLayerMask))
        {
            interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactionText.text = interactable.GetName();
                showInteractToolTip = true;
            }
        }
        else
            interactable = null;

        interactionUI.enabled = showInteractToolTip;
    }

    
    public void StartInspect(int id)
    {
        if (id == currentlyInspected.id)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            isInspecting = true;
            Vector3 inspectionPosition = cameraTransform.position + cameraTransform.forward * inspectionDistance;
            
            currentlyInspected.GetComponent<Evidence>().StartInspect(inspectionPosition);
        }
    }

    
    private void EndInspect()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        currentlyInspected.GetComponent<Evidence>().StopInspect();
        currentlyInspected = null;
        isInspecting = false;
    }

    
    private void RotateInspectedObject()
    {
        if (Input.GetMouseButton(1))
        {
            inspectionObjectRotation.x -= Input.GetAxisRaw("Mouse Y") * 2f;
            inspectionObjectRotation.y += Input.GetAxisRaw("Mouse X") * 2f;
            currentlyInspected.transform.rotation = Quaternion.Euler(inspectionObjectRotation.x, inspectionObjectRotation.y, 0f);
        }
    }
    
    
}
