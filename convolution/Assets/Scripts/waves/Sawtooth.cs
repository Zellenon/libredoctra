using System;
using UnityEngine;

public class Sawtooth : AbstractWave
{

    public override float value(float
   i)
    {
        return ((i / _freq) % 1) * _amp;
    }
}
