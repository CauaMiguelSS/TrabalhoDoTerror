using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum LiveEventType
{
    NONE,
    SECRET_DOCUMENT,
    STRANGE_SOUND,
    STRANGE_MOVEMENT,
    ROBOT_CHASE
}

public class LiveSystem : MonoBehaviour
{
    [Header("Audience")]
    [SerializeField] private TMP_Text _audienceText;
    private int _audienceCount = 3;
    private bool _isUpdatingAudience;

    [Header("Chat")]
    [SerializeField] private TMP_Text[] _chatSlots;
    [SerializeField] private float _messageDuration = 4f;

    private List<string> _chatMessages = new List<string>();

    private string[] _documentMessages =
    {
        "What is that document?",
        "Read that bro",
        "That looks important",
        "No way that's real",
        "Show that to us again",
        "What did you find?"
    };

    private string[] _soundMessages =
    {
        "Did you hear that?",
        "What was that noise?",
        "Go back",
        "Check behind you"
    };

    private string[] _movementMessages =
    {
        "Something moved",
        "I saw that",
        "Bro run",
        "WHAT WAS THAT"
    };

    private string[] _robotMessages =
    {
        "RUN RUN RUN",
        "DON'T LOOK BACK",
        "MOVE",
        "HE'S BEHIND YOU"
    };

    private void Start()
    {
        UpdateAudienceUI(_audienceCount);
        ClearChat();
    }

    public void TriggerEvent(LiveEventType eventType)
    {
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
        }
    }

    private void AddAudience(int amount)
    {
        if (_isUpdatingAudience)
            return;

        StartCoroutine(UpdateAudienceRoutine(amount));
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
                visualAudience = targetAudience;

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

        if (_chatMessages.Contains(message))
        {
            _chatMessages.Remove(message);
            UpdateChatUI();
        }

        for (int i = 0; i < _chatSlots.Length; i++)
        {
            Color resetColor = _chatSlots[i].color;
            resetColor.a = 1f;
            _chatSlots[i].color = resetColor;
        }
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
        }
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