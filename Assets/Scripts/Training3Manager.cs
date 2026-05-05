using UnityEngine;

public class Training3FirePassManager : MonoBehaviour
{
    public ExtinguishableFire[] targetFires;
    public Training3ScenarioManager scenarioManager;

    private bool completed = false;

    void Update()
    {
        if (completed) return;
        if (targetFires == null) return;

        foreach (ExtinguishableFire fire in targetFires)
        {
            if (fire != null && fire.IsExtinguished)
            {
                completed = true;

                if (scenarioManager != null)
                    scenarioManager.TriggerPass();

                break;
            }
        }
    }
}