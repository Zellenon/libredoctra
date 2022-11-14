using System;
using UnityEngine;
using System;

public class Sine : AbstractWave {

    public override float get(float i) {
        return Mathf.Sin(i * (_isReversed ? -1 : 1) * _freq) * _amp;
    }
}