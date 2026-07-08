using Cysharp.Threading.Tasks;

namespace NTools
{
    public interface IUIStackService
    {
        IUIScreen Current { get; }
        int Count { get; }

        UniTask<T> PushAsync<T> (T screen) where T : IUIScreen;
        UniTask PopAsync();
        UniTask PopAllAsync();
    }
}
