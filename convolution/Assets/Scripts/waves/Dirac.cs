using System;
using UnityEngine;

public class Dirac : AbstractWave
{

    public override float value(float
   i) {
        if (i == 0) return 1f; else return 0f;
    }
}
