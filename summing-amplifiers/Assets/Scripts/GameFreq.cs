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
    [SerializeField] private PlotObj topPlot, bottomPlot;
    [SerializeField] private EquationText eqnText;
    [SerializeField] private Slider _magSlider, _freqSlider, _phaseSlider;
    private float x01, y01, w1, h1, x02, y02, w2, h2;

    private int Nfft, Nfreq;
    private float _Fs, _tmax;
    public float _mag, _freq, _phase;
    private Vector2[] _funcpts, _fftpts;

    private LineRenderer _funclr, _fftlr;


    void Awake()
    {
        float ymax = Camera.main.orthographicSize;
        float xmax = ymax * Screen.width / Screen.height;
        Debug.LogFormat("xmax: {0}, ymax: {1}", xmax, ymax);

        // we will ue the top left quadrant of the screen
        float xymargin = 0.5f;

        x01 = 0; y01 = 2f / 3f * ymax - 0.3f;
        w1 = xmax - xymargin; h1 = ymax / 3f - xymargin;
        x02 = 0; y02 = -1f / 3f * ymax - 0.3f;
        w2 = xmax - xymargin; h2 = ymax * 2f / 3f - xymargin;

        Nfft = 1024;
        _tmax = 5f;
        _Fs = Nfft/(2 * _tmax); // sampling rate
        Nfreq = 50;    // how many harmonics to display

        _mag = 1.5f; _freq = 1.1f; _phase = 0;

        topPlot.CreateGrids(x01, y01, w1, h1, 1f, 1f, 1.1f * (_tmax / w1), 1.5f * (_mag / h1), Color.grey, defaultLineMaterial, OriginLabel, true, true);
        bottomPlot.CreateGrids(x02, y02, w2, h2, 1f, 1f, 1f, 1f, Color.grey, defaultLineMaterial, OriginLabel, true, false);
        OriginLabel.gameObject.SetActive(false);

        GameFunc();
        CreateFFTPlot();

    }



    // Start is called before the first frame update
    void Start()
    {
        _magSlider.onValueChanged.AddListener(UpdateSineParams);
        _freqSlider.onValueChanged.AddListener(UpdateSineParams);
        _phaseSlider.onValueChanged.AddListener(UpdateSineParams);
        UpdateSineParams();
    }


  


    public void UpdateSineParams(float val = 0)
    {
        // ignore function argument, so we can reuse same function for all sliders
        _mag = _magSlider.value * 0.1f;
        _freq = _freqSlider.value * 0.1f;
        _phase = _phaseSlider.value * 3f * Mathf.PI / 180f;
        UpdateFuncPoints();
        UpdateFFTPoints();
        eqnText.updateEquationText(_mag, _freq, _phase * 180 / Mathf.PI);
    }




    private void GameFunc()
    {
        _funcpts = new Vector2[Nfft];
        const float twoPI = 2 * Mathf.PI;

        for (int k=0; k<Nfft; k++)
        {
            float tval = Mathf.Lerp(-_tmax, _tmax, k/((float) Nfft-1));
            _funcpts[k].x = tval;
            _funcpts[k].y = _mag * Mathf.Cos( twoPI * _freq * tval + _phase );
        }


        GameObject lrContainer = new GameObject("Function Plot");
        lrContainer.transform.SetParent(transform, false);
        _funclr = lrContainer.AddComponent<LineRenderer>();
        _funclr.positionCount = Nfft;

        _funclr.material = defaultLineMaterial;
        _funclr.useWorldSpace = true;
        _funclr.startWidth = 0.05f;
        _funclr.endWidth = 0.05f;
        _funclr.startColor = Color.yellow;
        _funclr.endColor = Color.yellow;

        for (int k=0; k<Nfft; k++)
            _funclr.SetPosition(k, topPlot.ToScreenCoords(_funcpts[k]));

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
        bottomPlot.SetScaleYAxis(maxyval/h2, false);


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

        for (int k=0; k<=2*Nfreq; k++)
            _fftlr.SetPosition(k, bottomPlot.ToScreenCoords(_fftpts[k]));

    }


    public void UpdateFuncPoints()
    {
        const float twoPI = 2 * Mathf.PI;

        for (int k=0; k<Nfft; k++)
        {
            float tval = Mathf.Lerp(-_tmax, _tmax, k/((float) Nfft-1));
            _funcpts[k].x = tval;
            _funcpts[k].y = _mag * Mathf.Cos( twoPI * _freq * tval + _phase );
        }

        for (int k=0; k<Nfft; k++)
            _funclr.SetPosition(k, topPlot.ToScreenCoords(_funcpts[k]));
    }


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
        bottomPlot.SetScaleYAxis(maxyval/h2, false);

        for (int k=0; k<=2*Nfreq; k++)
            _fftlr.SetPosition(k, bottomPlot.ToScreenCoords(_fftpts[k]));
    }



    public void InterpolateAnchorPoints(Vector2[] xypoints, Vector2[] anchorpoints)
    {
        // the vector xypoints should already contain x coordinates
        // this function will fill in the corresponding y coordinates by linearly
        // interpolating the two surrounding vectors in anchorpoints

        int lindex, rindex;
        float lxvalue, rxvalue, lyvalue, ryvalue;

        rindex = 1;

        for (int k=0; k<xypoints.Length; k++)
        {
            while (anchorpoints[rindex].x < xypoints[k].x)
            {
                if (rindex == anchorpoints.Length - 1)
                    break;

                rindex++;
            }

            lindex = rindex - 1;
            lxvalue = anchorpoints[lindex].x;
            rxvalue = anchorpoints[rindex].x;
            lyvalue = anchorpoints[lindex].y;
            ryvalue = anchorpoints[rindex].y;

            float t = (xypoints[k].x - lxvalue)/(rxvalue - lxvalue);
            xypoints[k].y = Mathf.Lerp(lyvalue, ryvalue, t);
        }
    }




    void fft_test()
    {
        Complex[] samples = new Complex[16];

        // inplace bluestein FFT with default options
        Fourier.Forward(samples, FourierOptions.AsymmetricScaling);
    }

}
