using System;
using TMPro;
using UnityEngine;

public class ScreenCounter : MonoBehaviour
{
    public TMP_Text counter;

    private void Awake()
    {
        NTaskSample.OnUpdatedPhrase += UpdateScreen;
    }

    private void UpdateScreen (string phrase)
    {
        counter.text = phrase;
    }
}