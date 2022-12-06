using System;
using UnityEngine;

// A modification of the Dirac Delta with a second impulse that has half the height at x=1
public class Echo : AbstractWave
{

    public override float value(float i)
    {
        if (i == 0) return 1f;
        else if (Mathf.Abs(i - _freq) < 0.01) return 0.5f;
        else return 0f;
    }
}
