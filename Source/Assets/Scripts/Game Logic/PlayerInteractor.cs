using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameSceneManager;

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
    [SerializeField] private TextMeshProUGUI interactionText;

    [Space]
    [Header("Inspection")]
    //[SerializeField] private Transform inspectionTransform;
    
    private Vector2 inspectionObjectRotation = Vector2.zero;
    private Interactable clickedInteractable;
    private Interactable lookingAtInteractable;
    private PlayerManager manager;

    private void Awake()
    {
        manager = FindObjectOfType<PlayerManager>();
    }

    private void Update()
    {
        switch (manager.playerState)
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
                    manager.playerState = PlayerManager.PlayerStates.Move;
                }
                break;
        }
    }


    private void FixedUpdate()
    {
        if (manager.playerState == PlayerManager.PlayerStates.Move) //Or pause or anything else
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
                interactionText.text = lookingAtInteractable.GetName();
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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        crosshair.enabled = false;
        Vector3 inspectionPosition = cameraTransform.position + cameraTransform.forward * 0.7f;
        Debug.Log(inspectionPosition);
        evidence.StartInspect(inspectionPosition);

        manager.playerState = PlayerManager.PlayerStates.Inspect;
    }

    private void EndInspect()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        crosshair.enabled = true;

        clickedInteractable.GetComponent<Evidence>().StopInspect();
        clickedInteractable = null;

        manager.playerState = PlayerManager.PlayerStates.Move;
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
}
