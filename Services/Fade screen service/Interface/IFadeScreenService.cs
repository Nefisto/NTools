using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace NTools
{
    public interface IFadeScreenService
    {
        public UniTask FadeInAsync (Settings settings = null);
        public UniTask FadeOutAsync (Settings settings = null);

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