using UnityEngine.Events;

public interface IDeposit
{
    int ReqiuredDepositCount { get; }
    int CurrentDepositCount { get; }
    Event OnDepositSuccess { get; }
    void Deposit(ICollectable collectable);
}
