using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepositPoint : MonoBehaviour
{
    private IDeposit _deposit;
    public IDeposit Deposit => _deposit == null ? GetComponent<IDeposit>() : _deposit;

    private void OnEnable()
    {
        Deposit.OnDepositSuccess.AddListener(RoadAnimation);
    }

    private void OnDisable()
    {
        Deposit.OnDepositSuccess.AddListener(RoadAnimation);
    }

    private void RoadAnimation()
    {
        Debug.Log("Road is opening.");
    }
}
