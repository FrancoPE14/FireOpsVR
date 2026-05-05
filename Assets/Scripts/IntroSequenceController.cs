using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class IntroSequenceController : MonoBehaviour
{
    [Header("Messages")]
    public GameObject welcomeMessage;
    public GameObject readyMessage;

    [Header("Timer")]
    public GameObject countdownObject;
    public TextMeshProUGUI countdownText;
    public float countdownTime = 15f;

    [Header("Input")]
    public InputActionReference continueAction;

    [Header("Extinguisher")]
    public FireExtinguisherController extinguisherController;

    private enum IntroState
    {
        Welcome,
        Countdown,
        Ready,
        Done
    }

    private IntroState currentState;
    private float timer;

    void OnEnable()
    {
        if (continueAction != null && continueAction.action != null)
            continueAction.action.Enable();
    }

    void OnDisable()
    {
        if (continueAction != null && continueAction.action != null)
            continueAction.action.Disable();
    }

    void Start()
    {
        currentState = IntroState.Welcome;
        timer = countdownTime;

        if (welcomeMessage != null)
            welcomeMessage.SetActive(true);

        if (readyMessage != null)
            readyMessage.SetActive(false);

        if (countdownObject != null)
            countdownObject.SetActive(false);
    }

    void Update()
    {
        if (currentState == IntroState.Welcome)
        {
            if (AnyButtonPressed())
            {
                if (welcomeMessage != null)
                    welcomeMessage.SetActive(false);

                timer = countdownTime;
                currentState = IntroState.Countdown;

                if (countdownObject != null)
                    countdownObject.SetActive(true);

                if (countdownText != null)
                    countdownText.text = Mathf.CeilToInt(timer).ToString();
            }
        }
        else if (currentState == IntroState.Countdown)
        {
            timer -= Time.deltaTime;

            if (countdownText != null)
                countdownText.text = Mathf.CeilToInt(Mathf.Max(timer, 0f)).ToString();

            if (timer <= 0f)
            {
                if (countdownObject != null)
                    countdownObject.SetActive(false);

                if (readyMessage != null)
                    readyMessage.SetActive(true);

                currentState = IntroState.Ready;
            }
        }
        else if (currentState == IntroState.Ready)
        {
            if (AnyButtonPressed())
            {
                if (readyMessage != null)
                    readyMessage.SetActive(false);

                if (extinguisherController != null)
                    extinguisherController.ShowPickupInstruction();

                currentState = IntroState.Done;
            }
        }
    }

    private bool AnyButtonPressed()
    {
        return continueAction != null &&
               continueAction.action != null &&
               continueAction.action.WasPressedThisFrame();
    }
}