using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Image fadeImage;

    public static FadeScreen Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public async UniTask FadeInAsync (Settings settings = null)
    {
        settings ??= new Settings();
        fadeImage.color = new Color(settings.Color.r, settings.Color.g, settings.Color.b, fadeImage.color.a);
        fadeImage.raycastTarget = true;

        if (Mathf.Approximately(fadeImage.color.a, 1))
        {
            Debug.LogWarning("Trying to fade in while the image is already at 1 alpha, setting to 0.");
            fadeImage.color = new Color(settings.Color.r, settings.Color.g, settings.Color.b, 0f);
        }
        
        await fadeImage
            .DOFade(1f, settings.FadeDuration)
            .AsyncWaitForCompletion();
    }

    public async UniTask FadeOutAsync (Settings settings = null)
    {
        settings ??= new Settings();
        fadeImage.color = new Color(settings.Color.r, settings.Color.g, settings.Color.b, fadeImage.color.a);

        if (Mathf.Approximately(fadeImage.color.a, 0))
        {
            Debug.LogWarning("Trying to fade out while the image is already at 0 alpha, setting to 1.");
            fadeImage.color = new Color(settings.Color.r, settings.Color.g, settings.Color.b, 1f);
        }

        await fadeImage
            .DOFade(0f, settings.FadeDuration)
            .AsyncWaitForCompletion();
        fadeImage.raycastTarget = false;
    }

    public class Settings
    {
        public float FadeDuration { get; set; } = 1f;
        public Color Color { get; set; } = Color.black;
    }
}