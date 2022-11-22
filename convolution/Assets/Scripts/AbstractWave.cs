using System;
using UnityEngine;

public class AbstractWave
{
    protected float _amp;
    protected float _freq;
    protected float _end;

    public AbstractWave()
    {
        _amp = 1;
        _freq = 1;
        _end = 1;
    }


    public float get(float x) { if (x < _end) return value(x); else return 0; }

    public virtual float value(float x) { return 0; }

    public void frequency(float freq)
    {
        _freq = freq;
    }
    public void amplitude(float amp)
    {
        _amp = amp;
    }

    public void end(float end)
    {
        _end = end;
    }

    public float convolve(AbstractWave other, float offset, float x)
    {
        return get(x) * other.get(offset - x);
    }

}
