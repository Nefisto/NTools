// ! Original code from here: https://forum.unity.com/threads/a-more-flexible-coroutine-interface.94220/?_ga=2.56155371.1959154351.1600598637-1415149547.1596652085

using System.Collections;
using UnityEngine;

namespace NTools
{
    public partial class NTaskManager : MonoBehaviour
    {
        private static NTaskManager singleton;

        public static TaskState CreateTask (IEnumerator routine)
        {
            if (singleton)
                return new TaskState(routine);

            singleton = new GameObject("Task manager").AddComponent<NTaskManager>();
            return new TaskState(routine);
        }
    }
}