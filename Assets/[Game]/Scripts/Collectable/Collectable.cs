using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour, ICollectable
{
    private Rigidbody _rigidbody;
    public Rigidbody Rigidbody => _rigidbody == null ? GetComponent<Rigidbody>() : _rigidbody;

    private const float RELEASE_FORCE = 5000f;

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
        Debug.Log($"Disposed {gameObject.name}");
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDeposit deposit))
            deposit.Deposit(this);
    }
}
