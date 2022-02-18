using UnityEngine;

public static partial class Extensions
{
    /// <summary>
    /// This will give the size between anchors in X and Y
    /// </summary>
    // TODO: Research if unity offer some built-in method to make it
    public static Vector2 NTGetSizeBetweenAnchor (this RectTransform rectTransform)
    {
        // Cache the original size
        var originalSize = rectTransform.sizeDelta;
        
        // Set the delta to 0, which will make the borders to touch the anchors
        rectTransform.sizeDelta = Vector2.zero;
        
        // Get the size of the rect
        var sizeBetweenAnchors = rectTransform.rect.size;
            
        // Return it to original size
        rectTransform.sizeDelta = originalSize;

        return new Vector2(sizeBetweenAnchors.x, sizeBetweenAnchors.y);
    }
}