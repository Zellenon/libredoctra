using System;
using System.Collections;
using System.Collections.Generic;
using Complex = System.Numerics.Complex;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MathNet.Numerics.IntegralTransforms;

public class SineWave2 : MonoBehaviour
{
    [SerializeField] private Material defaultLineMaterial;
    [SerializeField] private TextMeshPro OriginLabel;
    [SerializeField] private PlotObj topPlot;
    [SerializeField] private EquationText eqnText;
    [SerializeField] private Slider _magSlider, _freqSlider, _phaseSlider;
    private float x01, y01, w1, h1, x02, y02, w2, h2;

    private int Nfft, Nfreq;
    private float _Fs, _tmax;
    public float _magLO, _freqLO, _phaseLO;
    private Vector2[] _funcpts, _fftpts;

    private LineRenderer _funclr, _fftlr;


    void Awake()
    {
        float ymax = 4f;//Camera.main.orthographicSize;
        float xmax = 3f;//ymax * Screen.width / Screen.height;
        Debug.LogFormat("xmax: {0}, ymax: {1}", xmax, ymax);

        // we will ue the top left quadrant of the screen
        float xymargin = 0.5f;

        x01 = -5; 
        y01 = 0.001f / 4f * ymax - 3f;
        w1 = xmax - xymargin; 
        h1 = ymax / 3f - xymargin;

        Nfft = 1024;
        _tmax = 5f;
        _Fs = Nfft/(2 * _tmax); // sampling rate
        Nfreq = 50;    // how many harmonics to display

        _magLO = 1.5f; _freqLO = 1.1f; _phaseLO = 0;

        topPlot.CreateGrids(x01, y01, w1, h1, 1f, 1f, 1.1f * (_tmax / w1), 1.5f * (_magLO / h1), Color.grey, defaultLineMaterial, OriginLabel, true, true);
        OriginLabel.gameObject.SetActive(false);

        CreateFuncPlot();

    }



    // Start is called before the first frame update
    void Start()
    {
        _magSlider.onValueChanged.AddListener(UpdateSineParams);
        _freqSlider.onValueChanged.AddListener(UpdateSineParams);
        _phaseSlider.onValueChanged.AddListener(UpdateSineParams);
        UpdateSineParams();
    }


    // Update is called once per frame
    void Update()
    {        
    }


    public void UpdateSineParams(float val = 0)
    {
        // ignore function argument, so we can reuse same function for all sliders
        _magLO = _magSlider.value * 0.1f;
        _freqLO = _freqSlider.value * 0.1f;
        _phaseLO = _phaseSlider.value * 3f * Mathf.PI / 180f;
        UpdateFuncPoints();
        eqnText.updateEquationText(_magLO, _freqLO, _phaseLO * 180 / Mathf.PI);
    }


    private void CreateFuncPlot()
    {
        _funcpts = new Vector2[Nfft];
        const float twoPI = 2 * Mathf.PI;

        for (int k=0; k<Nfft; k++)
        {
            float tval = Mathf.Lerp(-_tmax, _tmax, k/((float) Nfft-1));
            _funcpts[k].x = tval;
            _funcpts[k].y = _magLO * Mathf.Cos( twoPI * _freqLO * tval + _phaseLO );
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


    public void UpdateFuncPoints()
    {
        const float twoPI = 2 * Mathf.PI;

        for (int k=0; k<Nfft; k++)
        {
            float tval = Mathf.Lerp(-_tmax, _tmax, k/((float) Nfft-1));
            _funcpts[k].x = tval;
            _funcpts[k].y = _magLO * Mathf.Cos( twoPI * _freqLO * tval + _phaseLO );
        }

        for (int k=0; k<Nfft; k++)
            _funclr.SetPosition(k, topPlot.ToScreenCoords(_funcpts[k]));
    }


}