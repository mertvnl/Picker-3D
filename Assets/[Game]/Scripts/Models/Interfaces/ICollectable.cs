public interface ICollectable
{
    void Collect(ICollector collector);
    void UnCollect(ICollector collector);
    void Release();
    void Dispose();
}
