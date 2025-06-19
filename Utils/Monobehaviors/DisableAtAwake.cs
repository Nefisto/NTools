using UnityEngine;
using UnityEngine.UI;

namespace NTools
{
    public class DisableAtAwake : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}