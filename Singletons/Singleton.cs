using UnityEngine;

namespace NTools
{
    public abstract class Singleton<T> where T : class, new()
    {
        protected static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                _instance = new T();
                if (_instance == null)
                {
                    Debug.LogErrorFormat("No instance of {0} has been created for some reason", typeof(T));
                }

                return _instance;
            }
        }
    }
}