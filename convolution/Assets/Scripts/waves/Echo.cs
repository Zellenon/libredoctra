using System;
using UnityEngine;

public class Echo : AbstractWave
{

    public override float value(float i)
    {
        if (i == 0) return 1f;
        else if (Mathf.Abs(i - _freq) < 0.01) return 0.5f;
        else return 0f;
    }
}
