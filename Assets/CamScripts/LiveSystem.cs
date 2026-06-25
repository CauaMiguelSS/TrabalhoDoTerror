using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum LiveEventType
{
    NONE,
    SECRET_DOCUMENT,
    RARE_DOCUMENT,
    CLASSIFIED_DOCUMENT,
    STRANGE_SOUND,
    STRANGE_MOVEMENT,
    ROBOT_CHASE,
    DEAD_BODY
}

public class LiveSystem : MonoBehaviour
{
    public static LiveSystem Instance;

    [Header("Audience")]
    [SerializeField] private TMP_Text _audienceText;
    [SerializeField] private TMP_Text _audiencePopupText;
    [SerializeField] private float _popupDuration = 1f;
    [SerializeField] private float _idleTimeToLoseViewers = 6f;
    [SerializeField] private int _viewLossAmount = 2;

    private int _audienceCount = 3;
    private bool _isUpdatingAudience;
    private float _idleTimer;

    [Header("Chat")]
    [SerializeField] private TMP_Text[] _chatSlots;
    [SerializeField] private float _messageDuration = 4f;

    private List<string> _chatMessages = new List<string>();
    private Coroutine _popupRoutine;

    private readonly string[] _documentMessages =
    {
        "What is that document?",
        "Read that bro",
        "That looks important",
        "No way that's real",
        "Show that to us again",
        "What did you find?"
    };

    private readonly string[] _soundMessages =
    {
        "Did you hear that?",
        "What was that noise?",
        "Go back",
        "Check behind you"
    };

    private readonly string[] _movementMessages =
    {
        "Something moved",
        "I saw that",
        "Bro run",
        "WHAT WAS THAT"
    };

    private readonly string[] _robotMessages =
    {
        "RUN RUN RUN",
        "DON'T LOOK BACK",
        "MOVE",
        "HE'S BEHIND YOU",
        "LMAO",
        "OMG"
    };

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateAudienceUI(_audienceCount);
        ClearChat();

