using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DepositManager : Singleton<DepositManager>
{
    private List<IDeposit> _deposits = new List<IDeposit>();

    private void OnEnable()
    {
        LevelSystem.Instance.OnLevelLoadingStarted.AddListener(ResetDeposits);
    }

    private void OnDisable()
    {
        LevelSystem.Instance.OnLevelLoadingStarted.RemoveListener(ResetDeposits);
    }

    public void AddDeposit(IDeposit deposit)
    {
        if (_deposits.Contains(deposit)) 
            return;

        _deposits.Add(deposit);
    }

    public void RemoveDeposit(IDeposit deposit)
    {
        if (!_deposits.Contains(deposit))
            return;

        _deposits.Remove(deposit);
        CheckDepositStatus();
    }

    private void CheckDepositStatus()
    {
        if (_deposits.Count != 1)
            return;

        LevelSystem.Instance.SpawnNextLevel();
    }

    private void ResetDeposits()
    {
        _deposits.Clear();
    }
}
