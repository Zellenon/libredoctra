using System;
using UnityEngine;

public class Sine : AbstractWave
{

    public override float value(float i)
    {
        return Mathf.Sin(i * _freq) * _amp;
    }
}
