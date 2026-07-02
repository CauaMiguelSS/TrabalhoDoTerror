using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ObjectiveData
{
    public string ObjectiveName;
    public int TargetAmount;
}

public class ObjectiveSystem : MonoBehaviour
{
    public static ObjectiveSystem Instance;

    [Header("UI")]
    [SerializeField] private TMP_Text _objectiveText;
    [SerializeField] private Scrollbar _progressBar;
    [SerializeField] private float _fillSpeed = 0.5f;
    [SerializeField] private LiveSystem _liveSystem;

    [Header("Objectives")]
    [SerializeField] private List<ObjectiveData> _objectives = new List<ObjectiveData>();

    private int _currentObjectiveIndex;
    private int _currentProgress;
    private Coroutine _progressRoutine;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadCurrentObjective();
    }

    private void LoadCurrentObjective()
    {
        if (_currentObjectiveIndex >= _objectives.Count)
        {
            _objectiveText.text = "Reach 1000 Viewers";
            _progressBar.size = 1f;

            CheckVictory();

            return;
        }

        _currentProgress = 0;

        ObjectiveData currentObjective = _objectives[_currentObjectiveIndex];

        _objectiveText.text = currentObjective.ObjectiveName;
        _progressBar.size = 0f;
    }

    public void AddProgress(int amount)
    {
        if (_currentObjectiveIndex >= _objectives.Count)
            return;

        ObjectiveData currentObjective = _objectives[_currentObjectiveIndex];

        _currentProgress += amount;

        if (_currentProgress > currentObjective.TargetAmount)
        {
            _currentProgress = currentObjective.TargetAmount;
        }

        float targetValue =
            (float)_currentProgress / currentObjective.TargetAmount;

        if (_progressRoutine != null)
        {
            StopCoroutine(_progressRoutine);
        }

        _progressRoutine = StartCoroutine(AnimateProgress(targetValue));

        if (_currentProgress >= currentObjective.TargetAmount)
        {
            StartCoroutine(CompleteObjective());
        }
    }

    private IEnumerator AnimateProgress(float targetValue)
    {
        while (_progressBar.size != targetValue)
        {
            _progressBar.size = Mathf.MoveTowards(
                _progressBar.size,
                targetValue,
                _fillSpeed * Time.deltaTime
            );

            yield return null;
        }
    }

    private IEnumerator CompleteObjective()
    {
        yield return new WaitForSeconds(1f);

        _objectiveText.text = "Completed";

        yield return new WaitForSeconds(1.5f);

        _currentObjectiveIndex++;

        LoadCurrentObjective();
    }
    public void CheckVictory()
    {
        if (_currentObjectiveIndex < _objectives.Count)
            return;

        if (_liveSystem == null)
            return;

        if (_liveSystem.CurrentAudience >= 1000)
        {
            _liveSystem.Victory();
        }
    }
}