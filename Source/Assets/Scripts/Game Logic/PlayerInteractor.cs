using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static CrimeSceneManager;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

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

    [Space]
    [Header("Inspection")]
    //[SerializeField] private Transform inspectionTransform;

    [Space]
    [Header("Post Processing")]
    [SerializeField] private Volume volume;

    private Vector2 inspectionObjectRotation = Vector2.zero;
    private Interactable clickedInteractable;
    private Interactable lookingAtInteractable;
    private PlayerManager manager;
    private int oldlayer;
    private List<int> oldchildlayers = new List<int>();

    private void Awake()
    {
        manager = FindObjectOfType<PlayerManager>();
        //inspectionUI = 0f;
    }

    private void Update()
    {
        switch (manager.playerState)
        {
            case PlayerManager.PlayerStates.Inspect:
                interactionUI.enabled = false;
                crosshair.enabled = false;
                RotateInspectedObject();
                volume.enabled = true;

                if (Input.GetMouseButtonDown(1))
                {
                    EndInspect();
                    volume.enabled = false;
                }

                break;

            case PlayerManager.PlayerStates.Move:
                if (lookingAtInteractable != null)
                {
                    
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        
                            
                            clickedInteractable = lookingAtInteractable;
                            
                            clickedInteractable.OnInteract?.Invoke();
                            
                            Debug.Log(clickedInteractable);
                            
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
                Debug.Log(lookingAtInteractable.GetName());
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
        manager.playerState = PlayerManager.PlayerStates.Inspect;
        if (clickedInteractable.gameObject.transform.childCount > 0)
        {

            foreach (Transform child in clickedInteractable.gameObject.transform)
            {
                oldchildlayers.Add(child.gameObject.layer);
            }

            foreach (Transform child in clickedInteractable.gameObject.transform)
            {
                child.gameObject.layer = 7;
            }
        }
        else
        {
            oldlayer = clickedInteractable.gameObject.layer;
            clickedInteractable.gameObject.layer = 7;
        }
    }

    private void EndInspect()
    {
        OnInspectionUIEnd();

        if (clickedInteractable.gameObject.transform.childCount > 0)
        {
            int i = 0;

            foreach (Transform child in clickedInteractable.gameObject.transform)
            {
                child.gameObject.layer = oldchildlayers[i];
                i++;
            }
        }
        else
        {
            clickedInteractable.gameObject.layer = oldlayer;
        }

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
