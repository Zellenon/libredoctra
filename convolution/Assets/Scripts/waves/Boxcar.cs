using System;
using UnityEngine;
using System;

public class Boxcar : AbstractWave {

    public override float get(float x) {
        return (x >= 0 && x <= 1) ? 1.0f : 0.0f;
    }
}
