using System;
using UnityEngine;

public class Boxcar : AbstractWave
{

    public override float value(float
   x) {
        return (x >= 0 && x < 1) ? 1.0f : 0.0f;
    }
}
