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

    [Header("Donations")]
    [SerializeField] private TMP_Text _donationText;
    private float _totalDonations;

    [Header("Chat")]
    [SerializeField] private TMP_Text _chatText;

    private Queue<string> _chatMessages = new Queue<string>();

    private void Start()
    {
        UpdateAudienceUI();
        UpdateDonationUI();
    }

    public void TriggerEvent(LiveEventType eventType)
    {
        switch (eventType)
        {
            case LiveEventType.SECRET_DOCUMENT:
                AddAudience(25);
                AddDonation(10f);
                AddChatMessage("WHAT IS THAT?");
                break;

            case LiveEventType.STRANGE_SOUND:
                AddAudience(15);
                AddChatMessage("DID YOU HEAR THAT?");
                break;

            case LiveEventType.STRANGE_MOVEMENT:
                AddAudience(40);
                AddDonation(20f);
                AddChatMessage("SOMETHING MOVED");
                break;

            case LiveEventType.ROBOT_CHASE:
                AddAudience(100);
                AddDonation(50f);
                AddChatMessage("RUN RUN RUN");
                break;

            default:
                break;
        }
    }

    private void AddAudience(int amount)
    {
        _audienceCount += amount;
        UpdateAudienceUI();
    }

    private void AddDonation(float amount)
    {
        _totalDonations += amount;
        UpdateDonationUI();
    }

    private void AddChatMessage(string message)
    {
        _chatMessages.Enqueue(message);

        if (_chatMessages.Count > 6)
        {
            _chatMessages.Dequeue();
        }

        _chatText.text = string.Join("\n", _chatMessages);
    }

    private void UpdateAudienceUI()
    {
        _audienceText.text = "Viewers: " + _audienceCount;
    }

    private void UpdateDonationUI()
    {
        _donationText.text = "$" + _totalDonations.ToString("F2");
    }
}