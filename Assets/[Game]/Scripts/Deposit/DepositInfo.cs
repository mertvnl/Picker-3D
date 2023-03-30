using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DepositInfo : MonoBehaviour
{
    private IDeposit _deposit;
    public IDeposit Deposit => _deposit == null ? GetComponentInChildren<IDeposit>() : _deposit;

    [SerializeField] private TextMeshPro _infoText;

    private void OnEnable()
    {
        Deposit.OnDeposited.AddListener(UpdateInfoText);
    }

    private void OnDisable()
    {
        Deposit.OnDeposited.AddListener(UpdateInfoText);
    }

    private void Start()
    {
        UpdateInfoText();
    }

    private void UpdateInfoText()
    {
        _infoText.SetText($"{Deposit.CurrentDepositCount}/{Deposit.ReqiuredDepositCount}");
    }
}
