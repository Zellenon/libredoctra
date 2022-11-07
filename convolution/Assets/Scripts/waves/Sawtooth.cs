using System;
using UnityEngine;
using System;

public class Sawtooth : AbstractWave {

    public override float get(float i) {
        return ((i/_freq) % 1) * _amp;
    }
}
