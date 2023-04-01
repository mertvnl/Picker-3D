using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSystem : Singleton<LevelSystem>
{
    [SerializeField] private LevelDatabase levelDatabase;

    public bool IsLevelStarted { get; private set; }
    public Vector3 NextLevelStartPosition => _nextLevelPrefab.transform.position;

    public Event OnLevelLoadingStarted = new Event();
    public Event OnLevelLoaded = new Event();
    public Event OnLevelStarted = new Event();
    public Event OnLevelFinished = new Event();

    private int _currentLevelIndex;
    private bool _isLevelLoading;
    private float _levelOffset;
    private GameObject _nextLevelPrefab;
    private List<GameObject> _spawnedLevelPrefabs = new List<GameObject>();

    private const string LAST_LEVEL_KEY = "LastLevelIndex";

    private void OnEnable()
    {
        Events.OnLevelSuccess.AddListener(DestroyPreviousLevel);
    }

    private void OnDisable()
    {
        Events.OnLevelSuccess.RemoveListener(DestroyPreviousLevel);
    }

    public void StartLevel()
    {
        if (IsLevelStarted) return;

        IsLevelStarted = true;
        OnLevelStarted.Invoke();
    }

    public void FinishLevel()
    {
        if (!IsLevelStarted) return;

        IsLevelStarted = false;
        OnLevelFinished.Invoke();
    }

    public void LoadMainLevel()
    {
        if (_isLevelLoading) return;

        StartCoroutine(LoadLevelCo());
    }

    private IEnumerator LoadLevelCo()
    {
        _levelOffset = 0;
        IsLevelStarted = false;
        _isLevelLoading = true;
        OnLevelLoadingStarted.Invoke();
        yield return new WaitForSeconds(1f);
        yield return SceneManager.LoadSceneAsync(0);
        SpawnLastLevel();
        yield return new WaitForSeconds(0.5f);
        OnLevelLoaded.Invoke();
        _isLevelLoading = false;
    }

    public void SpawnNextLevel()
    {
        int nextLevelIndex = _currentLevelIndex + 1;

        SpawnLevel(nextLevelIndex);
    }

    public void SpawnLastLevel()
    {
        int lastLevelIndex = GetLastLevelIndex();

        SpawnLevel(lastLevelIndex);
    }

    private void SpawnLevel(int levelIndex)
    {
        if (levelIndex > GetLevelCount())
            levelIndex = 0;

        LevelData levelData = levelDatabase.GetLevelDataByIndex(levelIndex);

        Vector3 spawnPosition = Vector3.forward * _levelOffset;

        _nextLevelPrefab = Instantiate(levelData.LevelPrefab, spawnPosition, Quaternion.identity);
        _spawnedLevelPrefabs.Add(_nextLevelPrefab);

        _levelOffset += levelData.LevelLength;
        _currentLevelIndex = levelIndex;
    }

    private void DestroyPreviousLevel()
    {
        if (_spawnedLevelPrefabs.Count < 3)
            return;

        SaveLoadSystem.SetInt(LAST_LEVEL_KEY, _currentLevelIndex);
        GameObject levelToDestroy = _spawnedLevelPrefabs[0];
        _spawnedLevelPrefabs.Remove(levelToDestroy);
        Destroy(levelToDestroy);
    }

    public int GetLevelCount()
    {
        return levelDatabase.Levels.Count - 1;
    }

    public int GetLastLevelIndex()
    {
        return SaveLoadSystem.GetInt(LAST_LEVEL_KEY, 0);
    }
}