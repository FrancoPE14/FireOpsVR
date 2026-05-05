using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FireExtinguisherController : MonoBehaviour
{
    [Header("State")]
    public bool pinPulled = false;
    public bool nozzleGrabbed = false;

    [Header("Instruction Objects")]
    public GameObject pickupInstructionText;
    public GameObject pinInstructionText;
    public GameObject nozzleInstructionText;
    public GameObject squeezeInstructionText;

    [Header("Instruction Anchors")]
    public Transform playerCamera;
    public Transform pickupHintAnchor;
    public Transform pinHintAnchor;
    public Transform nozzleHintAnchor;
    public Transform squeezeHintAnchor;

    [Header("Hint Objects")]
    public GameObject pinHighlight;
    public GameObject nozzleHighlight;
    public GameObject leverHighlight;
    public TrainingFireTarget trainingFireTarget;

    [Header("References")]
    public NozzleHandler nozzleHandler;

    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    [Header("Reset Objects")]
    public GameObject pinObject;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrabbed);
            grabInteractable.selectExited.AddListener(OnReleased);
        }
    }

    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        SafeSetActive(pickupInstructionText, false);
        SafeSetActive(pinInstructionText, false);
        SafeSetActive(nozzleInstructionText, false);
        SafeSetActive(squeezeInstructionText, false);

        if (leverHighlight != null)
            leverHighlight.SetActive(false);
        if (pinHighlight != null) pinHighlight.SetActive(false);
        if (nozzleHighlight != null)
            nozzleHighlight.SetActive(false);

        //SetupHintFollower(pickupInstructionText, pickupHintAnchor);
        //SetupHintFollower(pinInstructionText, pinHintAnchor);
        //SetupHintFollower(nozzleInstructionText, nozzleHintAnchor);
        //SetupHintFollower(squeezeInstructionText, squeezeHintAnchor);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        SafeSetActive(pickupInstructionText, false);

        if (leverHighlight != null)
            leverHighlight.SetActive(false);

        if (!pinPulled)
        {
            SafeSetActive(pinInstructionText, true);

            if (pinHighlight != null)
                pinHighlight.SetActive(true);
        }
    }


    private void OnReleased(SelectExitEventArgs args)
    {
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }

    public void SetPinPulled(bool value)
    {
        pinPulled = value;

        SafeSetActive(pinInstructionText, false);

        if (pinHighlight != null)
            pinHighlight.SetActive(false);

        SafeSetActive(nozzleInstructionText, true);

        if (nozzleHighlight != null)
            nozzleHighlight.SetActive(true);

        if (nozzleHandler != null)
            nozzleHandler.EnableNozzle();
        Debug.Log(gameObject.name + " SetPinPulled called");

        if (trainingFireTarget != null)
        {
            trainingFireTarget.ShowPracticeFire();
        }
    }
    public void SetNozzleGrabbed(bool value)
    {
        nozzleGrabbed = value;

        if (nozzleInstructionText != null)
            nozzleInstructionText.SetActive(false);

        if (squeezeInstructionText != null)
            squeezeInstructionText.SetActive(true);

        if (nozzleHighlight != null)
            nozzleHighlight.SetActive(false);

        if (trainingFireTarget != null)
            trainingFireTarget.ShowSweepInstruction();
    }

    private void SafeSetActive(GameObject obj, bool state)
    {
        if (obj != null) obj.SetActive(state);
    }
    public void ShowPickupInstruction()
    {
        SafeSetActive(pickupInstructionText, true);

        if (leverHighlight != null)
            leverHighlight.SetActive(true);
    }
    public void ResetExtinguisherState()
    {
        pinPulled = false;
        nozzleGrabbed = false;

        if (pinObject != null)
            pinObject.SetActive(true);

        transform.position = originalPosition;
        transform.rotation = originalRotation;

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        SafeSetActive(pickupInstructionText, false);
        SafeSetActive(pinInstructionText, false);
        SafeSetActive(nozzleInstructionText, false);
        SafeSetActive(squeezeInstructionText, false);

        SafeSetActive(pinHighlight, false);
        SafeSetActive(nozzleHighlight, false);
        SafeSetActive(leverHighlight, false);

        if (nozzleHandler != null)
            nozzleHandler.ResetNozzleState();
    }
}