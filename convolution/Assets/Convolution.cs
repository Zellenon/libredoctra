using System;
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



    private int Nfft, Nfreq;
    private float _Fs, _tmax;
    public float _mag, _freq, _phase;


    private Vector2[] _func1pts, _funct2pts, _resultpts;

    //private LineRenderer _func1, _funt2, _result;
    
    

    void Awake(){
        
        float ymax = Camera.main.orthographicSize;
        float xmax = ymax*Camera.main.aspect;
        
        ymax = ymax - 0.5f;
        xmax = xmax - 0.5f;


        Debug.LogFormat("xmax: {0}, ymax: {1}", xmax, ymax);

        // we will ue the top left quadrant of the screen
        float xymargin = 0.5f;


        xtlp = 0; 
        ytlp = (1f / 3f) * ymax - 0.3f;
        // w2 = xmax - xymargin; h2 = ymax * 2f / 3f - xymargin;

        xtrp = 0; 
        ytrp = 2f / 3f * ymax - 0.3f;
        // w1 = xmax - xymargin; h1 = ymax / 3f - xymargin;

        Nfft = 1024;
        _tmax = 5f;
        _Fs = Nfft/(2 * _tmax); // sampling rate
        Nfreq = 50;    // how many harmonics to display

        _mag = 1.5f; _freq = 1.1f; _phase = 0;

        
        
        topPlot.CreateGrid(-xmax/2, ymax/2, (xmax/2)-0.2f, ymax/2, Color.grey, defaultLineMaterial);

        topRightPlot.CreateGrid(xmax/2, ymax/2, (xmax/2) - 0.2f, ymax/2, Color.grey, defaultLineMaterial);
        //OriginLabel.gameObject.SetActive(false);

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        
        
        //topPlot.Update();
        
    }

    
}
