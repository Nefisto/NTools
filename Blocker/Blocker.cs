using System.Collections.Generic;

public class Blocker
{
    private readonly Dictionary<object, ReasonData> blockerToReason = new();
    public bool IsBlocked => blockerToReason.Count > 0;

    public void AddBlocker (object blocker, string reason = null) => AddBlocker(blocker, new ReasonData(reason));

    public void AddBlocker (object blocker, ReasonData reasonData = null)
    {
        reasonData ??= new ReasonData();

        blockerToReason.TryAdd(blocker, reasonData);
    }

    public void RemoveBlocker (object blocker)
    {
        blockerToReason.Remove(blocker);
    }

    public class ReasonData
    {
        public ReasonData() { }
        public ReasonData (string reason) => Reason = reason;
        public string Reason { get; set; } = "Not informed";
    }
}