using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DepositManager : Singleton<DepositManager>
{
    private List<IDeposit> _deposits = new List<IDeposit>();

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
        if (_deposits.Count != 0)
            return;

        Debug.Log("All deposits completed. Spawn new level.");
    }
}
