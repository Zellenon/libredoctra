using System;
using UnityEngine;
using System;

public class Triangle : AbstractWave {

    public override float get(float i) {
        return 2 * (_freq * i - Mathf.Floor(_freq * i + 1.0/2.0));
    }
}
