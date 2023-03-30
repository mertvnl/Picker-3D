using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSystem : Singleton<LevelSystem>
{
    [SerializeField] private LevelDatabase levelDatabase;

    public bool IsLevelStarted { get; set; }

    private int currentLevelIndex;
    private bool isLevelLoading;

    public Event OnLevelLoadingStarted = new Event();
    public Event OnLevelLoaded = new Event();
    public Event OnLevelStarted = new Event();
    public Event OnLevelFinished = new Event();

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

    public void RestartLevel()
    {
        LoadLevel(0);
    }

    private void LoadLevel(int levelIndex)
    {
        if (isLevelLoading) return;

        StartCoroutine(LoadLevelCo(levelIndex));
    }

    private IEnumerator LoadLevelCo(int levelIndex)
    {
        IsLevelStarted = false;
        isLevelLoading = true;
        OnLevelLoadingStarted.Invoke();
        yield return new WaitForSeconds(1f);
        yield return SceneManager.LoadSceneAsync(0);
        yield return new WaitForSeconds(0.5f);
        currentLevelIndex = levelIndex;
        SaveLoadSystem.SetInt("LastLevelIndex", currentLevelIndex);
        OnLevelLoaded.Invoke();
        isLevelLoading = false;
    }

    public void LoadLastLevel()
    {
        LoadLevel(GetLastLevelIndex());
    }

    [Button]
    public void LoadNextLevel()
    {
        int nextLevelIndex = currentLevelIndex + 1;

        if (nextLevelIndex > GetLevelCount())
            LoadLevel(0);
        else
            LoadLevel(nextLevelIndex);
    }

    public void SpawnLevel(int levelIndex, float levelLength)
    {

    }

    public int GetLevelCount()
    {
        return levelDatabase.levels.Length - 1;
    }

    public int GetLastLevelIndex()
    {
        return SaveLoadSystem.GetInt("LastLevelIndex", 0);
    }
}