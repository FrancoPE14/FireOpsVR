using UnityEngine;

public class Training3WrongExtinguisherDetector : MonoBehaviour
{
    public Training3ScenarioManager scenarioManager;

    public void WrongNozzleTaken()
    {
        if (scenarioManager != null)
            scenarioManager.TriggerWrongExtinguisherFail();
    }
}