using System;
using UnityEngine;

/// A wave that creates a Boxcar Signal
public class Boxcar : AbstractWave
{

    public override float value(float x)
    {

        return (x >= 0 && x < 1) ? 1.0f : 0.0f;
    }
}
