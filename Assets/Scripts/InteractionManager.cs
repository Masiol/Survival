using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    private float interactionDistance => GameManager.instance.options.InteractionDistance;
    private IInteractable currentInteractable;
    private IPickupable currentPickupable;
    private IHoldable currentHoldable;
    private IOutlineable currentOutlineable;

    private bool checkInteraction = true;

    private void Start()
    {
        InputManager.Instance.OnInteract += HandleInteraction;
        InputManager.Instance.OnPickup += HandlePickup;
        InputManager.Instance.OnHold += HandleHold;
        InputManager.Instance.StopPlayer += StopDetectInteraction;
        Debug.Log("InteractionManager started.");
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnInteract -= HandleInteraction;
            InputManager.Instance.OnPickup -= HandlePickup;
            InputManager.Instance.OnHold -= HandleHold;
            InputManager.Instance.StopPlayer -= StopDetectInteraction;
        }
        Debug.Log("InteractionManager disabled.");
    }

    private void Update()
    {
        if(checkInteraction)
        CheckForInteractableObject();
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
            Debug.Log("Raycast hit: " + hit.collider.gameObject.name);

            // Check if the hit object is interactable, pickupable, or holdable
            if (hit.collider.gameObject.GetComponent<IInteractable>() != null ||
                hit.collider.gameObject.GetComponent<IPickupable>() != null ||
                hit.collider.gameObject.GetComponent<IHoldable>() != null)
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
        var holdable = _obj.GetComponent<IHoldable>();
        var outlineable = _obj.GetComponent<IOutlineable>();

        SetCurrentInteractable(interactable, ref currentInteractable, outlineable);
        SetCurrentInteractable(pickupable, ref currentPickupable, outlineable);
        SetCurrentInteractable(holdable, ref currentHoldable, outlineable);
    }

    private void SetCurrentInteractable<T>(T _newInteractable, ref T currentInteractable, IOutlineable _outlineable)
    {
        if (_newInteractable != null && !EqualityComparer<T>.Default.Equals(currentInteractable, _newInteractable))
        {
            Debug.Log("New interactable object: " + _newInteractable.GetType().Name);
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
            Debug.Log("Outline " + (_outline ? "enabled" : "disabled") + " on: " + _outlineable.GetType().Name);
            _outlineable.SetOutline(_outline);
        }
    }

    private void HandleInteraction()
    {
        if (currentInteractable != null)
        {
            Debug.Log("Interacting with: " + currentInteractable.GetType().Name);
            currentInteractable.Interact();
        }
    }

    private void HandlePickup()
    {
        if (currentPickupable != null)
        {
            Debug.Log("Picking up: " + currentPickupable.GetType().Name);
            currentPickupable.Pickup();
        }
    }

    private void HandleHold()
    {
        if (currentHoldable != null)
        {
            Debug.Log("Holding: " + currentHoldable.GetType().Name);
            currentHoldable.Hold();
        }
    }

    private void ClearCurrentInteractable()
    {
        if (currentInteractable != null || currentPickupable != null || currentHoldable != null || currentOutlineable != null)
        {
            Debug.Log("Clearing current interactable objects.");
            UpdateOutline(currentOutlineable, false);
            currentInteractable = null;
            currentHoldable = null;
            currentPickupable = null;
            currentOutlineable = null;
        }
    }
}
