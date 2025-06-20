using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace NTools
{
    public class FadeScreenService
    {
        private readonly Settings settings;

        public FadeScreenService (Settings settings) => this.settings = settings;

        public async UniTask FadeInAsync (Settings settings = null)
        {
            settings ??= new Settings();
            settings.FadeImage.color = new Color(settings.Color.r, settings.Color.g, settings.Color.b, settings.FadeImage.color.a);
            settings.FadeImage.raycastTarget = true;

            if (Mathf.Approximately(settings.FadeImage.color.a, 1))
            {
                Debug.LogWarning("Trying to fade in while the image is already at 1 alpha, setting to 0.");
                settings.FadeImage.color = new Color(settings.Color.r, settings.Color.g, settings.Color.b, 0f);
            }

            var elapsedTime = 0f;
            var startAlpha = settings.FadeImage.color.a;
            while (elapsedTime < settings.FadeDuration)
            {
                elapsedTime += Time.deltaTime;
                var newAlpha = Mathf.Lerp(startAlpha, 1f, elapsedTime / settings.FadeDuration);
                settings.FadeImage.color = new Color(settings.Color.r, settings.Color.g, settings.Color.b, newAlpha);
                await UniTask.Yield();
            }

            settings.FadeImage.color = new Color(settings.Color.r, settings.Color.g, settings.Color.b, 1f);
        }

        public async UniTask FadeOutAsync (Settings settings = null)
        {
            settings ??= new Settings();
            settings.FadeImage.color = new Color(settings.Color.r, settings.Color.g, settings.Color.b, settings.FadeImage.color.a);

            if (Mathf.Approximately(settings.FadeImage.color.a, 0))
            {
                Debug.LogWarning("Trying to fade out while the image is already at 0 alpha, setting to 1.");
                settings.FadeImage.color = new Color(settings.Color.r, settings.Color.g, settings.Color.b, 1f);
            }

            var elapsedTime = 0f;
            var startAlpha = settings.FadeImage.color.a;
            while (elapsedTime < settings.FadeDuration)
            {
                elapsedTime += Time.deltaTime;
                var newAlpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / settings.FadeDuration);
                settings.FadeImage.color = new Color(settings.Color.r, settings.Color.g, settings.Color.b, newAlpha);
                await UniTask.Yield();
            }

            settings.FadeImage.color = new Color(settings.Color.r, settings.Color.g, settings.Color.b, 0f);
            settings.FadeImage.raycastTarget = false;
        }

        [Serializable]
        public class Settings
        {
            [field: SerializeField]
            public Image FadeImage { get; set; }

            [field: SerializeField]
            public float FadeDuration { get; set; } = 1f;

            [field: SerializeField]
            public Color Color { get; set; } = Color.black;
        }
    }
}