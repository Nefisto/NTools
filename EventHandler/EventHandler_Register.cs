using System;
using System.Collections.Generic;

namespace NTools
{
    public static partial class EventHandler
    {
        public static void RegisterEvent (string globalEventName, Action action)
            => RegisterEvent(globalEventName, new InvokableAction(action));

        public static void RegisterEvent<T1> (string globalEventName, Action<T1> action)
            => RegisterEvent(globalEventName, new InvokableAction<T1>(action));

        public static void RegisterEvent<T1, T2> (string globalEventName, Action<T1, T2> action)
            => RegisterEvent(globalEventName, new InvokableAction<T1, T2>(action));

        public static void RegisterEvent (object targetObject, string eventName, Action action)
            => RegisterEvent(targetObject, eventName, new InvokableAction(action));

        public static void RegisterEvent<T1> (object targetObject, string eventName, Action<T1> action)
            => RegisterEvent(targetObject, eventName, new InvokableAction<T1>(action));

        public static void RegisterEvent<T1, T2> (object targetObject, string eventName, Action<T1, T2> action)
            => RegisterEvent(targetObject, eventName, new InvokableAction<T1, T2>(action));

        private static void RegisterEvent (string globalEventName, InvokableActionBase action)
        {
            if (globalEventTable.TryGetValue(globalEventName, out var actionList))
                actionList.Add(action);
            else
                globalEventTable.Add(globalEventName, new List<InvokableActionBase>
                {
                    action
                });
        }

        private static void RegisterEvent (object targetObject, string eventName, InvokableActionBase action)
        {
            if (!eventTable.TryGetValue(targetObject, out var eventToAction))
            {
                eventToAction = new Dictionary<string, List<InvokableActionBase>>();
                eventTable.Add(targetObject, eventToAction);
            }

            if (eventToAction.TryGetValue(eventName, out var actionList))
                actionList.Add(action);
            else
                eventToAction.Add(eventName, new List<InvokableActionBase>
                {
                    action
                });
        }
    }
}