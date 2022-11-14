using System;
using UnityEngine;

public class AbstractWave
{
    protected float _amp;
    protected float _freq;
    protected bool _isReversed;

    public AbstractWave()
    {
        _amp = 1;
        _freq = 1;
        _isReversed = false;
    }


    public virtual float get(float x) {return 0;}

    public void isReversed(bool isReversed)
    {
        _isReversed = isReversed;
    }

    public void frequency(float freq)
    {
        _freq = freq;
    }
    public void amplitude(float amp)
    {
        _amp = amp;
    }

}
