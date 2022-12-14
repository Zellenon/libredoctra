using System;
using System.Collections;
using System.Collections.Generic;
using Complex = System.Numerics.Complex;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MathNet.Numerics.IntegralTransforms;

public class GameFreq : MonoBehaviour
{
    [SerializeField] private Material defaultLineMaterial;
    [SerializeField] private TextMeshPro OriginLabel;
    [SerializeField] private PlotObj  bottomPlot;
    //[SerializeField] private EquationText eqnText;
    //[SerializeField] private Slider _magSlider, _freqSlider, _phaseSlider;
    private float x01, y01, w1, h1, x02, y02, w2, h2;

    private int Nfft, Nfreq;
    private float _Fs, _tmax;
    public float _mag, _freq, _phase;

    private Vector2[] _funcpts, _fftpts;

    private LineRenderer _funclr, _fftlr;


    void Awake()
    {
        float ymax = 4f; //Camera.main.orthographicSize;
        float xmax = 3f; //ymax * Screen.width / Screen.height;
        Debug.LogFormat("xmax: {0}, ymax: {1}", xmax, ymax);

        // we will ue the top left quadrant of the screen
        float xymargin = 0.5f;


        x02 = -5; 
        y02 = 0.001f / 4f * ymax - 3f;
        w2 = xmax - xymargin; h2 = ymax * 2f / 3f - xymargin;

        Nfft = 1024;
        _tmax = 5f;
        _Fs = Nfft/(2 * _tmax); // sampling rate
        Nfreq = 50;    // how many harmonics to display

        _mag = 1.5f; _freq = 1.1f; _phase = 0;

       // topPlot.CreateGrids(x01, y01, w1, h1, 1f, 1f, 1.1f * (_tmax / w1), 1.5f * (_mag / h1), Color.yellow, defaultLineMaterial, OriginLabel, true, true);
        bottomPlot.CreateGrids(x02, y02, 4f, 4f, 2f, 2f, 3f, 3f, Color.yellow, defaultLineMaterial, OriginLabel, false, false);
        OriginLabel.gameObject.SetActive(true);

        //CreateFuncPlot();
        CreateFFTPlot();
        //UpdateFFTPoints();

    }



    // Start is called before the first frame update
    /*
    void Start()
    {
        _magSlider.onValueChanged.AddListener(UpdateSineParams);
        _freqSlider.onValueChanged.AddListener(UpdateSineParams);
        _phaseSlider.onValueChanged.AddListener(UpdateSineParams);
        UpdateSineParams();
    }
    */


    // Update is called once per frame
    void Update()
    {        
    }


    private void CreateFFTPlot()
    {
        Complex[] samples = new Complex[Nfft];
        for (int k=0; k<Nfft; k++)
            samples[k] = _funcpts[k].y;

        // inplace bluestein FFT with default options
        Fourier.Forward(samples, FourierOptions.AsymmetricScaling);

        _fftpts = new Vector2[2 * Nfreq + 1];

        _fftpts[Nfreq].x = 0;   // index for DC
        float maxyval = (float) samples[0].Magnitude;
        _fftpts[Nfreq].y = maxyval;   // DC value

        for (int k=0; k<Nfreq; k++)
        {
            _fftpts[k].x = (k - Nfreq) / (2 * _tmax);
            float mag = (float) samples[Nfft + k - Nfreq].Magnitude;
            _fftpts[k].y = mag;
            if (mag > maxyval)
                maxyval = mag;

            _fftpts[Nfreq + 1 + k].x = (k + 1) / (2 * _tmax);
            mag = (float) samples[k + 1].Magnitude;
            _fftpts[Nfreq + 1 + k].y = mag;
            if (mag > maxyval)
                maxyval = mag;
        }

        bottomPlot.SetScaleXAxis(Nfreq/(2 * _tmax * w2), true);
       // bottomPlot.SetScaleYAxis(maxyval/h2, false);


        GameObject lrContainer = new GameObject("FFT Plot");
        lrContainer.transform.SetParent(transform, false);
        _fftlr = lrContainer.AddComponent<LineRenderer>();
        _fftlr.positionCount = 2 * Nfreq + 1;

        _fftlr.material = defaultLineMaterial;
        _fftlr.useWorldSpace = true;
        _fftlr.startWidth = 0.05f;
        _fftlr.endWidth = 0.05f;
        _fftlr.startColor = Color.green;
        _fftlr.endColor = Color.green;

        for (int k=0; k<=1000; k++)
            _fftlr.SetPosition(k, bottomPlot.ToScreenCoords(_fftpts[k]));

    }
/*
    public void UpdateFFTPoints()
    {
        Complex[] samples = new Complex[Nfft];
        for (int k=0; k<Nfft; k++)
            samples[k] = _funcpts[k].y;

        // inplace bluestein FFT with default options
        Fourier.Forward(samples, FourierOptions.AsymmetricScaling);

        _fftpts = new Vector2[2 * Nfreq + 1];

        _fftpts[Nfreq].x = 0;   // index for DC
        float maxyval = (float) samples[0].Magnitude;
        _fftpts[Nfreq].y = maxyval;   // DC value
        
        for (int k=0; k<Nfreq; k++)
        {
            _fftpts[k].x = (k - Nfreq) / (2 * _tmax);
             float mag = (float) samples[Nfft + k - Nfreq].Magnitude;
            _fftpts[k].y = mag;
            if (mag > maxyval)
                maxyval = mag;

            _fftpts[Nfreq + 1 + k].x = (k + 1) / (2 * _tmax);
            mag = (float) samples[k + 1].Magnitude;
            _fftpts[Nfreq + 1 + k].y = mag;
            if (mag > maxyval)
                maxyval = mag;
                
        }

        bottomPlot.SetScaleXAxis(Nfreq/(2 * _tmax * w2), true);
        bottomPlot.SetScaleYAxis(12, false);

        for (int k=0; k<=Nfreq; k++)
            _fftlr.SetPosition(k, bottomPlot.ToScreenCoords(_fftpts[k]));

          //Debug.Debug.Log(mag);  
    }


*/

}
