using UnityEngine;
using UnityEngine.InputSystem;

public class SprayController : MonoBehaviour
{
    public ParticleSystem sprayParticles;
    public InputActionReference triggerAction;

    [Header("Optional State Checks")]
    public bool requirePinPulled = true;
    public bool requireNozzleGrabbed = true;
    public bool requireNozzleHeld = true; // IMPORTANT: keep this true

    public FireExtinguisherController extinguisherController;
    public NozzleHandler nozzleHandler;

    public float agentLevel = 100f;
    public float drainRate = 0.001f;
    public float sprayThreshold = 0.5f;

    private bool isSpraying = false;
    public SpraySoundController spraySoundController;
    void OnEnable()
    {
        if (triggerAction != null && triggerAction.action != null)
            triggerAction.action.Enable();

        StopSpray();
    }

    void OnDisable()
    {
        StopSpray();
    }

    void Update()
    {
        if (sprayParticles == null) return;
        if (triggerAction == null || triggerAction.action == null) return;

        if (!CanSpray())
        {
            StopSpray();
            return;
        }

        float squeeze = triggerAction.action.ReadValue<float>();

        if (squeeze >= sprayThreshold)
        {
            StartSpray();

            agentLevel -= Time.deltaTime * drainRate;
            agentLevel = Mathf.Max(agentLevel, 0f);
        }
        else
        {
            StopSpray();
        }
    }

    private bool CanSpray()
    {
        if (agentLevel <= 0f)
            return false;

        if (requirePinPulled)
        {
            if (extinguisherController == null || !extinguisherController.pinPulled)
                return false;
        }

        if (requireNozzleGrabbed)
        {
            if (extinguisherController == null || !extinguisherController.nozzleGrabbed)
                return false;
        }

        if (requireNozzleHeld)
        {
            if (nozzleHandler == null || !nozzleHandler.IsNozzleHeld())
                return false;
        }

        return true;
    }

    private void StartSpray()
    {
        if (isSpraying) return;

        isSpraying = true;

        if (spraySoundController != null)
            spraySoundController.StartSpraySound();

        if (sprayParticles != null && !sprayParticles.isPlaying)
            sprayParticles.Play();
    }

    private void StopSpray()
    {
        isSpraying = false;

        if (sprayParticles != null && sprayParticles.isPlaying)
            sprayParticles.Stop();

        if (spraySoundController != null)
            spraySoundController.StopSpraySound();
    }

    public bool IsActivelySpraying()
    {
        return isSpraying;
    }

    public void ResetSpray()
    {
        agentLevel = 100f;
        StopSpray();
    }
}