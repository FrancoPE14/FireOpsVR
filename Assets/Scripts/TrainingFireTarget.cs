using UnityEngine;

public class TrainingFireTarget : MonoBehaviour
{
    [Header("Fire Objects")]
    public GameObject fireVisualRoot;
    public GameObject sweepInstructionText;
    public GameObject completionMessageObject;

    [Header("Player")]
    public Transform playerCamera;
    public float messageDistance = 1.2f;
    public Vector3 messageOffset = new Vector3(0f, -0.05f, 0f);

    [Header("Training Settings")]
    public float requiredSprayTime = 2.5f;
    public float loseProgressSpeed = 0.5f;

    private float sprayTimer = 0f;
    private bool completed = false;
    private float timeSinceLastHit = 999f;

    void Start()
    {
        if (fireVisualRoot != null)
            fireVisualRoot.SetActive(false);

        if (sweepInstructionText != null)
            sweepInstructionText.SetActive(false);

        if (completionMessageObject != null)
            completionMessageObject.SetActive(false);
    }

    void Update()
    {
        if (completed) return;

        timeSinceLastHit += Time.deltaTime;

        // If user stops hitting the fire, slowly lose progress
        if (timeSinceLastHit > 0.15f)
        {
            sprayTimer -= Time.deltaTime * loseProgressSpeed;
            if (sprayTimer < 0f)
                sprayTimer = 0f;
        }
    }

    public void ShowPracticeFire()
    {
        if (fireVisualRoot != null)
            fireVisualRoot.SetActive(true);
    }

    public void HidePracticeFire()
    {
        if (fireVisualRoot != null)
            fireVisualRoot.SetActive(false);
    }

    public void ShowSweepInstruction()
    {
        if (sweepInstructionText != null)
            sweepInstructionText.SetActive(true);
    }

    public void HideSweepInstruction()
    {
        if (sweepInstructionText != null)
            sweepInstructionText.SetActive(false);
    }

    public void RegisterSprayHit(float hitAmount)
    {
        if (completed) return;

        timeSinceLastHit = 0f;
        sprayTimer += hitAmount;

        if (sprayTimer >= requiredSprayTime)
        {
            CompleteTrainingFire();
        }
    }

    private void CompleteTrainingFire()
    {
        completed = true;

        HidePracticeFire();
        HideSweepInstruction();
        ShowCompletionMessage();
    }
    private void ShowCompletionMessage()
    {
        if (completionMessageObject == null || playerCamera == null)
            return;

        Vector3 forward = playerCamera.forward;
        forward.y = 0f;
        forward.Normalize();

        if (forward.sqrMagnitude < 0.001f)
            forward = playerCamera.forward;

        completionMessageObject.transform.position =
            playerCamera.position + forward * 1.2f + new Vector3(0f, -0.05f, 0f);

        Vector3 lookDir = completionMessageObject.transform.position - playerCamera.position;
        if (lookDir.sqrMagnitude > 0.001f)
        {
            completionMessageObject.transform.rotation =
                Quaternion.LookRotation(-lookDir.normalized);
        }

        completionMessageObject.SetActive(true);
    }
}