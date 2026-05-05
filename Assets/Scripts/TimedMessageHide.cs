using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimedMessageHide : MonoBehaviour
{
    public float duration = 10f;
    public TextMeshProUGUI timerText;

    [Header("Hide Immediately When This Message Appears")]
    public GameObject[] objectsToHide;

    [Header("Buttons To Enable After Timer")]
    public Button[] buttonsToEnable;

    private float timer;

    void Start()
    {
        SetButtonsInteractable(false);
    }

    void OnEnable()
    {
        timer = duration;

        HideExtraObjects(); // hides extinguisher immediately

        UpdateTimerText();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        UpdateTimerText();

        if (timer <= 0f)
        {
            SetButtonsInteractable(true);
            gameObject.SetActive(false);
        }
    }

    private void HideExtraObjects()
    {
        if (objectsToHide == null) return;

        foreach (GameObject obj in objectsToHide)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }

    private void SetButtonsInteractable(bool state)
    {
        if (buttonsToEnable == null) return;

        foreach (Button btn in buttonsToEnable)
        {
            if (btn != null)
                btn.interactable = state;
        }
    }

    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            timerText.text = Mathf.CeilToInt(Mathf.Max(timer, 0f)) + " s";
        }
    }
}