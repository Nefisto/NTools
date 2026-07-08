using Cysharp.Threading.Tasks;

namespace NTools
{
    public interface IUIScreen : IMonobehavior
    {
        UniTask OpenAsync();
        UniTask CloseAsync();
    }
}
