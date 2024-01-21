﻿using System.Collections;

public partial class NTaskTests
{
    private int counter;
    private IEnumerator CountingRoutine()
    {
        counter = 0;
        while (true)
        {
            counter++;
            yield return null;
        }
    }
    
    private IEnumerator RoutineThatBreaks()
    {
        yield break;
    }

    private IEnumerator RoutineThatTakesOneFrame()
    {
        yield return null;
    }
}