using UnityEngine;
using UnityEngine.InputSystem;

public class Training2ScenarioManager : MonoBehaviour
{
    [Header("Extinguishers")]
    public GameObject wrongExtinguisher;
    public GameObject correctExtinguisher;

    [Header("Messages")]
    public GameObject failMessage;
    public GameObject passMessage;

    [Header("Fire Objects")]
    public GameObject[] firesToHideOnPass;

    [Header("Input")]
    public InputActionReference retryAction; // A button

    private bool failed = false;
    private bool passed = false;
    private Vector3 wrongStartPosition;
    private Quaternion wrongStartRotation;

    void OnEnable()
    {
        if (retryAction != null && retryAction.action != null)
            retryAction.action.Enable();
    }

    void Start()
    {
        if (wrongExtinguisher != null)
        {
            wrongStartPosition = wrongExtinguisher.transform.position;
            wrongStartRotation = wrongExtinguisher.transform.rotation;
        }
        if (failMessage != null) failMessage.SetActive(false);
        if (passMessage != null) passMessage.SetActive(false);

        if (wrongExtinguisher != null) wrongExtinguisher.SetActive(true);
        if (correctExtinguisher != null) correctExtinguisher.SetActive(true);
    }


    void Update()
    {
        if (failMessage != null && failMessage.activeSelf)
        {
            if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
            {
                Debug.Log("R pressed directly");
                failMessage.SetActive(false);
            }
        }
    }
    private void HideFailMessage()
    {
        if (failMessage != null)
            failMessage.SetActive(false);
    }

    public void TriggerFail()
    {
        if (failed || passed) return;

        failed = true;
        ResetWrongExtinguisher();

        if (failMessage != null)
            failMessage.SetActive(true);
    }

    public void TriggerPass()
    {
        if (passed) return;

        passed = true;

        HideAllFires();

        if (passMessage != null)
            passMessage.SetActive(true);
    }


    private void HideAllFires()
    {
        if (firesToHideOnPass == null) return;

        foreach (GameObject fire in firesToHideOnPass)
        {
            if (fire != null)
                fire.SetActive(false);
        }
    }
    private void ResetWrongExtinguisher()
    {
        if (wrongExtinguisher == null) return;

        Rigidbody rb = wrongExtinguisher.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        wrongExtinguisher.transform.position = wrongStartPosition;
        wrongExtinguisher.transform.rotation = wrongStartRotation;
        wrongExtinguisher.SetActive(true);
    }
 
}