        _audiencePopupText.text = "";
        _idleTimer = _idleTimeToLoseViewers;
    }

    private void Update()
    {
        _idleTimer -= Time.deltaTime;

        if (_idleTimer <= 0f)
        {
            RemoveAudience(_viewLossAmount);
            _idleTimer = _idleTimeToLoseViewers;
        }
    }

    public void ResetIdleTimer()
    {
        _idleTimer = _idleTimeToLoseViewers;
    }

    public void TriggerEvent(LiveEventType eventType)
    {
        ResetIdleTimer();

        switch (eventType)
        {
            case LiveEventType.SECRET_DOCUMENT:
                AddAudience(25);
                AddChatMessage(GetRandomMessage(_documentMessages));
                break;

            case LiveEventType.STRANGE_SOUND:
                AddAudience(15);
                AddChatMessage(GetRandomMessage(_soundMessages));
                break;

            case LiveEventType.STRANGE_MOVEMENT:
                AddAudience(40);
                AddChatMessage(GetRandomMessage(_movementMessages));
                break;

            case LiveEventType.ROBOT_CHASE:
                AddAudience(100);
                AddChatMessage(GetRandomMessage(_robotMessages));
                break;

            case LiveEventType.RARE_DOCUMENT:
                AddAudience(75);
                AddChatMessage("That's important");
                break;

            case LiveEventType.CLASSIFIED_DOCUMENT:
                AddAudience(150);
                AddChatMessage("NO WAY");
                break;

            case LiveEventType.DEAD_BODY:
                AddAudience(300);
                AddChatMessage("IS THAT A BODY?");
                break;

        }
    }

    private void AddAudience(int amount)
    {
        ShowAudiencePopup(amount);

        if (_isUpdatingAudience)
            return;

        StartCoroutine(UpdateAudienceRoutine(amount));
    }

    private void RemoveAudience(int amount)
    {
        ShowAudiencePopup(-amount);

        _audienceCount -= amount;

        if (_audienceCount < 0)
        {
            _audienceCount = 0;
        }

        UpdateAudienceUI(_audienceCount);
    }

    private IEnumerator UpdateAudienceRoutine(int amount)
    {
        _isUpdatingAudience = true;

        int targetAudience = _audienceCount + amount;
        int visualAudience = _audienceCount;

        while (visualAudience < targetAudience)
        {
            visualAudience += Random.Range(1, 8);

            if (visualAudience > targetAudience)
            {
                visualAudience = targetAudience;
            }

            UpdateAudienceUI(visualAudience + Random.Range(-2, 3));

            yield return new WaitForSeconds(0.05f);
        }

        _audienceCount = targetAudience;
        UpdateAudienceUI(_audienceCount);

        _isUpdatingAudience = false;
    }

    private void AddChatMessage(string message)
    {
        _chatMessages.Add(message);

        if (_chatMessages.Count > _chatSlots.Length)
        {
            _chatMessages.RemoveAt(0);
        }

        UpdateChatUI();

        StartCoroutine(RemoveMessageRoutine(message));
    }

    private IEnumerator RemoveMessageRoutine(string message)
    {
        yield return new WaitForSeconds(_messageDuration);

        int messageIndex = _chatMessages.IndexOf(message);

        if (messageIndex == -1)
            yield break;

        TMP_Text targetText = _chatSlots[messageIndex];

        float fadeTimer = 1f;
        Color originalColor = targetText.color;

        while (fadeTimer > 0f)
        {
            fadeTimer -= Time.deltaTime * 2f;

            Color newColor = originalColor;
            newColor.a = fadeTimer;

            targetText.color = newColor;

            yield return null;
        }

        _chatMessages.Remove(message);
        UpdateChatUI();
    }

    private void UpdateChatUI()
    {
        for (int i = 0; i < _chatSlots.Length; i++)
        {
            _chatSlots[i].text = "";
        }

        int startIndex = Mathf.Max(0, _chatMessages.Count - _chatSlots.Length);

        for (int i = 0; i < _chatMessages.Count - startIndex; i++)
        {
            _chatSlots[i].text = _chatMessages[startIndex + i];

            Color textColor = _chatSlots[i].color;
            textColor.a = 1f;
            _chatSlots[i].color = textColor;

            StartCoroutine(ChatPopEffect(_chatSlots[i]));
        }
    }

    private IEnumerator ChatPopEffect(TMP_Text targetText)
    {
        RectTransform textTransform = targetText.rectTransform;

        Vector3 originalScale = Vector3.one;
        Vector3 popScale = Vector3.one * 1.15f;

        float timer = 0f;

        while (timer < 0.12f)
        {
            timer += Time.deltaTime * 8f;
            textTransform.localScale = Vector3.Lerp(originalScale, popScale, timer);
            yield return null;
        }

        timer = 0f;

        while (timer < 0.12f)
        {
            timer += Time.deltaTime * 8f;
            textTransform.localScale = Vector3.Lerp(popScale, originalScale, timer);
            yield return null;
        }

        textTransform.localScale = originalScale;
    }

    private void ShowAudiencePopup(int amount)
    {
        if (_popupRoutine != null)
        {
            StopCoroutine(_popupRoutine);
        }

        _popupRoutine = StartCoroutine(AudiencePopupRoutine(amount));
    }

    private IEnumerator AudiencePopupRoutine(int amount)
    {
        RectTransform popupTransform = _audiencePopupText.rectTransform;

        Vector3 startPosition = popupTransform.localPosition;
        Vector3 targetPosition = startPosition + Vector3.up * 20f;

        Color popupColor = _audiencePopupText.color;

        popupColor.a = 1f;
        _audiencePopupText.color = popupColor;

        _audiencePopupText.text = amount > 0
            ? "+" + amount
            : amount.ToString();

        float timer = 0f;

        while (timer < _popupDuration)
        {
            timer += Time.deltaTime;

            popupTransform.localPosition = Vector3.Lerp(
                startPosition,
                targetPosition,
                timer / _popupDuration
            );

            popupColor.a = Mathf.Lerp(1f, 0f, timer / _popupDuration);
            _audiencePopupText.color = popupColor;

            yield return null;
        }

        popupTransform.localPosition = startPosition;
        _audiencePopupText.text = "";
    }

    private void UpdateAudienceUI(int amount)
    {
        _audienceText.text = amount.ToString();
    }

    private string GetRandomMessage(string[] pool)
    {
        return pool[Random.Range(0, pool.Length)];
    }

    private void ClearChat()
    {
        for (int i = 0; i < _chatSlots.Length; i++)
        {
            _chatSlots[i].text = "";
        }
    }
}