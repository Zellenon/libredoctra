using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandSumSines : MonoBehaviour
{
    [SerializeField] private PlotObj _plotObj;
    [SerializeField] private Color _plotColor;
    private int _npoints;
    private int _nharmonics;
    private float _T0;   // time period of fundamental frequency
    private float _maxval;  // need this to determine correct scaling
    public Vector2[] _xypoints;

    public float a0;  // DC coeff
    public float[] ak, bk; // fundamentals and harmonics


    void Awake()
    {
        _T0 = 1;    // period of 1 time unit
        _npoints = 100;
        _nharmonics = 3;
        float deltax = 2f * _T0 / _npoints;
        _xypoints = new Vector2[_npoints+1];
        float xval = 0;
        for (int np = 0; np <= _npoints; np++)
        {
            _xypoints[np].x = xval;
            xval += deltax;
        }

        ak = new float[_nharmonics];
        bk = new float[_nharmonics];
        CreateRandomXY();
    }


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(String.Format("Max Y value: {0:f1}", _maxval));
        _plotObj.AddPlotLine(_xypoints, 2*_T0, _maxval, _plotColor);    // rescale, recenter and plot
    }


    // Update is called once per frame
    void Update()
    {        
    }



    public void SetPlotObj(PlotObj plotobj)
    {
        _plotObj = plotobj;
    }


    float randBelowOne(System.Random rnd)
    {
        return (0.1f * (rnd.Next(21) - 10)); // random number in range -1.0, -0.9, ... , 0.9, 1.0 
    }


    void CreateRandomXY()
    {
        System.Random rnd = new System.Random();
        a0 = randBelowOne(rnd);

        for (int k=0; k<_nharmonics; k++)
        {
            ak[k] = randBelowOne(rnd);
            bk[k] = randBelowOne(rnd);
        }
     
        const float twoPI = 2 * Mathf.PI;
        float f0 = 1/_T0;
        _maxval = 0;
        for (int np = 0; np <= _npoints; np++)
        {
            float yval = a0;
            float xval = _xypoints[np].x;

            for (int k=0; k<_nharmonics; k++)
            {
                yval += ak[k] * Mathf.Cos(twoPI * f0 * (k+1) * xval );
                yval += bk[k] * Mathf.Sin(twoPI * f0 * (k+1) * xval ); 
            }

            if (Mathf.Abs(yval) > _maxval)
                _maxval = Mathf.Abs(yval);

            _xypoints[np].y = yval;
        }

    }

}
