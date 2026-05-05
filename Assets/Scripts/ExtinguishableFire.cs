using UnityEngine;

public class ExtinguishableFire : MonoBehaviour
{
    [Header("Fire")]
    public GameObject fireVisualRoot;
    public float requiredSprayTime = 2f;

    [Header("After Fire Is Out")]
    public GameObject sweepInstructionText;
    public GameObject passMessageObject;
    public Transform playerCamera;
    public float messageDistance = 1.2f;
    public Vector3 messageOffset = new Vector3(0f, 0.05f, 0f);

    private float sprayTimer = 0f;
    private bool extinguished = false;

    public bool IsExtinguished => extinguished;

    public void RegisterSprayHit(float amount)
    {
        if (extinguished) return;

        sprayTimer += amount;
        Debug.Log("Fire spray progress: " + sprayTimer);

        if (sprayTimer >= requiredSprayTime)
        {
            ExtinguishFire();
        }
    }

    private void ExtinguishFire()
    {
        extinguished = true;

        if (fireVisualRoot != null)
            fireVisualRoot.SetActive(false);

        if (sweepInstructionText != null)
            sweepInstructionText.SetActive(false);

        ShowPassMessage();

        Debug.Log("Fire extinguished and completion message shown!");
    }

    private void ShowPassMessage()
    {
        if (passMessageObject == null || playerCamera == null)
            return;

        Vector3 forward = playerCamera.forward;
        forward.y = 0f;
        forward.Normalize();

        if (forward.sqrMagnitude < 0.001f)
            forward = playerCamera.forward;

        passMessageObject.transform.position =
            playerCamera.position + forward * messageDistance + messageOffset;

        Vector3 lookDir = passMessageObject.transform.position - playerCamera.position;

        if (lookDir.sqrMagnitude > 0.001f)
            passMessageObject.transform.rotation = Quaternion.LookRotation(-lookDir.normalized);

        passMessageObject.SetActive(true);
    }
}