using UnityEngine;

public class DisableAtAwake : MonoBehaviour
{
    private void Awake() => gameObject.SetActive(false);
}