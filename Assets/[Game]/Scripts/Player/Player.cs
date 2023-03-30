﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public Rigidbody Rigidbody => _rigidbody == null ? GetComponent<Rigidbody>() : _rigidbody;

    public bool IsControlable { get; private set; }

    public Event OnReachDepositArea { get; } = new Event();

    private void OnEnable()
    {
        LevelSystem.Instance.OnLevelStarted.AddListener(Initialize);
        Events.OnDepositRoadAnimationCompleted.AddListener(() => SetControl(true));
    }

    private void OnDisable()
    {
        LevelSystem.Instance.OnLevelStarted.RemoveListener(Initialize);
        Events.OnDepositRoadAnimationCompleted.RemoveListener(() => SetControl(true));
    }

    private void Initialize()
    {
        SetControl(true);
    }

    private void SetControl(bool isActive)
    {
        IsControlable = isActive;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out DepositPoint depositPoint))
        {
            SetControl(false);
            OnReachDepositArea.Invoke();
        }
    }
}
