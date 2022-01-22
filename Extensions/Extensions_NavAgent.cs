using System;
using System.Collections;
using NTools;
using UnityEngine;
using UnityEngine.AI;

public static partial class Extensions
{
    public static Task SetDestinationWithEndCallback (this NavMeshAgent nav, Vector3 targetPosition, Action OnComplete)
    {
        return new Task(Set());
        
        IEnumerator Set()
        {
            nav.SetDestination(targetPosition);
            yield return null;
            
            while (nav.remainingDistance >= .3f)
                yield return null;

            OnComplete.Invoke();
            
        }
    }

    public static Task SetPathWithEndCallback (this NavMeshAgent nav, NavMeshPath path, Action OnComplete)
    {
        return new Task(Set());
        
        IEnumerator Set()
        {
            nav.SetPath(path);
            
            while (nav.remainingDistance >= .3f)
                yield return null;
            
            OnComplete.Invoke();
        }
    }
}