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

    private float xTopLeftPlot, yTopLeftPlot, xTopRightPlot, yTopRightPlot, topPlotsWidth, topPlotsHeight;

    private int Nfft, Nfreq;
    private float _Fs, _tmax;
    public float _mag, _freq, _phase;

    private List<Vector2> _func1pts = new List<Vector2>();
    private List<Vector2> _func2pts = new List<Vector2>();
    private List<Vector2> _func3pts = new List<Vector2>();

    //private Vector2[] _func1pts, _funct2pts, _resultpts;

    private LineRenderer _func1, _func2, _result;



    void Awake()
    {

        _height = Camera.main.orthographicSize;
        _width = _height * Camera.main.aspect;

        _height = _height - 0.5f;
        _width = _width - 0.5f;
        _xscale = 1.1f / (2 * _width);
        _yscale = 0.2f;



        Debug.LogFormat("_width: {0}, _height: {1}", _width, _height);

        
        float xymargin = 0.5f;


        xTopLeftPlot = - _width / 2;
        yTopLeftPlot =  _height / 2;
        
        xTopRightPlot = _width / 2;
        yTopRightPlot =  _height / 2;

        topPlotsWidth = (_width / 2) - 0.2f;
        topPlotsHeight =  _height / 2;
       

        

        



        topPlot.CreateGrid(xTopLeftPlot, yTopLeftPlot, topPlotsWidth, topPlotsHeight, Color.grey, defaultLineMaterial);

        topRightPlot.CreateGrid(xTopRightPlot, yTopRightPlot, topPlotsWidth, topPlotsHeight, Color.grey, defaultLineMaterial);
        //OriginLabel.gameObject.SetActive(false);


    }


    // Start is called before the first frame update
    void Start()
    {
        _xscale = 1.1f / (2 * _width);
        _yscale = 0.2f;

        GameObject lineContainer = new GameObject("Func1");
        lineContainer.transform.SetParent(transform, false);

        Color plotColor = Color.red;
        _func1 = lineContainer.AddComponent<LineRenderer>();
        _func1.material = defaultLineMaterial;
        _func1.useWorldSpace = true;
        _func1.startWidth = 0.2f;
        _func1.endWidth = 0.2f;
        _func1.startColor = plotColor;
        _func1.endColor = plotColor;
        _func1.positionCount = 5;  // need at least 2


        GameObject lineContainer2 = new GameObject("Func2");
        lineContainer2.transform.SetParent(transform, false);
        _func2 = lineContainer2.AddComponent<LineRenderer>();
        _func2.material = defaultLineMaterial;
        _func2.useWorldSpace = true;
        _func2.startWidth = 0.2f;
        _func2.endWidth = 0.2f;
        _func2.startColor = plotColor;
        _func2.endColor = plotColor;
        _func2.positionCount = 5;  // need at least 2





        //Sample sawtooth wave
        _func1pts.Add(new Vector3(((xTopLeftPlot-(topPlotsWidth/2))),(yTopLeftPlot),0.0f));
        _func1pts.Add(new Vector3((xTopLeftPlot),(yTopLeftPlot),0.0f));
        _func1pts.Add(new Vector3((xTopLeftPlot),(yTopLeftPlot+(topPlotsHeight/2)),0.0f));
        _func1pts.Add(new Vector3(((xTopLeftPlot+(topPlotsWidth/2))),(yTopLeftPlot),0.0f));
        _func1pts.Add(new Vector3(((xTopLeftPlot+(topPlotsWidth))),(yTopLeftPlot),0.0f));

        //Sample sawtooth wave
        _func2pts.Add(new Vector3(((xTopRightPlot-(topPlotsWidth/2))),(yTopRightPlot),0.0f));
        _func2pts.Add(new Vector3((xTopRightPlot),(yTopRightPlot),0.0f));
        _func2pts.Add(new Vector3((xTopRightPlot),(yTopRightPlot+(topPlotsHeight/2)),0.0f));
        _func2pts.Add(new Vector3(((xTopRightPlot+(topPlotsWidth/2))),(yTopRightPlot),0.0f));
        _func2pts.Add(new Vector3(((xTopRightPlot+(topPlotsWidth))),(yTopRightPlot),0.0f));


        for (int i = 0; i < _func1pts.Count; i++)
        {
            _func1.SetPosition(i, _func1pts[i]);
        }

        for (int i = 0; i < _func2pts.Count; i++)
        {
            _func2.SetPosition(i, _func2pts[i]);
        }

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _func1pts.Count; i++)
        {
            _func1.SetPosition(i, _func1pts[i]);
        }
        for (int i = 0; i < _func2pts.Count; i++)
        {
            _func2.SetPosition(i, _func2pts[i]);
        }

        //topPlot.Update();

    }

    Vector2 ToScreenCoords(Vector2 funccoords)
    {
        return (new Vector3(-_width + funccoords.x / _xscale, funccoords.y / _yscale));
    }

    void UpdatePoints (){
        //TODO make a funt that loops through the functpts lists and setpositions them all. this funct can then be called once at awake and
        //then once every update for less clutter

    }
}
