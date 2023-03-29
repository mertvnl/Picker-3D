using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public Rigidbody Rigidbody => _rigidbody == null ? GetComponent<Rigidbody>() : _rigidbody;

    public bool IsControlable {  get; private set; }

    private void OnEnable()
    {
        LevelSystem.Instance.OnLevelStarted.AddListener(Initialize);
    }

    private void OnDisable()
    {
        LevelSystem.Instance.OnLevelStarted.RemoveListener(Initialize);
    }

    private void Initialize()
    {
        IsControlable = true;
    }
}
