using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class NTaskSample : MonoBehaviour
{
    public static event Action<int> OnUpdatedLayerA;
    public static event Action<int> OnUpdatedLayerB;
    public static event Action<int> OnUpdatedLayerC;

    private NTask counterTask;
    
    public void Start()
    {
        counterTask ??= new NTask(LayerARoutine(), false);
    }

    public void StartTask() => counterTask.Start(); 
    
    public void Pause() => counterTask.Pause();

    public void Unpause() => counterTask.Unpause();

    public void MoveOneStep() => counterTask.MoveNext();

    private static IEnumerator LayerARoutine()
    {
        var number = 0;
        while (true)
        {
            OnUpdatedLayerA?.Invoke(number);
            yield return new WaitForSeconds(.5f);
            yield return LayerBRoutine();

            number++;
        }
    }

    private static IEnumerator LayerBRoutine()
    {
        var number = 0;
        while (number < 4)
        {
            OnUpdatedLayerB?.Invoke(number);
            yield return new WaitForSeconds(.5f);
            yield return LayerCRoutine();
            
            number++;
        }
    }

    private static IEnumerator LayerCRoutine()
    {
        var number = 0;
        while (number < 6)
        {
            OnUpdatedLayerC?.Invoke(number);
            yield return new WaitForSeconds(.5f);

            number++;
        }
    }
}