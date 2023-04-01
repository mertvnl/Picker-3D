using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Depositable : MonoBehaviour, IDeposit
{
    [field: SerializeField] public int ReqiuredDepositCount { get; set; }
    public int CurrentDepositCount => _depositedCollectables.Count;
    public Event OnDepositSuccess { get; } = new Event();
    public Event OnDeposited { get; } = new Event();

    private Coroutine _checkDepositCountCoroutine;
    private WaitForSeconds _checkDuration = new WaitForSeconds(FAIL_CHECK_DELAY);
    private bool _isCompleted;
    private List<ICollectable> _depositedCollectables = new List<ICollectable>();

    private const float FAIL_CHECK_DELAY = 2f;

    private void OnEnable()
    {
        DepositManager.Instance.AddDeposit(this);
    }

    public void Deposit(ICollectable collectable)
    {
        if (_depositedCollectables.Contains(collectable))
            return;
        
        _depositedCollectables.Add(collectable);
        collectable.Dispose();
        CheckDepositCount();
        OnDeposited.Invoke();
    }

    public void CheckDepositCount()
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
            DepositManager.Instance.RemoveDeposit(this);
            OnDepositSuccess.Invoke();
        }
        else
        {
            GameManager.Instance.CompleteLevel(false);
        }
    }
}
