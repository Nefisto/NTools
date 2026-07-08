public interface ITickService
{
    public void Register (ITickable tickable);
    public void Unregister (ITickable tickable);
    public void Tick();
}