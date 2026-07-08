using UnityEngine;

namespace NTools
{
    public interface IMonobehavior
    {
        public Transform Transform => (this as MonoBehaviour)?.transform;
    }
}
