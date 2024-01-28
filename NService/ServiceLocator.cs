using UnityEngine;

public static class ServiceLocator
{
    private static MyService myService;

    public static MyService MyService
    {
        get
        {
            if (myService != null)
                return myService;

            var instance = new GameObject("[Service] MyService");
            myService = instance.AddComponent<MyService>();

            Object.DontDestroyOnLoad(instance);

            return myService;
        }
    }
}