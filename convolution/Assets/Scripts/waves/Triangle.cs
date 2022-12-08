using System;
using UnityEngine;

public class Triangle : AbstractWave
{

    public override float value(float i)
    {
        return 2.0f * Mathf.Abs(_freq * i - Mathf.Floor(_freq * i + 1.0f / 2.0f));
    }
}
