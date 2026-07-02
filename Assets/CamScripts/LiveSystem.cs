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
    public static LiveSystem Instance { get; private set; }

    [Header("Victory")]
    [SerializeField] private int _viewerGoal = 1000;
    [SerializeField] private GameObject _victoryPanel;
    private bool _victoryTriggered;

    [Header("Audience")]
    [SerializeField] private TMP_Text _audienceText;
    [SerializeField] private TMP_Text _audiencePopupText;
    [SerializeField] private float _popupDuration = 1f;
    [SerializeField] private float _idleTimeToLoseViewers = 6f;
    [SerializeField] private int _viewLossAmount = 2;

    private int _audienceCount = 3;
    public int CurrentAudience => _audienceCount;
    private bool _isUpdatingAudience;
    private float _idleTimer;

    [Header("Event Chat")]
    [SerializeField] private TMP_Text[] _chatSlots;
    [SerializeField] private float _messageDuration = 4f;

    [Header("Chat Pools (Configurável na Unity)")]
    [SerializeField] private string[] _normalMessages;
    [SerializeField] private string[] _panicMessages;
    [SerializeField] private string[] _documentMessages;
    [SerializeField] private string[] _soundMessages;
    [SerializeField] private string[] _movementMessages;

    [Header("Panic Settings")]
    [SerializeField] private float _panicMessageDelay = 0.3f;
    [SerializeField] private float _panicDuration = 8f;

    private List<string> _chatMessages = new List<string>();
    private Coroutine _popupRoutine;
    private Dictionary<TMP_Text, Coroutine> _activePopEffects = new Dictionary<TMP_Text, Coroutine>();

    private const float AUDIENCE_TICK_RATE = 0.02f;
    private WaitForSeconds _audienceTickWait = new WaitForSeconds(AUDIENCE_TICK_RATE);

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateAudienceUI(_audienceCount);
        ClearChat();

        if (_audiencePopupText != null) _audiencePopupText.text = "";
        _idleTimer = _idleTimeToLoseViewers;

        StartCoroutine(NormalChatRoutine());

        if (_victoryPanel != null) _victoryPanel.SetActive(false);
    }

    private void Update()
    {
        _idleTimer -= Time.deltaTime;
        if (_idleTimer <= 0f)
        {
            RemoveAudience(_viewLossAmount);
            ResetIdleTimer();
        }
    }

    public void ResetIdleTimer()
    {
        _idleTimer = _idleTimeToLoseViewers;
    }

    public float GetChatDelay()
    {
        if (_audienceCount >= 300) return Random.Range(1f, 3f);
        if (_audienceCount >= 100) return Random.Range(2f, 5f);
        if (_audienceCount >= 50) return Random.Range(4f, 7f);
        if (_audienceCount >= 10) return Random.Range(6f, 9f);
        return Random.Range(8f, 12f);
    }

    private IEnumerator NormalChatRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(GetChatDelay());

            if (_normalMessages != null && _normalMessages.Length > 0)
            {
                AddChatMessage(GetRandomMessage(_normalMessages));
            }
        }
    }

    private IEnumerator PanicRoutine()
    {
        float timer = 0f;
        WaitForSeconds delay = new WaitForSeconds(_panicMessageDelay);

        while (timer < _panicDuration)
        {
            if (_panicMessages != null && _panicMessages.Length > 0)
            {
                AddChatMessage(GetRandomMessage(_panicMessages));
            }

            yield return delay;
            timer += _panicMessageDelay;
        }
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
                StartCoroutine(PanicRoutine());
                break;
        }
    }

    private void AddAudience(int amount)
    {
        ShowAudiencePopup(amount);

        if (_isUpdatingAudience)
        {
            _audienceCount += amount;
            return;
        }

        StartCoroutine(UpdateAudienceRoutine(amount));
    }

    private void RemoveAudience(int amount)
    {
        ShowAudiencePopup(-amount);
        _audienceCount = Mathf.Max(0, _audienceCount - amount);
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
            if (visualAudience > targetAudience) visualAudience = targetAudience;

            UpdateAudienceUI(visualAudience + Random.Range(-2, 3));
            yield return _audienceTickWait;
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

        if (_chatMessages.Contains(message))
        {
            _chatMessages.Remove(message);
            UpdateChatUI();
        }
    }

    private void UpdateChatUI()
    {
        ClearChat();

        int slotIndex = _chatSlots.Length - 1;

        for (int i = _chatMessages.Count - 1; i >= 0 && slotIndex >= 0; i--)
        {
            TMP_Text slot = _chatSlots[slotIndex];
            slot.text = _chatMessages[i];

            Color c = slot.color;
            c.a = 1f;
            slot.color = c;

            if (_activePopEffects.TryGetValue(slot, out Coroutine activeRoutine))
            {
                if (activeRoutine != null) StopCoroutine(activeRoutine);
            }
            _activePopEffects[slot] = StartCoroutine(ChatPopEffect(slot));

            slotIndex--;
        }
    }

    private IEnumerator ChatPopEffect(TMP_Text targetText)
    {
        RectTransform textTransform = targetText.rectTransform;

        Vector3 originalScale = Vector3.one;
        Vector3 popScale = Vector3.one * 1.15f;

        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime * 8f;
            textTransform.localScale = Vector3.Lerp(originalScale, popScale, timer);
            yield return null;
        }

        timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime * 8f;
            textTransform.localScale = Vector3.Lerp(popScale, originalScale, timer);
            yield return null;
        }

        textTransform.localScale = originalScale;
        _activePopEffects[targetText] = null;
    }

    private void ShowAudiencePopup(int amount)
    {
        if (_audiencePopupText == null) return;
        if (_popupRoutine != null) StopCoroutine(_popupRoutine);
        _popupRoutine = StartCoroutine(AudiencePopupRoutine(amount));
    }

    private IEnumerator AudiencePopupRoutine(int amount)
    {
        RectTransform popupTransform = _audiencePopupText.rectTransform;

        Vector3 startPosition = popupTransform.localPosition;
        Vector3 targetPosition = startPosition + Vector3.up * 20f;

        Color popupColor = _audiencePopupText.color;

        _audiencePopupText.text = amount > 0 ? $"+{amount}" : amount.ToString();

        float timer = 0f;

        while (timer < _popupDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / _popupDuration;

            popupTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, progress);
            popupColor.a = Mathf.Lerp(1f, 0f, progress);
            _audiencePopupText.color = popupColor;

            yield return null;
        }

        popupTransform.localPosition = startPosition;
        _audiencePopupText.text = "";
    }

    private void UpdateAudienceUI(int amount)
    {
        if (_audienceText != null) _audienceText.text = amount.ToString();

        if (!_victoryTriggered && amount >= _viewerGoal)
        {
            Victory();
        }

        if (ObjectiveSystem.Instance != null)
        {
            ObjectiveSystem.Instance.CheckVictory();
        }
    }

    private string GetRandomMessage(string[] pool)
    {
        if (pool == null || pool.Length == 0) return "...";
        return pool[Random.Range(0, pool.Length)];
    }

    private void ClearChat()
    {
        for (int i = 0; i < _chatSlots.Length; i++)
        {
            if (_chatSlots[i] != null) _chatSlots[i].text = "";
        }
    }

    public void Victory()
    {
        _victoryTriggered = true;

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (_victoryPanel != null) _victoryPanel.SetActive(true);
    }
}