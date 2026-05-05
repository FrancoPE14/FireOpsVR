using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class UIButtonTeleportManager : MonoBehaviour
{
    [Header("XR Player Root")]
    public Transform xrOrigin;

    [Header("Training Target Points")]
    public Transform training1Point;
    public Transform training2Point;
    public Transform training3Point;

    [Header("Messages To Hide When Leaving Start Area")]
    public GameObject greatJobMessage;
    public GameObject continueTraining2Message;
    public GameObject continueTraining3Message;

    [Header("Messages To Show On Return")]
    public GameObject showAfterTraining1Message; // "Select Training 2"
    public GameObject showAfterTraining2Message; // "Select Training 3"
    [Header("Pass Messages To Hide On Return")]
    public GameObject training1PassMessage;
    public GameObject training2PassMessage;

    [Header("Training 1 Objects")]
    public GameObject[] training1Fires;
    public GameObject training1Extinguisher;

    [Header("Training 2 Objects")]
    public GameObject[] training2Fires;
    public GameObject training2Extinguisher;

    [Header("Training 3 Objects")]
    public GameObject[] training3Fires;
    public GameObject training3Extinguisher;
    public GameObject projectNotCompleteMessage;
    public float training3MessageDelay = 3f;

    [Header("Return Button Input")]
    public InputActionReference returnAction;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool isInTrainingArea = false;
    private int currentTraining = 0;
    [Header("Retry / Fail Message Input")]
    public GameObject training2FailMessage;
    [Header("Training 3 Result Messages")]
    public GameObject training3PassMessage;
    public GameObject trainingCompleteMessage;
    [Header("Shared Extinguishers To Reset Before Training 3")]
    public GameObject[] sharedExtinguishersForTraining3;
    [Header("Scenario Specific Fail Detectors")]
    public WrongExtinguisherFail training2WrongDetector; // dry powder
    public WrongExtinguisherFail training3WrongDetector; // foam
    [Header("Scenario Area Managers")]
    public GameObject training2AreaManager;
    public GameObject training3AreaManager;
    [Header("Training 2/3 Separate Foam Extinguishers")]
    public GameObject foamTraining2;
    public GameObject foamTraining3;


    private void OnEnable()
    {
        if (returnAction != null && returnAction.action != null)
        {
            returnAction.action.Enable();
            returnAction.action.performed += OnReturnPressed;
        }

    
    }

    private void OnDisable()
    {
        if (returnAction != null && returnAction.action != null)
        {
            returnAction.action.performed -= OnReturnPressed;
        }


    }
    private IEnumerator HideFailMessageSafely()
    {
        yield return null; // wait 1 frame

        if (training2FailMessage != null)
            training2FailMessage.SetActive(false);
    }
    private void Start()
    {
        if (xrOrigin == null)
        {
            Debug.LogError("XR Origin is not assigned.");
            return;
        }

        startPosition = xrOrigin.position;
        startRotation = xrOrigin.rotation;

        SetObjectsActive(training1Fires, false);
        SetObjectsActive(training2Fires, false);
        SetObjectsActive(training3Fires, false);

       // if (training1Extinguisher != null) training1Extinguisher.SetActive(false);
        //if (training2Extinguisher != null) training2Extinguisher.SetActive(false);
       // if (training3Extinguisher != null) training3Extinguisher.SetActive(false);

        if (projectNotCompleteMessage != null) projectNotCompleteMessage.SetActive(false);
        if (training2AreaManager != null)
            training2AreaManager.SetActive(false);

        if (training3AreaManager != null)
            training3AreaManager.SetActive(false);

        if (training2WrongDetector != null)
            training2WrongDetector.enabled = false;

        if (training3WrongDetector != null)
            training3WrongDetector.enabled = false;
    }

    public void GoToTraining1()
    {
        HideStartMessages();

        SetObjectsActive(training1Fires, true);
        if (training1Extinguisher != null) training1Extinguisher.SetActive(true);

        currentTraining = 1;
        TeleportTo(training1Point);
    }

    public void GoToTraining2()
    {
        HideStartMessages();

        if (training2AreaManager != null)
            training2AreaManager.SetActive(true);

        if (training3AreaManager != null)
            training3AreaManager.SetActive(false);

        SetObjectsActive(training2Fires, true);
        if (training2Extinguisher != null) training2Extinguisher.SetActive(true);

        if (training2WrongDetector != null)
            training2WrongDetector.enabled = true;

        if (training3WrongDetector != null)
            training3WrongDetector.enabled = false;
        if (foamTraining2 != null) foamTraining2.SetActive(true);
        if (foamTraining3 != null) foamTraining3.SetActive(false);

        currentTraining = 2;
        TeleportTo(training2Point);
    }
    public void GoToTraining3()
    {
        HideStartMessages();

        if (training2AreaManager != null)
            training2AreaManager.SetActive(false);

        if (training3AreaManager != null)
            training3AreaManager.SetActive(true);

        SetObjectsActive(training3Fires, true);

        foreach (GameObject ext in sharedExtinguishersForTraining3)
        {
            ResetExtinguisher(ext);
        }

        if (training3Extinguisher != null)
            training3Extinguisher.SetActive(true);

        if (training2WrongDetector != null)
            training2WrongDetector.enabled = false;

        if (training3WrongDetector != null)
            training3WrongDetector.enabled = true;
        if (foamTraining2 != null) foamTraining2.SetActive(false);
        if (foamTraining3 != null) foamTraining3.SetActive(true);

        currentTraining = 3;
        TeleportTo(training3Point);
    }

    private void TeleportTo(Transform targetPoint)
    {
        if (xrOrigin == null || targetPoint == null)
        {
            Debug.LogWarning("XR Origin or target point is missing.");
            return;
        }

        xrOrigin.position = targetPoint.position;
        xrOrigin.rotation = targetPoint.rotation;

        isInTrainingArea = true;
    }

    private void OnReturnPressed(InputAction.CallbackContext context)
    {
        if (!isInTrainingArea || xrOrigin == null)
            return;

        xrOrigin.position = startPosition;
        xrOrigin.rotation = startRotation;

        isInTrainingArea = false;

        if (training1PassMessage != null)
            training1PassMessage.SetActive(false);

        if (training2PassMessage != null)
            training2PassMessage.SetActive(false);


        if (currentTraining == 1)
        {
            if (showAfterTraining1Message != null)
                showAfterTraining1Message.SetActive(true);
        }
        else if (currentTraining == 2)
        {
            if (showAfterTraining2Message != null)
                showAfterTraining2Message.SetActive(true);
        }
        else if (currentTraining == 3)
        {
            if (trainingCompleteMessage != null)
                trainingCompleteMessage.SetActive(true);
        }

        currentTraining = 0;
    }


    private void HideStartMessages()
    {
        if (greatJobMessage != null) greatJobMessage.SetActive(false);
        if (continueTraining2Message != null) continueTraining2Message.SetActive(false);
        if (continueTraining3Message != null) continueTraining3Message.SetActive(false);
        if (projectNotCompleteMessage != null) projectNotCompleteMessage.SetActive(false);
    }

    private void SetObjectsActive(GameObject[] objects, bool state)
    {
        if (objects == null) return;

        foreach (GameObject obj in objects)
        {
            if (obj != null)
                obj.SetActive(state);
        }
    }
    private void ResetExtinguisher(GameObject extinguisher)
    {
        if (extinguisher == null) return;

        Rigidbody rb = extinguisher.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        FireExtinguisherController controller =
            extinguisher.GetComponent<FireExtinguisherController>();

        if (controller != null)
            controller.ResetExtinguisherState();

        SprayController spray =
            extinguisher.GetComponentInChildren<SprayController>();

        if (spray != null)
            spray.ResetSpray();

        extinguisher.SetActive(false);
        extinguisher.SetActive(true);
    }
}