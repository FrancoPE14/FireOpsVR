using UnityEngine;

public class Training3ScenarioManager : MonoBehaviour
{
    [Header("Wrong Extinguisher")]
    public GameObject wrongExtinguisher;
    public GameObject wrongFailMessage;

    [Header("Correct CO2 Extinguisher")]
    public GameObject correctCO2Extinguisher;

    [Header("Pass Message")]
    public GameObject passMessage;

    [Header("Fire Objects")]
    public GameObject[] firesToHideOnPass;

    private bool failedOnce = false;
    private bool passed = false;

    void Start()
    {
        if (wrongFailMessage != null)
            wrongFailMessage.SetActive(false);

        if (passMessage != null)
            passMessage.SetActive(false);
    }

    public void TriggerWrongExtinguisherFail()
    {
        if (failedOnce || passed) return;

        failedOnce = true;

        if (wrongFailMessage != null)
            wrongFailMessage.SetActive(true);

        Debug.Log("Training 3 fail: wrong extinguisher selected.");
    }

    public void TriggerPass()
    {
        if (passed) return;

        passed = true;

        HideAllFires();

        if (passMessage != null)
            passMessage.SetActive(true);

        Debug.Log("Training 3 passed.");
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
}