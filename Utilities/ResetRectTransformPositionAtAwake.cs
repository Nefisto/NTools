using UnityEngine;

public class ResetRectTransformPositionAtAwake : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private Vector2 positionToReset;

    private void Awake() => ((RectTransform)transform).anchoredPosition = positionToReset;
}