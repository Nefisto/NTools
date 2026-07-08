using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace NTools
{
    /// <summary>
    /// A screen built at runtime by <see cref="UIStackSample"/>. It fades in/out on open/close and shows
    /// Push / Pop / "Click me" buttons on a per-depth row, so the screens underneath stay visible and you
    /// can try to click them: while covered, the stack blocks their input and nothing happens.
    /// </summary>
    public class SampleScreen : MonoBehaviour, IUIScreen
    {
        private CanvasGroup canvasGroup;
        private Text title;
        private int index;
        private int clicks;

        public void Init (int index, int depth, Color color, Action onPush, Action onPop)
        {
            this.index = index;

            var rect = (RectTransform)transform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            // Full-screen tint, but it must NOT eat raycasts: clicks fall through to whatever is below, and
            // the stack (via CanvasGroup.blocksRaycasts) is what decides which screen is actually interactive.
            var background = gameObject.AddComponent<Image>();
            background.color = color;
            background.raycastTarget = false;

            // This is the CanvasGroup the stack toggles when the screen gets covered / uncovered.
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
            canvasGroup.alpha = 0f;

            // Each screen gets its own row so the ones below stay visible and clickable.
            var row = 0.16f + depth % 5 * 0.15f;

            title = UIStackSample.CreateText(transform, Label(), 40, new Vector2(0.5f, row + 0.09f), Color.white);
            UIStackSample.CreateButton(transform, "Push", new Vector2(0.34f, row), onPush);
            UIStackSample.CreateButton(transform, "Pop", new Vector2(0.5f, row), onPop);
            UIStackSample.CreateButton(transform, "Click me", new Vector2(0.66f, row), OnClicked);
        }

        private string Label() => $"Screen {index}   (clicks: {clicks})";

        private void OnClicked()
        {
            clicks++;
            title.text = Label();
            Debug.Log($"[UIStackSample] Screen {index} received a click ({clicks}). " +
                      "A covered screen should NOT be able to log this.");
        }

        public UniTask OpenAsync() => Fade(1f, 0.25f);

        public async UniTask CloseAsync()
        {
            await Fade(0f, 0.2f);
            Destroy(gameObject);
        }

        private async UniTask Fade (float targetAlpha, float duration)
        {
            var startAlpha = canvasGroup.alpha;
            var elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
                await UniTask.Yield();
            }

            canvasGroup.alpha = targetAlpha;
        }
    }
}
