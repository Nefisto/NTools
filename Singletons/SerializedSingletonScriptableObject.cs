using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NTools.OdinExtensions
{
    public abstract class SerializedSingletonScriptableObject<T> : SerializedScriptableObject
        where T : ScriptableObject
    {
        protected static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var type = typeof(T);
                    var instances = Resources.LoadAll<T>(string.Empty);
                    _instance = instances.FirstOrDefault();
                    if (_instance == null)
                    {
                        Debug.LogErrorFormat("[ScriptableSingleton] No instance of {0} found!", type);
                    }
                    else if (instances.Count() > 1)
                    {
                        Debug.LogErrorFormat("[ScriptableSingleton] Multiple instances of {0} found!", type);
                    }
                }

                return _instance;
            }
        }
    }
}