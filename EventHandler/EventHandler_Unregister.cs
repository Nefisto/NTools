using System;

namespace NTools
{
    public static partial class EventHandler
    {
        public static void UnregisterEvent (string globalEventName, Action action)
            => UnregisterEvent(globalEventName, new InvokableAction(action));

        public static void UnregisterEvent<T1> (string globalEventName, Action<T1> action)
            => UnregisterEvent(globalEventName, new InvokableAction<T1>(action));

        public static void UnregisterEvent<T1, T2> (string globalEventName, Action<T1, T2> action)
            => UnregisterEvent(globalEventName, new InvokableAction<T1, T2>(action));

        public static void UnregisterEvent (object targetObject, string eventName, Action action)
            => UnregisterEvent(targetObject, eventName, new InvokableAction(action));

        public static void UnregisterEvent<T1> (object targetObject, string eventName, Action<T1> action)
            => UnregisterEvent(targetObject, eventName, new InvokableAction<T1>(action));

        public static void UnregisterEvent<T1, T2> (object targetObject, string eventName, Action<T1, T2> action)
            => UnregisterEvent(targetObject, eventName, new InvokableAction<T1, T2>(action));

        private static void UnregisterEvent (string globalEventName, InvokableActionBase action)
        {
            if (!globalEventTable.TryGetValue(globalEventName, out var eventList))
                return;

            for (var i = 0; i < eventList.Count; i++)
            {
                if (!(eventList[i] is InvokableAction invokableAction))
                    continue;

                if (!invokableAction.IsSameAction(action))
                    continue;

                eventList.RemoveAt(i);
                break;
            }
        }

        private static void UnregisterEvent (object targetObject, string eventName, InvokableActionBase action)
        {
            if (!eventTable.TryGetValue(targetObject, out var eventNameToList))
                return;

            if (!eventNameToList.TryGetValue(eventName, out var eventList))
                return;

            for (var i = 0; i < eventList.Count; i++)
            {
                if (!(eventList[i] is InvokableAction invokableAction))
                    continue;

                if (!invokableAction.IsSameAction(action))
                    continue;

                eventList.RemoveAt(i);
                break;
            }
        }
    }
}