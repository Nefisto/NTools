using NTools;
using UnityEngine;

/// <summary>
/// Lazy approach to create a singleton in a Dont Destroy on Load
/// </summary>
public class PersistentLazySingletonMonoBehaviour<T> : LazyBehavior
    where T : Component
{
    protected static T instance { get; set; }

    public static T Instance
    {
        get
        {
            if (instance == null)
                CreatePersistentObject();

            return instance;
        }
    }

    private static void CreatePersistentObject()
    {
        var go = new GameObject(typeof(T).ToString());
        instance = go.AddComponent<T>();
        DontDestroyOnLoad(go);
    }
}