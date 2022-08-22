namespace NTools
{
    public static partial class EventHandler
    {
        public static void ClearEvents()
        {
            globalEventTable.Clear();

            foreach (var (_, dictionary) in eventTable)
                dictionary.Clear();
            eventTable.Clear();
        }
    }
}