// using System;
using System.Collections;
using System.Collections.Generic;
using Complex = System.Numerics.Complex;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using MathNet.Numerics.IntegralTransforms;

public class Convolution : MonoBehaviour
{


    [SerializeField] private Material defaultLineMaterial;
    [SerializeField] private Material lineMaterial;

    [SerializeField] private TextMeshPro OriginLabel;
    [SerializeField] private PlotObj topPlot, topRightPlot, bottomPlot;
    // [SerializeField] private EquationText eqnText;
    // [SerializeField] private Slider _magSlider, _freqSlider, _phaseSlider;


    // xtlp - x-top-left-plot; ect for x,y,width,height
    // top-right-plot; ect
    // bottom-plot
    private float xtlp, ytlp, wtlp, htlp,
    xtrp, ytrp, wtrp, htrp,
    xbp, ybp, wbp, hbp;

    public float _funct1mag, _funct1freq,
    _funct2mag, _funct2freq;

    private float _width, _height, _xscale, _yscale;


    private int Nfft, Nfreq;
    private float _Fs, _tmax;
    public float _mag, _freq, _phase;


    private Vector2[] _func1pts, _funct2pts, _resultpts;

    private LineRenderer _func1, _funt2, _result;



    void Awake()
    {

        _height = Camera.main.orthographicSize;
        _width = _height * Camera.main.aspect;

        _height = _height - 0.5f;
        _width = _width - 0.5f;
        _xscale = 1.1f / (2 * _width);
        _yscale = 0.2f;



        Debug.LogFormat("_width: {0}, _height: {1}", _width, _height);

        // we will ue the top left quadrant of the screen
        float xymargin = 0.5f;


        xtlp = 0;
        ytlp = (1f / 3f) * _height - 0.3f;
        // w2 = _width - xymargin; h2 = _height * 2f / 3f - xymargin;

        xtrp = 0;
        ytrp = 2f / 3f * _height - 0.3f;
        // w1 = _width - xymargin; h1 = _height / 3f - xymargin;

        Nfft = 1024;
        _tmax = 5f;
        _Fs = Nfft / (2 * _tmax); // sampling rate
        Nfreq = 50;    // how many harmonics to display

        _mag = 1.5f; _freq = 1.1f; _phase = 0;



        topPlot.CreateGrid(-_width / 2, _height / 2, (_width / 2) - 0.2f, _height / 2, Color.grey, defaultLineMaterial);

        topRightPlot.CreateGrid(_width / 2, _height / 2, (_width / 2) - 0.2f, _height / 2, Color.grey, defaultLineMaterial);
        //OriginLabel.gameObject.SetActive(false);


    }


    // Start is called before the first frame update
    void Start()
    {
        _xscale = 1.1f / (2 * _width);
        _yscale = 0.2f;

        GameObject lineContainer = new GameObject("Custom Function");
        lineContainer.transform.SetParent(transform, false);

        Color plotColor = Color.red;
        _func1 = lineContainer.AddComponent<LineRenderer>();
        _func1.material = lineMaterial;
        _func1.useWorldSpace = true;
        _func1.startWidth = 0.2f;
        _func1.endWidth = 0.2f;
        _func1.startColor = plotColor;
        _func1.endColor = plotColor;
        _func1.positionCount = 5;  // need at least 2
        for (int i = 0; i < 5; i++)
        {
            _func1.SetPosition(i, ToScreenCoords(new Vector2(0.2f * i, 1f * i)));
        }

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 5f; i++)
        {
            _func1.SetPosition(i, ToScreenCoords(new Vector2(i * 0.1f, Random.Range(0, 10) / 5f - 1)));
        }

        //topPlot.Update();

    }

    Vector2 ToScreenCoords(Vector2 funccoords)
    {
        return (new Vector3(-_width + funccoords.x / _xscale, funccoords.y / _yscale));
    }

}
