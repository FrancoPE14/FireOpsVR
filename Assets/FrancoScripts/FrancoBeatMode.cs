using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class FrancoBeatMode : MonoBehaviour
{
    [Header("Challenge Settings")]
    [Tooltip("Challenge length in seconds. 60 = 1 minute, 90 = 1 minute 30 seconds, 120 = 2 minutes.")]
    public float challengeTime = 60f;

    [Tooltip("If true, the challenge finishes when the original scenario completion objects become active.")]
    public bool autoFinishWhenScenarioCompletes = true;

    private float activeChallengeTime;
    private float timeLeft;
    private bool challengeRunning;

    private GameObject canvasRoot;

    private GameObject startPanel;
    private GameObject hudPanel;
    private GameObject resultPanel;
    private GameObject openBeatModeButton;

    private TMP_Text timerText;
    private TMP_Text finalTimeText;
    private TMP_Text scoreText;
    private TMP_Text rankText;

    // Existing objects from the real scenario.
    // These are optional. The code still works if some are missing.
    private GameObject trainingCompleteObject;
    private GameObject youPassObject;
    private GameObject greatJobObject;

    private void Start()
    {
        FindUIObjects();
        FindScenarioObjects();
        ConnectButtons();

        // The original scenario should start normally.
        // Only the small optional Beat the Clock button is visible.
        HideAllBeatModeUI();
    }

    private void Update()
    {
        if (!challengeRunning)
        {
            return;
        }

        if (autoFinishWhenScenarioCompletes && ScenarioLooksCompleted())
        {
            FinishChallenge(true);
            return;
        }

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            UpdateTimerText();
            FinishChallenge(false);
            return;
        }

        UpdateTimerText();
    }

    public void OpenBeatMode()
    {
        challengeRunning = false;

        SetActiveSafe(openBeatModeButton, false);
        SetActiveSafe(startPanel, true);
        SetActiveSafe(hudPanel, false);
        SetActiveSafe(resultPanel, false);
    }

    public void CloseBeatMode()
    {
        challengeRunning = false;
        HideAllBeatModeUI();
    }

    public void StartChallenge()
    {
        activeChallengeTime = Mathf.Max(1f, challengeTime);
        timeLeft = activeChallengeTime;
        challengeRunning = true;

        SetActiveSafe(openBeatModeButton, false);
        SetActiveSafe(startPanel, false);
        SetActiveSafe(hudPanel, true);
        SetActiveSafe(resultPanel, false);

        UpdateTimerText();
    }

    public void FireExtinguished()
    {
        FinishChallenge(true);
    }

    public void RetryChallenge()
    {
        StartChallenge();
    }

    public void BackToStart()
    {
        challengeRunning = false;

        SetActiveSafe(openBeatModeButton, false);
        SetActiveSafe(startPanel, true);
        SetActiveSafe(hudPanel, false);
        SetActiveSafe(resultPanel, false);
    }

    public void BackToScenario()
    {
        challengeRunning = false;
        HideAllBeatModeUI();
    }

    private void FinishChallenge(bool success)
    {
        challengeRunning = false;

        float timeUsed = Mathf.Clamp(activeChallengeTime - timeLeft, 0f, activeChallengeTime);
        int score = success ? CalculateScore() : 0;
        string rank = GetRank(score, success);

        if (finalTimeText != null)
        {
            finalTimeText.text = "Time Used: " + FormatTime(timeUsed);
        }

        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }

        if (rankText != null)
        {
            rankText.text = "Rank: " + rank;
        }

        SetActiveSafe(openBeatModeButton, false);
        SetActiveSafe(startPanel, false);
        SetActiveSafe(hudPanel, false);
        SetActiveSafe(resultPanel, true);
    }

    private bool ScenarioLooksCompleted()
    {
        if (IsActiveInScene(trainingCompleteObject))
        {
            return true;
        }

        if (IsActiveInScene(youPassObject))
        {
            return true;
        }

        if (IsActiveInScene(greatJobObject))
        {
            return true;
        }

        return false;
    }

    private bool IsActiveInScene(GameObject targetObject)
    {
        return targetObject != null && targetObject.activeInHierarchy;
    }

    private int CalculateScore()
    {
        if (activeChallengeTime <= 0f)
        {
            return 0;
        }

        float remainingPercent = timeLeft / activeChallengeTime;
        int score = Mathf.RoundToInt(remainingPercent * 100f);

        // Small completion bonus so finishing feels rewarding.
        score += 10;

        return Mathf.Clamp(score, 0, 100);
    }

    private string GetRank(int score, bool success)
    {
        if (!success)
        {
            return "Needs Practice";
        }

        if (score >= 90)
        {
            return "Fire Captain";
        }

        if (score >= 75)
        {
            return "Fast Responder";
        }

        if (score >= 60)
        {
            return "Trained Responder";
        }

        return "Needs Practice";
    }

    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            timerText.text = "Time Left: " + FormatTime(timeLeft);
        }
    }

    private string FormatTime(float seconds)
    {
        int totalSeconds = Mathf.CeilToInt(seconds);
        int minutes = totalSeconds / 60;
        int remainingSeconds = totalSeconds % 60;

        return minutes.ToString("00") + ":" + remainingSeconds.ToString("00");
    }

    private void HideAllBeatModeUI()
    {
        SetActiveSafe(startPanel, false);
        SetActiveSafe(hudPanel, false);
        SetActiveSafe(resultPanel, false);

        // Only this small optional button stays visible.
        SetActiveSafe(openBeatModeButton, true);
    }

    private void FindUIObjects()
    {
        canvasRoot = FindGameObjectInScene("Beat The Clock Canvas");

        startPanel = FindChildGameObject("Start Panel");
        hudPanel = FindChildGameObject("HUD Panel");
        resultPanel = FindChildGameObject("Result Panel");
        openBeatModeButton = FindChildGameObject("Open Beat Mode Button");

        timerText = GetTMPText("Timer Text");
        finalTimeText = GetTMPText("Final Time Text");
        scoreText = GetTMPText("Score Text");
        rankText = GetTMPText("Rank Text");
    }

    private void FindScenarioObjects()
    {
        trainingCompleteObject = FindGameObjectInScene("TrainingComplete");
        youPassObject = FindGameObjectInScene("youPass");
        greatJobObject = FindGameObjectInScene("greatJob");
    }

    private void ConnectButtons()
    {
        ConnectButton("Open Beat Mode Button", OpenBeatMode);
        ConnectButton("Start Challenge Button", StartChallenge);
        ConnectButton("Close Beat Mode Button", CloseBeatMode);
        ConnectButton("Fire Extinguished Button", FireExtinguished);
        ConnectButton("Retry Button", RetryChallenge);
        ConnectButton("Back To Start Button", BackToStart);
    }

    private void ConnectButton(string buttonName, UnityAction action)
    {
        GameObject buttonObject = FindChildGameObject(buttonName);

        if (buttonObject == null)
        {
            Debug.LogWarning("Missing button object: " + buttonName);
            return;
        }

        Button button = buttonObject.GetComponent<Button>();

        if (button == null)
        {
            Debug.LogWarning("Missing Button component on: " + buttonName);
            return;
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }

    private TMP_Text GetTMPText(string objectName)
    {
        GameObject textObject = FindChildGameObject(objectName);

        if (textObject == null)
        {
            Debug.LogWarning("Missing text object: " + objectName);
            return null;
        }

        TMP_Text textComponent = textObject.GetComponent<TMP_Text>();

        if (textComponent == null)
        {
            Debug.LogWarning("Missing TMP_Text component on: " + objectName);
            return null;
        }

        return textComponent;
    }

    private GameObject FindChildGameObject(string objectName)
    {
        if (canvasRoot != null)
        {
            Transform[] children = canvasRoot.GetComponentsInChildren<Transform>(true);

            foreach (Transform child in children)
            {
                if (child.name == objectName)
                {
                    return child.gameObject;
                }
            }
        }

        return FindGameObjectInScene(objectName);
    }

    private GameObject FindGameObjectInScene(string objectName)
    {
        Transform[] allTransforms = FindObjectsOfType<Transform>(true);

        foreach (Transform currentTransform in allTransforms)
        {
            if (currentTransform.name == objectName)
            {
                return currentTransform.gameObject;
            }
        }

        return null;
    }

    private void SetActiveSafe(GameObject targetObject, bool isActive)
    {
        if (targetObject != null)
        {
            targetObject.SetActive(isActive);
        }
    }
}