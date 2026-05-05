using UnityEngine;

public class MultiFireTrainingManager : MonoBehaviour
{
    [Header("Fire Targets")]
    public ExtinguishableFire[] targetFires;

    [Header("Scenario Manager")]
    public Training2ScenarioManager training2ScenarioManager;

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

                if (training2ScenarioManager != null)
                    training2ScenarioManager.TriggerPass();

                break;
            }
        }
    }
}