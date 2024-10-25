using System;
using System.Collections;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ScreenFading
{
    [TitleGroup("References")]
    [SerializeField]
    private Image image;

    [TitleGroup("Debug")]
    [ReadOnly]
    [ShowInInspector]
    private bool isFaded;

    public IEnumerator FadeOut (Settings settings = null)
    {
        if (isFaded)
            yield break;

        yield return Fade(settings ?? new Settings(), 1f, 0f);
        image.raycastTarget = false;
        isFaded = true;
    }

    public IEnumerator FadeIn (Settings settings = null)
    {
        if (!isFaded)
            yield break;

        image.raycastTarget = true;
        yield return Fade(settings ?? new Settings(), 0f, 1f);
        isFaded = false;
    }

    private IEnumerator Fade (Settings settings, float from, float to)
    {
        image.color = settings.color;
        // TODO: Create SetAlpha on NTools
        image.color = image.color.SetAlpha(from);

        var timer = 0f;
        while (timer < settings.duration)
        {
            var lastFrameDuration = Time.deltaTime;

            var currentPercentage = Mathf.Clamp(timer / settings.duration, 0f, 1f);
            var alphaPercentage = from > to ? 1 - currentPercentage : currentPercentage;

            image.color = image.color.SetAlpha(alphaPercentage);

            timer += lastFrameDuration;
            yield return null;
        }

        image.color = image.color.SetAlpha(to);
    }
    
    public class Settings
    {
        public Color32 color = Color.black;

        /// <summary>
        /// In seconds
        /// </summary>
        public float duration = .5f;
    }
}