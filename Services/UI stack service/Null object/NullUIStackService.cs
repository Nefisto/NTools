using Cysharp.Threading.Tasks;

namespace NTools
{
    public class NullUIStackService : IUIStackService
    {
        public IUIScreen Current => null;
        public int Count => 0;

        public UniTask<T> PushAsync<T> (T screen) where T : IUIScreen => UniTask.FromResult(screen);
        public UniTask PopAsync() => UniTask.CompletedTask;
        public UniTask PopAllAsync() => UniTask.CompletedTask;
    }
}
