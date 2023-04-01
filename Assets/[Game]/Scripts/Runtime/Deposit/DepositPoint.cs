using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepositPoint : MonoBehaviour
{
    private IDeposit _deposit;
    public IDeposit Deposit => _deposit == null ? GetComponentInChildren<IDeposit>() : _deposit;

    [SerializeField] private Transform moveableGround;
    [SerializeField] private Transform barrierL, barrierR;
    [SerializeField] private GameObject block;

    private const float GROUND_TARGET_POSITION = -0.25f;
    private const float GROUND_MOVE_DURATION = 1f;

    private void OnEnable()
    {
        Deposit.OnDepositSuccess.AddListener(RoadAnimation);
    }

    private void OnDisable()
    {
        Deposit.OnDepositSuccess.RemoveListener(RoadAnimation);
    }

    private void RoadAnimation()
    {
        moveableGround.DOLocalMoveY(GROUND_TARGET_POSITION, GROUND_MOVE_DURATION).SetEase(Ease.OutBack);
        barrierL.DOLocalRotate(Vector3.forward * 90, GROUND_MOVE_DURATION);
        barrierR.DOLocalRotate(Vector3.forward * -90, GROUND_MOVE_DURATION);

        DOVirtual.DelayedCall(GROUND_MOVE_DURATION + 0.25f, OnRoadAnimationCompleted);

        void OnRoadAnimationCompleted()
        {
            block.SetActive(false);
            Events.OnDepositRoadAnimationCompleted.Invoke();
        }
    }
}
