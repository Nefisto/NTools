using UnityEngine;

public class DoNotDestroyOnLoad : MonoBehaviour
{
    private void Awake()
    {
        transform.parent = null;
        DontDestroyOnLoad(gameObject);
    }
}