using Cysharp.Threading.Tasks;

public class NullFadeScreenService : IFadeScreenService
{
    public UniTask FadeInAsync (IFadeScreenService.Settings settings = null) => UniTask.CompletedTask;

    public UniTask FadeOutAsync (IFadeScreenService.Settings settings = null) => UniTask.CompletedTask;
}