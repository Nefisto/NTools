using System.Collections;
using NTools;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GetComponentTests
{
    [UnityTest]
    public IEnumerator GetComponentOnActiveChildren()
    {
        var p = new GameObject("Parent");
        Object.Instantiate(new GameObject(), p.transform).AddComponent<TestComponent>();

        yield return null;
        
        var foundComp = p.transform.NGetComponentInChildren<TestComponent>(Extensions.Self.Exclude);
        
        Assert.IsNotNull(foundComp);
    }
    
    [UnityTest]
    public IEnumerator GetComponentOnActiveChildrenExcludingParent()
    {
        var p = new GameObject("Parent").AddComponent<TestComponent>();
        var c = Object.Instantiate(new GameObject(), p.transform).AddComponent<TestComponent>();

        yield return null;
        
        var foundComp = p.transform.NGetComponentInChildren<TestComponent>(Extensions.Self.Exclude);
        
        Assert.AreEqual(foundComp.transform, c.transform);
    }

    
    [UnityTest]
    public IEnumerator GetComponentOnInactiveChildren()
    {
        var p = new GameObject("Parent");
        var c = Object.Instantiate(new GameObject(), p.transform).AddComponent<TestComponent>();
        c.gameObject.SetActive(false);

        yield return null;
        
        var foundComp = p.transform.NGetComponentInChildren<TestComponent>(Extensions.Self.Exclude, Extensions.Inactive.Include);
        
        Assert.IsNotNull(foundComp);
    }
}