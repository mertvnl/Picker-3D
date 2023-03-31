using System.Collections.Generic;

public interface ICollector
{
    List<ICollectable> Collectables { get; }

    void AddCollectable(ICollectable collectable);
    void RemoveCollectable(ICollectable collectable);
}