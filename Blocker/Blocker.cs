using System.Collections.Generic;
using UnityEngine;

namespace NTools
{
    public class Blocker
    {
        private readonly Dictionary<object, Dictionary<string, ReasonData>> ownerToReasons = new();

        public bool IsBlocked => ownerToReasons.Count > 0;

        /// <summary>
        /// Every reason currently blocking, across all owners.
        /// </summary>
        public List<ReasonData> GetBlockers ()
        {
            var blockers = new List<ReasonData>();

            foreach (var reasons in ownerToReasons.Values)
                blockers.AddRange(reasons.Values);

            return blockers;
        }

        /// <summary>
        /// The reasons a specific owner is blocking with (empty if none).
        /// </summary>
        public List<ReasonData> GetBlockers (object owner)
            => ownerToReasons.TryGetValue(owner, out var reasons)
                ? new List<ReasonData>(reasons.Values)
                : new List<ReasonData>();

        public void AddBlocker (object owner, string reason = null) => AddBlocker(owner, new ReasonData(reason));

        public void AddBlocker (object owner, ReasonData reasonData = null)
        {
            reasonData ??= new ReasonData();

            if (!ownerToReasons.TryGetValue(owner, out var reasons))
                ownerToReasons[owner] = reasons = new Dictionary<string, ReasonData>();

            if (!reasons.TryAdd(reasonData.Reason, reasonData))
                Debug.LogError($"[Blocker] Owner '{owner}' already has a block with reason '{reasonData.Reason}'. " +
                               "This should never happen and likely indicates a logic error (same block added twice).");
        }

        /// <summary>
        /// Removes a single reason from an owner. If it was the owner's last reason, the owner is dropped.
        /// </summary>
        public void RemoveBlocker (object owner, string reason)
        {
            if (!ownerToReasons.TryGetValue(owner, out var reasons))
                return;

            reasons.Remove(reason);

            if (reasons.Count == 0)
                ownerToReasons.Remove(owner);
        }

        /// <summary>
        /// Removes every reason tied to an owner at once
        /// </summary>
        public void RemoveBlocker (object owner) => ownerToReasons.Remove(owner);

        public class ReasonData
        {
            public ReasonData() { }
            public ReasonData (string reason) => Reason = reason;
            public string Reason { get; set; } = "Not informed";

            public static implicit operator string (ReasonData reasonData) => reasonData?.Reason;
        }
    }
}
