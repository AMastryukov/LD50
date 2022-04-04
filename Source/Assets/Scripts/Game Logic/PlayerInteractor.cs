using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static CrimeSceneManager;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private float interactionDistance = 2.5f;
    [SerializeField] private LayerMask interactableLayerMask;
    [SerializeField] private Transform cameraTransform;
    
    [Space]
    [Header("UI")]
    [SerializeField] private Canvas interactionUI;
    [SerializeField] private Image crosshair;
    [SerializeField] private CanvasGroup inspectionUI;
    [SerializeField] private TextMeshProUGUI inpsectionNameText;
    [SerializeField] private TextMeshProUGUI inpsectionDescriptionText;
    
    private Vector2 inspectionObjectRotation = Vector2.zero;
    private Interactable clickedInteractable;
    private Interactable lookingAtInteractable;
    private PlayerManager manager;

    private void Awake()
    {
        manager = FindObjectOfType<PlayerManager>();
        //inspectionUI = 0f;
    }

    private void Update()
    {
        switch (manager.CurrentState)
        {
            case PlayerManager.PlayerStates.Inspect:
                interactionUI.enabled = false;
                crosshair.enabled = false;
                RotateInspectedObject();

                if (Input.GetMouseButtonDown(1))
                {
                    EndInspect();
                }

                break;

            case PlayerManager.PlayerStates.Move:
                if (lookingAtInteractable != null)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        clickedInteractable = lookingAtInteractable;
                        clickedInteractable.OnInteract?.Invoke();
                        clickedInteractable.Interact();
                            
                        if (clickedInteractable is Evidence)
                        {
                            StartInspect((Evidence)clickedInteractable);
                        }
                    }
                }
                break;

            case PlayerManager.PlayerStates.Interrogate:
                if (Input.GetMouseButtonDown(1))
                {
                    manager.CurrentState = PlayerManager.PlayerStates.Move;
                }
                break;
        }
    }


    private void FixedUpdate()
    {
        if (manager.CurrentState == PlayerManager.PlayerStates.Move) //Or pause or anything else
        {
            CastInteractionRay();
        }
    }

    /// <summary>
    /// Had some bugs with the raycast in normal update so changed it to fixed, might have been my unity since I had some other math problems until i restarted.
    /// But this works right now just fine.
    /// </summary>
    private void CastInteractionRay()
    {
        RaycastHit hit;
        bool showInteractToolTip = false;

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, interactionDistance, interactableLayerMask))
        {
            lookingAtInteractable = hit.collider.GetComponent<Interactable>();
            if (lookingAtInteractable != null)
            {
                showInteractToolTip = true;
            }
        }
        else
        {
            lookingAtInteractable = null;
        }

        interactionUI.enabled = showInteractToolTip;
    }

    public void StartInspect(Evidence evidence)
    {
        OnInspectionUI(evidence);
        
        Vector3 inspectionPosition = cameraTransform.position + cameraTransform.forward * 0.6f;
        evidence.StartInspect(inspectionPosition);
        manager.CurrentState = PlayerManager.PlayerStates.Inspect;
    }

    private void EndInspect()
    {
        OnInspectionUIEnd();

        clickedInteractable.GetComponent<Evidence>().StopInspect();
        clickedInteractable = null;

        manager.CurrentState = PlayerManager.PlayerStates.Move;
    }

    private void RotateInspectedObject()
    {
        if (Input.GetMouseButton(0))
        {
            inspectionObjectRotation.x -= Input.GetAxisRaw("Mouse Y") * 2f;
            inspectionObjectRotation.y += Input.GetAxisRaw("Mouse X") * -2f;
            clickedInteractable.transform.rotation = Quaternion.Euler(inspectionObjectRotation.x, inspectionObjectRotation.y, 0f);
        }
    }

    private void OnInspectionUI(Evidence evidence)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        inspectionUI.DOFade(1f, 0.5f)
            .SetEase(Ease.InCirc)
            .OnComplete(() =>
            {
                inspectionUI.interactable = true;
                inspectionUI.blocksRaycasts = true;
            });
        crosshair.enabled = false;
        
        inpsectionDescriptionText.text = evidence.evidenceData.Description;
        inpsectionNameText.text = evidence.evidenceData.name;
    }
    
    private void OnInspectionUIEnd()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inspectionUI.DOFade(0f, 0.5f)
            .SetEase(Ease.InCirc)
            .OnComplete(() =>
            {
                inspectionUI.interactable = false;
                inspectionUI.blocksRaycasts = false;
            });
        crosshair.enabled = true;
    }
    
}
