using System;
using UnityEngine;
using System;

public class Echo : AbstractWave {

    public override float get(float i) {
        if (i == 0) return 1f; 
        else if (i == _freq * (_isReversed ? -1 : 1)) return 0.5f;
        else return 0f;
    }
}
