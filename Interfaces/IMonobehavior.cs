using UnityEngine;

public interface IMonobehavior
{
    public Transform Transform => (this as MonoBehaviour)?.transform;
}