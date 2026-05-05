using UnityEngine;

public class WrongExtinguisherFail : MonoBehaviour
{
    public SprayController sprayController;
    public Training2ScenarioManager scenarioManager;

    public float failAfterSpraySeconds = 5f;

    private float sprayTimer = 0f;
    private bool failed = false;

    void OnEnable()
    {
        ResetFailDetector();
    }

    void Update()
    {
        if (failed) return;
        if (sprayController == null || scenarioManager == null) return;

        if (sprayController.IsActivelySpraying())
        {
            sprayTimer += Time.deltaTime;

            if (sprayTimer >= failAfterSpraySeconds)
            {
                failed = true;
                scenarioManager.TriggerFail();
            }
        }
        else
        {
            sprayTimer = 0f;
        }
    }

    public void ResetFailDetector()
    {
        sprayTimer = 0f;
        failed = false;
    }
}