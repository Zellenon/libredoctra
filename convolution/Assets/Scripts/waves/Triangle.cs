using System;
using UnityEngine;

public class Triangle : AbstractWave {

    public override float get(float i) {
        return 2.0f * (_freq * i - Mathf.Floor(_freq * i + 1.0f/2.0f));
    }
}
