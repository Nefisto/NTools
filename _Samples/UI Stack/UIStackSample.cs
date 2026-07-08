using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NTools
{
    /// <summary>
    /// Drop this on a single GameObject in an (otherwise empty) scene and press Play to validate
    /// <see cref="UIStackService"/>. It builds the whole UI at runtime (event system, canvas, screens,
    /// buttons) so the scene itself stays trivial.
    ///
    /// "Push" stacks a new screen on top. Each screen puts its controls on a different row, so the ones
    /// underneath stay visible; the stack blocks their input, so their buttons stop responding until they
    /// become the top again. "Pop" closes the top screen and re-enables the one revealed beneath it.
    /// </summary>
    public class UIStackSample : MonoBehaviour
    {
        private static readonly Color[] Palette =
        {
            new(0.20f, 0.28f, 0.45f, 0.55f),
            new(0.45f, 0.24f, 0.30f, 0.55f),
            new(0.23f, 0.42f, 0.32f, 0.55f),
            new(0.42f, 0.38f, 0.20f, 0.55f),
        };

        private readonly UIStackService stack = new();
        private Transform canvasRoot;
        private int screensCreated;

        private void Start()
        {
            EnsureEventSystem();
            canvasRoot = CreateCanvas();

            var header = CreateText(canvasRoot,
                "UI Stack sample — 'Push' opens a screen on top; the ones below stay visible but the stack " +
                "blocks their input. Try clicking a covered screen's 'Click me' (nothing happens), then 'Pop'.",
                26, new Vector2(0.5f, 0.95f), Color.white);
            ((RectTransform)header.transform).sizeDelta = new Vector2(1600, 120);

            Push();
        }

        private void Push() => PushAsync().Forget();
        private void Pop() => PopAsync().Forget();

        private async UniTaskVoid PushAsync()
        {
            var depth = stack.Count;
            var index = ++screensCreated;

            var go = new GameObject($"Screen {index}", typeof(RectTransform));
            go.transform.SetParent(canvasRoot, false);

            var screen = go.AddComponent<SampleScreen>();
            screen.Init(index, depth, Palette[depth % Palette.Length], Push, Pop);

            await stack.PushAsync(screen);
        }

        private async UniTaskVoid PopAsync() => await stack.PopAsync();

        private static void EnsureEventSystem()
        {
            if (EventSystem.current != null)
                return;

            var go = new GameObject("EventSystem", typeof(EventSystem));

            // This project uses the Input System package, so add its UI module. Fall back to the legacy
            // module if the package isn't present. Reflection keeps the sample free of an asmdef dependency.
            var moduleType =
                Type.GetType("UnityEngine.InputSystem.UI.InputSystemUIInputModule, Unity.InputSystem")
                ?? Type.GetType("UnityEngine.EventSystems.StandaloneInputModule, UnityEngine.UI");

            if (moduleType != null)
                go.AddComponent(moduleType);
            else
                Debug.LogWarning("[UIStackSample] No UI input module found; button clicks won't register.");
        }

        private static Transform CreateCanvas()
        {
            var go = new GameObject("UI Stack Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));

            var canvas = go.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            var scaler = go.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);

            return go.transform;
        }

        public static Text CreateText (Transform parent, string content, int fontSize, Vector2 anchor, Color color)
        {
            var go = new GameObject("Text", typeof(RectTransform));
            go.transform.SetParent(parent, false);

            var rect = (RectTransform)go.transform;
            rect.anchorMin = rect.anchorMax = anchor;
            rect.sizeDelta = new Vector2(700, 120);
            rect.anchoredPosition = Vector2.zero;

            var text = go.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.text = content;
            text.fontSize = fontSize;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = color;
            return text;
        }

        public static Button CreateButton (Transform parent, string label, Vector2 anchor, Action onClick)
        {
            var go = new GameObject($"{label} Button", typeof(RectTransform), typeof(Image), typeof(Button));
            go.transform.SetParent(parent, false);

            var rect = (RectTransform)go.transform;
            rect.anchorMin = rect.anchorMax = anchor;
            rect.sizeDelta = new Vector2(320, 70);
            rect.anchoredPosition = Vector2.zero;

            go.GetComponent<Image>().color = new Color(0.95f, 0.95f, 0.95f, 0.95f);

            var button = go.GetComponent<Button>();
            button.onClick.AddListener(() => onClick?.Invoke());

            var textRect = (RectTransform)CreateText(go.transform, label, 28, new Vector2(0.5f, 0.5f), Color.black).transform;
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            return button;
        }
    }
}
