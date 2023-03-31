using UnityEngine.Events;

public interface IDeposit
{
    int ReqiuredDepositCount { get; }
    int CurrentDepositCount { get; }
    Event OnDepositSuccess { get; }
    Event OnDeposited { get; }
    void Deposit(ICollectable collectable);
    void CheckDepositCount();
}
