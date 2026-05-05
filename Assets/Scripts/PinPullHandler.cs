using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PinPullHandler : MonoBehaviour
{
    [Header("References")]
    public FireExtinguisherController extinguisherController;

    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;

    private bool pinPulled = false;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnPinGrabbed);
            grabInteractable.selectExited.AddListener(OnPinReleased);
        }

        // Start pinned in place
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnPinGrabbed);
            grabInteractable.selectExited.RemoveListener(OnPinReleased);
        }
    }

    private void OnPinGrabbed(SelectEnterEventArgs args)
    {
        if (pinPulled) return;

        pinPulled = true;

        Debug.Log("Pin pulled.");

        // Advance extinguisher tutorial
        if (extinguisherController != null)
            extinguisherController.SetPinPulled(true);

        // Detach from extinguisher so it becomes its own object
        transform.SetParent(null, true);

        // Keep it stable while the ray is holding it
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        Debug.Log("Pin pulled for: " + extinguisherController.gameObject.name);
    }

    private void OnPinReleased(SelectExitEventArgs args)
    {
        if (!pinPulled) return;

        // Now let it drop naturally
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }

        // Optional: prevent re-grabbing after drop
        if (grabInteractable != null)
            grabInteractable.enabled = false;
    }
}