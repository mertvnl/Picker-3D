public interface ICollectable : IComponent
{
    void Collect(ICollector collector);
    void UnCollect(ICollector collector);
    void Release();
    void Dispose();
}
