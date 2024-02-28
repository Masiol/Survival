using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI TextOnScreen;
    private float interactionDistance => GameManager.instance.options.InteractionDistance;
    private IInteractable currentInteractable;
    private IPickupable currentPickupable;
    private IGrabbable currentGrabbable;
    private IOutlineable currentOutlineable;
    private IDetectable currentDetectable;
    private bool checkInteraction = true;

    [SerializeField] private string[] texts;

    private void Start()
    {
        InputManager.Instance.OnInteract += HandleInteraction;
        InputManager.Instance.OnPickup += HandlePickup;
        InputManager.Instance.OnGrab += HandleGrab;
        InputManager.Instance.StopPlayer += StopDetectInteraction;
        //Debug.Log("InteractionManager started.");
    }

    private void Update()
    {
        if (checkInteraction)
            CheckForInteractableObject();
    }
    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnInteract -= HandleInteraction;
            InputManager.Instance.OnPickup -= HandlePickup;
            InputManager.Instance.OnGrab -= HandleGrab;
            InputManager.Instance.StopPlayer -= StopDetectInteraction;
        }
       // Debug.Log("InteractionManager disabled.");
    }
    private void StopDetectInteraction(bool interaction)
    {
        checkInteraction = interaction;
    }
    private void CheckForInteractableObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
        {
            //Debug.Log("Raycast hit: " + hit.collider.gameObject.name);

            // Check if the hit object is interactable, pickupable, or holdable
            if (hit.collider.gameObject.GetComponent<IInteractable>() != null ||
                hit.collider.gameObject.GetComponent<IPickupable>() != null ||
                hit.collider.gameObject.GetComponent<IDetectable>() != null ||
                hit.collider.gameObject.GetComponent<IGrabbable>() != null)
            {
                UpdateInteractableObject(hit.collider.gameObject);
            }
            else
            {
                ClearCurrentInteractable();
            }
        }
        else
        {
            ClearCurrentInteractable();
        }
    }
    private void UpdateInteractableObject(GameObject _obj)
    {
        var interactable = _obj.GetComponent<IInteractable>();
        var pickupable = _obj.GetComponent<IPickupable>();
        var grabbable = _obj.GetComponent<IGrabbable>();
        var outlineable = _obj.GetComponent<IOutlineable>();
        var detectable = _obj.GetComponent<IDetectable>();

        // Reset current interactable if there's a change
        if (currentInteractable != interactable || currentPickupable != pickupable || currentGrabbable != grabbable)
        {
            ClearCurrentInteractable();
        }

        if (interactable != null)
        {
            SetCurrentInteractable(interactable, ref currentInteractable, outlineable);
            TextOnScreen.text = texts[0];
        }
        else if (pickupable != null && grabbable != null)
        {
            // Ustawiamy oba, poniewa¿ obiekt jest zarówno grabbable jak i pickupable
            SetCurrentInteractable(pickupable, ref currentPickupable, outlineable);
            SetCurrentInteractable(grabbable, ref currentGrabbable, outlineable);
            TextOnScreen.text = texts[2]; // Zak³adaj¹c, ¿e texts[3] to "Pickup or Grab"
        }
        else if (pickupable != null)
        {
            SetCurrentInteractable(pickupable, ref currentPickupable, outlineable);
            TextOnScreen.text = texts[1];
        }
        else if (detectable != null)
        {
            SetCurrentInteractable(detectable, ref currentDetectable, outlineable);
            TextOnScreen.text = "";
            detectable.DetectRaycast(true);
            GetComponentInParent<DetectableObserver>().AddDetectable(detectable);
        }

    }
    private void SetCurrentInteractable<T>(T _newInteractable, ref T currentInteractable, IOutlineable _outlineable)
    {
        if (_newInteractable != null && !EqualityComparer<T>.Default.Equals(currentInteractable, _newInteractable))
        {
           // Debug.Log("New interactable object: " + _newInteractable.GetType().Name);
            UpdateOutline(currentOutlineable, false);
            currentInteractable = _newInteractable;
            UpdateOutline(_outlineable, true);
            currentOutlineable = _outlineable;
        }
    }

    private void UpdateOutline(IOutlineable _outlineable, bool _outline)
    {
        if (_outlineable != null && _outlineable as MonoBehaviour != null && (_outlineable as MonoBehaviour).gameObject != null)
        {
            //Debug.Log("Outline " + (_outline ? "enabled" : "disabled") + " on: " + _outlineable.GetType().Name);
            _outlineable.SetOutline(_outline);
        }
    }

    private void HandleInteraction()
    {
        if (currentInteractable != null)
        {
           // Debug.Log("Interacting with: " + currentInteractable.GetType().Name);
            currentInteractable.Interact();
        }
    }
    private void HandlePickup()
    {
        if (currentPickupable != null)
        {
           // Debug.Log("Picking up: " + currentPickupable.GetType().Name);
            currentPickupable.Pickup();
        }
    }
    private void HandleGrab()
    {
        if (currentGrabbable != null)
        {
           // Debug.Log("Holding: " + currentGrabbable.GetType().Name);
            currentGrabbable.Grab();
        }
    }
    private void ClearCurrentInteractable()
    {
        if (currentInteractable != null || currentPickupable != null || currentGrabbable != null || currentOutlineable != null || currentDetectable != null)
        {
          //  Debug.Log("Clearing current interactable objects.");
            UpdateOutline(currentOutlineable, false);
            currentInteractable = null;
            currentGrabbable = null;
            currentPickupable = null;
            currentOutlineable = null;
            if (currentDetectable != null)
            {
                currentDetectable.DetectRaycast(false);
                GetComponentInParent<DetectableObserver>().RemoveDetectable(currentDetectable);
                currentDetectable = null;
            }
            TextOnScreen.text = "";
        }
    }
}
