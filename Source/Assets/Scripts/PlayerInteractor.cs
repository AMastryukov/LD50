using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{ 
    
    private static PlayerInteractor _instance;
    
   [Header("Interaction")] 
   [SerializeField] private float interactionDistance = 4f;
   [SerializeField] private LayerMask interactableLayerMask;
   [SerializeField] private GameObject interactionUI;
   [SerializeField] private TextMeshProUGUI interactionText;
   private Interactable interactable;
   
   [Space]
   [Header("Inspection")]
   public float inspectionDistance = 1f;
   public bool isInspecting = false;

   [SerializeField] private Interactable currentlyInspected;
   private Vector2 objectRotation = Vector2.zero;
   
   public static PlayerInteractor Instance
   {
       get
       {
           if (_instance == null) _instance = GameObject.FindObjectOfType<PlayerInteractor>();
           return _instance;
       }
   }

   
   private void Update()
   {
       if (isInspecting)
       {
           interactionUI.SetActive(false);
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
        
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionDistance, interactableLayerMask))
        {
            interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactionText.text = interactable.GetDescription();
                showInteractToolTip = true;
            }
        }
        else
            interactable = null;
    
        interactionUI.SetActive(showInteractToolTip);
    }

    
    public void StartInspect(int id)
    {
        if (id == currentlyInspected.id)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            isInspecting = true;
            Vector3 inspectionPosition = Camera.main.transform.position + Camera.main.transform.forward * inspectionDistance;
            
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
            objectRotation.x -= Input.GetAxisRaw("Mouse Y") * 2f;
            objectRotation.y += Input.GetAxisRaw("Mouse X") * 2f;
            currentlyInspected.transform.rotation = Quaternion.Euler(objectRotation.x, objectRotation.y, 0f);
        }
    }
    
    
}
