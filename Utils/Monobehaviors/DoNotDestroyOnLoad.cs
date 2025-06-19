using UnityEngine;

namespace NTools
{
    public class DoNotDestroyOnLoad : MonoBehaviour
    {
        private void Awake()
        {
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
    }
}