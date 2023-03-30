using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Depositable : MonoBehaviour, IDeposit
{
    [field: SerializeField] public int ReqiuredDepositCount { get; private set; }
    [field: SerializeField] public int CurrentDepositCount { get; private set; }
    public Event OnDepositSuccess { get; } = new Event();

    private Coroutine _checkDepositCountCoroutine;
    private WaitForSeconds _checkDuration = new WaitForSeconds(FAIL_CHECK_DELAY);
    private bool _isCompleted;

    private const float FAIL_CHECK_DELAY = 2f;

    private void OnEnable()
    {
        DepositManager.Instance.AddDeposit(this);
    }

    private void OnDisable()
    {
        DepositManager.Instance.RemoveDeposit(this);
    }

    public void Deposit(ICollectable collectable)
    {
        CurrentDepositCount++;
        CheckDepositCount();
    }

    private void CheckDepositCount()
    {
        if (_checkDepositCountCoroutine != null)
            StopCoroutine(_checkDepositCountCoroutine);

        _checkDepositCountCoroutine = StartCoroutine(CheckDepositCountCo());
    }

    private IEnumerator CheckDepositCountCo()
    {
        yield return _checkDuration;

        if (_isCompleted)
            yield break;

        if (CurrentDepositCount >= ReqiuredDepositCount)
        {
            _isCompleted = true;
            Debug.Log("Deposit completed successfuly.");
            DepositManager.Instance.RemoveDeposit(this);
            OnDepositSuccess.Invoke();
        }
        else
        {
            GameManager.Instance.CompleteLevel(false);
        }
    }
}
