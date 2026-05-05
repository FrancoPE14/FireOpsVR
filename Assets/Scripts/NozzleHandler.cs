using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NozzleHandler : MonoBehaviour
{
    [Header("References")]
    public FireExtinguisherController extinguisherController;
    public ParticleSystem foamPS;
    public GameObject pullNozzleInstruction;

    [Header("Visual Pull Effect")]
    public Transform visualToOffset;
    public Vector3 heldLocalPositionOffset = new Vector3(0f, 0f, 0.0f);
    public Vector3 heldLocalRotationOffset = new Vector3(0f, 10f, 0f);
    public float visualLerpSpeed = 8f;

    private XRSimpleInteractable simpleInteractable;

    private bool nozzleEnabled = false;
    private bool nozzleHeld = false;
    private bool nozzleAlreadyGrabbed = false;

    private Vector3 originalLocalPosition;
    private Quaternion originalLocalRotation;
// public Training3WrongExtinguisherDetector wrongDetector;

    void Awake()
    {
        simpleInteractable = GetComponent<XRSimpleInteractable>();

        if (simpleInteractable != null)
            simpleInteractable.enabled = false;

        if (pullNozzleInstruction != null)
            pullNozzleInstruction.SetActive(false);

        if (visualToOffset != null)
        {
            originalLocalPosition = visualToOffset.localPosition;
            originalLocalRotation = visualToOffset.localRotation;
        }
    }
    void Start()
    {
        if (visualToOffset != null)
        {
            visualToOffset.localPosition = originalLocalPosition;
            visualToOffset.localRotation = originalLocalRotation;
        }
    }

    void OnEnable()
    {
        if (simpleInteractable != null)
        {
            simpleInteractable.selectEntered.AddListener(OnNozzleSelected);
            simpleInteractable.selectExited.AddListener(OnNozzleReleased);
        }
    }

    void OnDisable()
    {
        if (simpleInteractable != null)
        {
            simpleInteractable.selectEntered.RemoveListener(OnNozzleSelected);
            simpleInteractable.selectExited.RemoveListener(OnNozzleReleased);
        }
    }

    void Update()
    {
        if (visualToOffset == null) return;

        if (!nozzleHeld)
        {
            // Force exact attached pose before grab
            visualToOffset.localPosition = originalLocalPosition;
            visualToOffset.localRotation = originalLocalRotation;
            return;
        }

        Vector3 targetPos = originalLocalPosition + heldLocalPositionOffset;
        Quaternion targetRot = originalLocalRotation * Quaternion.Euler(heldLocalRotationOffset);

        visualToOffset.localPosition = Vector3.Lerp(
            visualToOffset.localPosition,
            targetPos,
            Time.deltaTime * visualLerpSpeed
        );

        visualToOffset.localRotation = Quaternion.Slerp(
            visualToOffset.localRotation,
            targetRot,
            Time.deltaTime * visualLerpSpeed
        );
    }

    public void EnableNozzle()
    {
        nozzleEnabled = true;

        if (simpleInteractable != null)
            simpleInteractable.enabled = true;

        if (pullNozzleInstruction != null)
            pullNozzleInstruction.SetActive(true);

        Debug.Log("Nozzle step enabled.");
    }

    private void OnNozzleSelected(SelectEnterEventArgs args)
    {
        if (!nozzleEnabled)
            return;

        nozzleHeld = true;

        if (!nozzleAlreadyGrabbed)
        {
            nozzleAlreadyGrabbed = true;

            if (pullNozzleInstruction != null)
                pullNozzleInstruction.SetActive(false);

            if (extinguisherController != null)
                extinguisherController.SetNozzleGrabbed(true);
        }

    }

    private void OnNozzleReleased(SelectExitEventArgs args)
    {
        nozzleHeld = false;
    }

    public bool IsNozzleHeld()
    {
        return nozzleHeld;
    }

    public void StartSpraying()
    {
        if (foamPS != null && !foamPS.isPlaying)
            foamPS.Play();
    }

    public void StopSpraying()
    {
        if (foamPS != null && foamPS.isPlaying)
            foamPS.Stop();
    }
    public void ResetNozzleState()
    {
        nozzleEnabled = false;
        nozzleHeld = false;
        nozzleAlreadyGrabbed = false;

        if (simpleInteractable != null)
            simpleInteractable.enabled = false;

        if (pullNozzleInstruction != null)
            pullNozzleInstruction.SetActive(false);

        if (foamPS != null && foamPS.isPlaying)
            foamPS.Stop();

        if (visualToOffset != null)
        {
            visualToOffset.localPosition = originalLocalPosition;
            visualToOffset.localRotation = originalLocalRotation;
        }
    }
}