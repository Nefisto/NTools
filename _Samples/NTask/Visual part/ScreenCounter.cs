using System;
using TMPro;
using UnityEngine;

public class ScreenCounter : MonoBehaviour
{
    public TMP_Text layerACounter;
    public TMP_Text layerBCounter;
    public TMP_Text layerCCounter;

    private void Awake()
    {
        NTaskSample.OnUpdatedLayerA += number => UpdateNumber(number, layerACounter);
        NTaskSample.OnUpdatedLayerB += number => UpdateNumber(number, layerBCounter);
        NTaskSample.OnUpdatedLayerC += number => UpdateNumber(number, layerCCounter);
    }

    private void UpdateNumber (int number, TMP_Text layer) => layer.text = $"{number}";
}