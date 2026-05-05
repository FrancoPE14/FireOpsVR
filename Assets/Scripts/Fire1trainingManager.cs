using UnityEngine;

public class SingleFireTrainingManager : MonoBehaviour
{
    [Header("Fire Targets")]
    public ExtinguishableFire[] targetFires1;

    [Header("Objects To Hide When Any Fire Is Extinguished")]
    public GameObject[] fireObjectsToHide;

    [Header("Pass Message")]
    public GameObject passMessageObject;

    private bool messageShown = false;

    void Start()
    {
        if (passMessageObject != null)
            passMessageObject.SetActive(false);
    }

    void Update()
    {
        if (messageShown) return;
        if (targetFires1 == null) return;

        foreach (ExtinguishableFire fire in targetFires1)
        {
            if (fire != null && fire.IsExtinguished)
            {
                messageShown = true;

                HideAllFires();

                if (passMessageObject != null)
                    passMessageObject.SetActive(true);

                break;
            }
        }
    }

    private void HideAllFires()
    {
        if (fireObjectsToHide == null) return;

        foreach (GameObject fireObj in fireObjectsToHide)
        {
            if (fireObj != null)
                fireObj.SetActive(false);
        }
    }
}