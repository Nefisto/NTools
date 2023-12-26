using System;
using System.Collections;
using NTools;
using UnityEngine;
using UnityEngine.AI;

namespace NTools
{
    public static partial class Extensions
    {
        public static NTask SetDestinationWithEndCallback (this NavMeshAgent nav, Vector3 targetPosition, Action OnComplete)
        {
            return new NTask(Set());

            IEnumerator Set()
            {
                nav.SetDestination(targetPosition);
                yield return null;

                while (nav.remainingDistance >= .3f)
                    yield return null;

                OnComplete.Invoke();

            }
        }

        public static NTask SetPathWithEndCallback (this NavMeshAgent nav, NavMeshPath path, Action OnComplete)
        {
            return new NTask(Set());

            IEnumerator Set()
            {
                nav.SetPath(path);

                while (nav.remainingDistance >= .3f)
                    yield return null;

                OnComplete.Invoke();
            }
        }
    }
}