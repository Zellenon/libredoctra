using System;
using UnityEngine;
using System;

public abstract class AbstractWave : MonoBehaviour
{
    protected float _amp;
    protected float _freq;
    protected bool _isReversed;

    // public AbstractWave(float amp, float freq,
    // bool isReversed)
    // {
    //     _amp = amp;
    //     _freq = freq;
    //     _isReversed = isReversed;
    // }

    public abstract float get(int i);

    public void isReversed(bool isReversed)
    {
        _isReversed = isReversed;
    }

    public void frequency(float freq)
    {
        _freq = freq;
    }
    public void amplitute(float amp)
    {
        _amp = amp;
    }

}
