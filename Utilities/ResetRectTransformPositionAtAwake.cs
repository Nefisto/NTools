using Sirenix.OdinInspector;
using UnityEngine;

public class ResetRectTransformPositionAtAwake : MonoBehaviour
{
    [TitleGroup("Settings")]
    [SerializeField]
    private Vector2 positionToReset;

    private void Awake() => ((RectTransform)transform).anchoredPosition = positionToReset;
}