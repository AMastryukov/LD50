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
    public static Action<string> OnEvidenceFoundNotification;
    public static Action<bool> OnModifyDOF;

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
    [SerializeField] private CanvasGroup notificationUI;
    [SerializeField] private TextMeshProUGUI notificationMessage;


    [Space]
    [Header("Post Processing")]
    [SerializeField] private Volume volume;

    [Space]
    [Header("Materials")]
    [SerializeField] private Material main_mat;
    [SerializeField] private Material emissive_mat;

    [Space]
    [Header("Flashlight")]
    [SerializeField] private Light flashlight;

    private Vector2 inspectionObjectRotation = Vector2.zero;
    private Interactable clickedInteractable;
    private Interactable lookingAtInteractable;
    private PlayerManager manager;
    private int oldlayer;
    private List<int> oldchildlayers = new List<int>();

    private void Awake()
    {
        manager = FindObjectOfType<PlayerManager>();
        OnEvidenceFoundNotification += OnNotificationUI;
        OnModifyDOF += EnableDOF;
        ResetInspectionUI();
        ResetInteractionUI();
        ResetNotificationnUI();
    }

    private void Update()
    {
        switch (manager.CurrentState)
        {
            case PlayerManager.PlayerStates.Inspect:
                interactionUI.enabled = false;
                crosshair.enabled = false;
                RotateInspectedObject();
                EnableDOF(true);
                if (Input.GetMouseButtonDown(1))
                {
                    EndInspect();
                    EnableDOF(false);
                }

                break;

            case PlayerManager.PlayerStates.Move:
                crosshair.enabled = true;
                if (Input.GetKeyDown(KeyCode.F))
                {
                    // FLASHLIGHT
                    // flashlight.enabled = !flashlight.enabled;
                }
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
                interactionUI.enabled = false;
                crosshair.enabled = false;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    manager.CurrentState = PlayerManager.PlayerStates.Move;
                }
                break;
        }
    }

    private void EnableDOF(bool val)
    {
        volume.profile.TryGet<DepthOfField>(out var depthoffield);
        if (val)
        {
            if(depthoffield.active)
                return;
            depthoffield.active = true;
            DOTween.To(() => depthoffield.focusDistance.value, x => depthoffield.focusDistance.value = x, 0.1f, 0.5f).onComplete =
                () =>
                {
                    depthoffield.active = true;
                };
        }
        else
        {
            DOTween.To(() => depthoffield.focusDistance.value, x => depthoffield.focusDistance.value = x, 2f,
                0.5f).onComplete = () =>
            {
                depthoffield.active = false;
            };
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

        if (clickedInteractable.gameObject.transform.childCount > 0)
        {
            foreach (Transform child in clickedInteractable.gameObject.transform)
            {
                oldchildlayers.Add(child.gameObject.layer);
                child.gameObject.layer = 7;
                child.gameObject.GetComponent<Renderer>().material = emissive_mat;
            }
        }
        else
        {
            oldlayer = clickedInteractable.gameObject.layer;
            clickedInteractable.gameObject.layer = 7;
            clickedInteractable.gameObject.GetComponent<Renderer>().material = emissive_mat;
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
                child.gameObject.GetComponent<Renderer>().material = main_mat;
                i++;
            }
        }
        else
        {
            clickedInteractable.gameObject.layer = oldlayer;
            clickedInteractable.gameObject.GetComponent<Renderer>().material = main_mat;
        }

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
        inpsectionNameText.text = evidence.evidenceData.Name;
    }

    private void OnNotificationUI(string text)
    {
        notificationUI.DOKill();

        notificationMessage.text = text;
        notificationUI.DOFade(1f, 1f).onComplete = () =>
        {
            notificationUI.DOFade(0, 5f).onComplete = ResetNotificationnUI;
        };
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

    public void ResetInteractionUI()
    {
        interactionUI.enabled = false;
    }

    private void ResetInspectionUI()
    {
        inpsectionDescriptionText.text = "";
        inpsectionNameText.text = "";
    }
    
    private void ResetNotificationnUI()
    {
        notificationUI.DOKill();

        notificationUI.alpha = 0;
        notificationMessage.text = string.Empty;
    }
    
}
