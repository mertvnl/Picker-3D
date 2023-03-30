using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public Rigidbody Rigidbody => _rigidbody == null ? GetComponent<Rigidbody>() : _rigidbody;

    public bool IsControlable { get; private set; }
    public Event OnReachDepositArea { get; } = new Event();
    public Event OnReachFinish { get; } = new Event();

    private DepositPoint _lastDepositPoint;

    private void OnEnable()
    {
        LevelSystem.Instance.OnLevelStarted.AddListener(Initialize);
        Events.OnDepositRoadAnimationCompleted.AddListener(Initialize);
    }

    private void OnDisable()
    {
        LevelSystem.Instance.OnLevelStarted.RemoveListener(Initialize);
        Events.OnDepositRoadAnimationCompleted.RemoveListener(Initialize);
    }

    private void Initialize()
    {
        SetControl(true);
    }

    private void SetControl(bool isActive)
    {
        IsControlable = isActive;
        Rigidbody.isKinematic = !isActive;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out DepositPoint depositPoint))
        {
            if (ReferenceEquals(depositPoint, _lastDepositPoint))
                return;

            _lastDepositPoint = depositPoint;
            _lastDepositPoint.Deposit.CheckDepositCount();
            SetControl(false);
            OnReachDepositArea.Invoke();
        }

        if (other.TryGetComponent(out FinishRoad finishRoad))
        {
            SetControl(false);
            OnReachFinish.Invoke();
        }
    }
}
