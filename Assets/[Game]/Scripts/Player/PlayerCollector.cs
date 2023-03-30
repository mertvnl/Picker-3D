using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour, ICollector
{
    private Player _player;
    public Player Player => _player == null ? GetComponent<Player>() : _player;

    public List<ICollectable> Collectables { get; private set; } = new List<ICollectable>();

    private void OnEnable()
    {
        Player.OnReachDepositArea.AddListener(ReleaseCollectables);
    }

    private void OnDisable()
    {
        Player.OnReachDepositArea.RemoveListener(ReleaseCollectables);
    }

    public void AddCollectable(ICollectable collectable)
    {
        if (Collectables.Contains(collectable)) 
            return;

        Collectables.Add(collectable);
    }

    public void RemoveCollectable(ICollectable collectable)
    {
        if (!Collectables.Contains(collectable))
            return;

        Collectables.Remove(collectable);
    }

    private void ReleaseCollectables()
    {
        if (Collectables.Count == 0) 
            return;

        Collectables.ForEach(c => c.Release());
        Collectables.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ICollectable collectable))
        {
            collectable.Collect(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ICollectable collectable))
        {
            collectable.UnCollect(this);
        }
    }
}
