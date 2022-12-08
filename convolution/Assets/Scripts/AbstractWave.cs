using System;
using UnityEngine;

/// The AbstractWave is the parent class for all wave classes used in graphs to represent simple, mathematically-defined functions.
public class AbstractWave
{
    protected float _amp;
    protected float _freq;
    /// If get() is called for a value higher than _end, 0 is returned
    protected float _end;

    public AbstractWave()
    {
        _amp = 1;
        _freq = 1;
        _end = 1;
    }


    /// Get the value of the function at the given x position, unless x is greater than _end
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

    /// Convolve this wave with another wave
    public float convolve(AbstractWave other, float offset, float x)
    {
        return get(x) * other.get(offset - x);
    }

}
