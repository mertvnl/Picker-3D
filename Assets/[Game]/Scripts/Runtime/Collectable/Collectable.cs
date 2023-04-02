using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour, ICollectable
{
    private Rigidbody _rigidbody;
    public Rigidbody Rigidbody => _rigidbody == null ? GetComponent<Rigidbody>() : _rigidbody;

    private const float RELEASE_FORCE = 2500f;
    private const float DISPOSE_DELAY = 1f;

    public void Collect(ICollector collector)
    {
        collector.AddCollectable(this);
    }

    public void UnCollect(ICollector collector)
    {
        collector.RemoveCollectable(this);
    }

    public void Release()
    {
        Rigidbody.AddForce(Vector3.forward * RELEASE_FORCE);
    }

    public void Dispose()
    {
        DOVirtual.DelayedCall(DISPOSE_DELAY, () => 
        {
            PoolingSystem.Instance.InstantiatePoolObject("CollectableDisposeFX", transform.position);
            Destroy(gameObject);
        }).SetLink(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDeposit deposit))
            deposit.Deposit(this);
    }
}
