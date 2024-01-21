using System;
using System.Collections;
using UnityEngine;

public class NTaskSample : MonoBehaviour
{
    public static event Action<string> OnUpdatedPhrase;

    private NTask counterTask;
    
    public void Start()
    {
        counterTask ??= new NTask(MyCounterRoutine());
    }

    public void Pause() => counterTask.Pause();

    public void Unpause() => counterTask.Unpause();

    public void MoveOneStep() => counterTask.MoveNext();

    private IEnumerator MyCounterRoutine()
    {
        var number = 0;
        while (true)
        {
            var phrase = $"{number}";
            OnUpdatedPhrase?.Invoke(phrase);
            yield return new WaitForSeconds(.5f);

            number++;
        }
    }
}