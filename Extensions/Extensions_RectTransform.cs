using UnityEngine;

namespace NTools
{
    public static partial class Extensions
    {
        /// <summary>
        /// This will give the size between anchors in X and Y
        /// </summary>
        public static Vector2 GetSizeBetweenAnchor (this RectTransform rectTransform)
        {
            var originalSize = rectTransform.sizeDelta;
            // Set the delta to 0, which will make the borders to touch the anchors
            rectTransform.sizeDelta = Vector2.zero;
            
            var sizeBetweenAnchors = rectTransform.rect.size;
            rectTransform.sizeDelta = originalSize;

            return new Vector2(sizeBetweenAnchors.x, sizeBetweenAnchors.y);
        }
    }
}