﻿using System;
using System.Collections;
using NTools;
using UnityEngine;

public class NTaskSample : MonoBehaviour
{
    public static event Action<int> OnUpdatedLayerA;
    public static event Action<int> OnUpdatedLayerB;
    public static event Action<int> OnUpdatedLayerC;

    private NTask counterTask;
    
    public void Start()
    {
        counterTask?.Stop();
        counterTask = new NTask(LayerARoutine());
    }

    public void StartTask() => counterTask.Resume(); 
    
    public void Pause() => counterTask.Pause();

    public void Resume() => counterTask.Resume();

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