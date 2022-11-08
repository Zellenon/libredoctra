using System;
using UnityEngine;
using System;

public class Dirac : AbstractWave {


    public override float get(float i) {
        if (i == 0) return 1f; else return 0f;
    }
}
