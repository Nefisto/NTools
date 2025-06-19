using UnityEngine;

namespace NTools
{
    /// <summary>
    /// Component used to express information about some game object on inspector
    /// </summary>
    public class Information : MonoBehaviour
    {
        [Multiline(5)]
        [SerializeField]
        // HIDELABEL
        private string message;
    }
}