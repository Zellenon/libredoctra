using System;
using UnityEngine;

public class AbstractWave
{
    protected float _amp;
    protected float _freq;

    public AbstractWave()
    {
        _amp = 1;
        _freq = 1;
    }


    public virtual float get(float x) { return 0; }

    public void frequency(float freq)
    {
        _freq = freq;
    }
    public void amplitude(float amp)
    {
        _amp = amp;
    }

    public float convolve(AbstractWave other, float offset, float x)
    {
        return get(x) * other.get(offset - x);
    }

}
