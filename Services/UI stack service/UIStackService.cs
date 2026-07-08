using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace NTools
{
    public class UIStackService : IUIStackService
    {
        private readonly Stack<IUIScreen> stack = new();

        public IUIScreen Current => stack.Count > 0 ? stack.Peek() : null;
        public int Count => stack.Count;

        /// <summary>
        /// Opens <paramref name="screen"/> on top of the stack. The screen underneath stays open but has
        /// its input blocked (raycasts disabled) until it becomes the top again.
        /// </summary>
        public async UniTask<T> PushAsync<T> (T screen) where T : IUIScreen
        {
            if (screen is null)
            {
                Debug.LogError("[UIStackService] Trying to push a null screen.");
                return default;
            }

            SetInputBlocked(Current, true);

            stack.Push(screen);
            await screen.OpenAsync();

            return screen;
        }

        /// <summary>
        /// Closes the top screen and re-enables input on the one that becomes the new top.
        /// </summary>
        public async UniTask PopAsync()
        {
            if (stack.Count == 0)
            {
                Debug.LogWarning("[UIStackService] Trying to pop while the stack is empty.");
                return;
            }

            var top = stack.Pop();
            await top.CloseAsync();

            SetInputBlocked(Current, false);
        }

        /// <summary>
        /// Closes every screen from the top down until the stack is empty.
        /// </summary>
        public async UniTask PopAllAsync()
        {
            while (stack.Count > 0)
                await PopAsync();
        }

        private static void SetInputBlocked (IUIScreen screen, bool blocked)
        {
            if (screen?.Transform == null)
                return;

            if (!screen.Transform.TryGetComponent(out CanvasGroup canvasGroup))
                canvasGroup = screen.Transform.gameObject.AddComponent<CanvasGroup>();

            canvasGroup.blocksRaycasts = !blocked;
        }
    }
}
