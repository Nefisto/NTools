using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Just a sugar to avoid the need to keep adding entries for every new type of assets
/// </summary>
public interface IDatabase : IEnumerable<DataLoader<Object>>
{
    public IEnumerator LoadAssets()
    {
        foreach (var dataLoader in this)
            yield return dataLoader.LoadData();
    }

    public IEnumerator UnloadAssets()
    {
        foreach (var dataLoader in this)
            dataLoader.UnloadData();
        yield break;
    }
}