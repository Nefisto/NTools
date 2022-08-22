namespace NTools
{
    public static partial class EventHandler
    {
        public static void RaiseEvent (string globalEventName)
        {
            if (!globalEventTable.TryGetValue(globalEventName, out var eventList))
                return;

            foreach (var invokableActionBase in eventList)
                ((InvokableAction)invokableActionBase).Invoke();
        }

        public static void RaiseEvent<T1> (string globalEventName, T1 arg1)
        {
            if (!globalEventTable.TryGetValue(globalEventName, out var eventList))
                return;

            foreach (var invokableAction in eventList)
                ((InvokableAction<T1>)invokableAction).Invoke(arg1);
        }

        public static void RaiseEvent<T1, T2> (string globalEventName, T1 arg1, T2 arg2)
        {
            if (!globalEventTable.TryGetValue(globalEventName, out var eventList))
                return;

            foreach (var invokableActionBase in eventList)
                ((InvokableAction<T1, T2>)invokableActionBase).Invoke(arg1, arg2);
        }

        public static void RaiseEvent (object targetObject, string eventName)
        {
            if (!eventTable.TryGetValue(targetObject, out var eventsToActions))
                return;

            if (!eventsToActions.TryGetValue(eventName, out var eventList))
                return;

            foreach (var invokableAction in eventList)
                ((InvokableAction)invokableAction).Invoke();
        }

        public static void RaiseEvent<T1> (object targetObject, string eventName, T1 arg1)
        {
            if (!eventTable.TryGetValue(targetObject, out var eventsToActions))
                return;

            if (!eventsToActions.TryGetValue(eventName, out var eventList))
                return;

            foreach (var invokableAction in eventList)
                ((InvokableAction<T1>)invokableAction).Invoke(arg1);
        }

        public static void RaiseEvent<T1, T2> (object targetObject, string eventName, T1 arg1, T2 arg2)
        {
            if (!eventTable.TryGetValue(targetObject, out var eventsToActions))
                return;

            if (!eventsToActions.TryGetValue(eventName, out var eventList))
                return;

            foreach (var invokableAction in eventList)
                ((InvokableAction<T1, T2>)invokableAction).Invoke(arg1, arg2);
        }
    }
}