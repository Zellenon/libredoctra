using System;
using UnityEngine;
using System;

public class Boxcar : AbstractWave {

    public override float get(int x) {
        return (x >= 0 && x <= 1) ? 1.0f : 0.0f;
    }
}
