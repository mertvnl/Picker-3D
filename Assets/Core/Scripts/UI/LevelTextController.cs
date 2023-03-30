using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelTextController : MonoBehaviour
{
    private TextMeshProUGUI _text;
    public TextMeshProUGUI Text { get { return _text == null ? _text = GetComponent<TextMeshProUGUI>() : _text; } }
    private void OnEnable()
    {
        LevelSystem.Instance.OnLevelLoaded.AddListener(UpdateLevelText);
        LevelSystem.Instance.OnLevelStarted.AddListener(UpdateLevelText);
    }

    private void OnDisable()
    {
        LevelSystem.Instance.OnLevelLoaded.RemoveListener(UpdateLevelText);
        LevelSystem.Instance.OnLevelStarted.RemoveListener(UpdateLevelText);
    }

    private void UpdateLevelText()
    {
        Text.SetText("Level " + SaveLoadSystem.GetInt("FakeLevel", 1));
    }
}
