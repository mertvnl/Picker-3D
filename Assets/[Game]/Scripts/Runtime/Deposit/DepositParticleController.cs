using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepositParticleController : MonoBehaviour
{
    private IDeposit _deposit;
    public IDeposit Deposit => _deposit == null ? GetComponentInChildren<IDeposit>() : _deposit;

    [SerializeField] private Transform particleSpawnPoint;

    private void OnEnable()
    {
        Deposit.OnDepositSuccess.AddListener(SuccessParticle);
    }

    private void OnDisable()
    {
        Deposit.OnDepositSuccess.RemoveListener(SuccessParticle);
    }

    private void SuccessParticle()
    {
        PoolingSystem.Instance.InstantiatePoolObject("DepositSuccessFX", particleSpawnPoint.position);
    }
}
