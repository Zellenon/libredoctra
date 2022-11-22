// using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Complex = System.Numerics.Complex;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using TMPro;

public class Convolution : MonoBehaviour
{
    [SerializeField] private Material defaultLineMaterial;
    [SerializeField] private Material lineMaterial;

    [SerializeField] private TextMeshPro OriginLabel;
    [SerializeField] private PlotObj topLeftPlot, topRightPlot, bottomPlot, convPlot;
    // [SerializeField] private EquationText eqnText;

    private float xtlp, ytlp, wtlp, htlp,
    xtrp, ytrp, wtrp, htrp,
    xbp, ybp, wbp, hbp;

    public float _funct1mag, _funct1freq,
    _funct2mag, _funct2freq;

    private float _width, _height, _xscale, _yscale, xymargin;

    private const int STEPCOUNT = 200;
    private const float MAX_X = 4.0f;

    private float xTopLeftPlot, yTopLeftPlot, xTopRightPlot, yTopRightPlot;
    private float topPlotsWidth, topPlotsHeight, xConvolveGraph, YConvolveGraph;
    private float convolveGraphWidth, convolveGraphHeight;

    private int Nfft, Nfreq;
    private float _Fs, _tmax;
    public float _mag, _freq, _phase;

    private List<Vector2> _func1pts = new List<Vector2>();
    private List<Vector2> _func2pts = new List<Vector2>();
    private List<Vector2> _func3pts = new List<Vector2>();
    private List<Vector2> _func4pts = new List<Vector2>();
    private List<Vector2> _func5pts = new List<Vector2>();

    //Container 1 is top left, 2 is top right, 3 is top left on the bottom graph, 4 is the inverted top right wave, 5 is the convolved wave
    private GameObject lineContainer, lineContainer2, lineContainer3, lineContainer4, lineContainer5;
    public Transform _funct1position;

    private LineRenderer _func1, _func2, _func3, _func4, _func5;

    // these variables will refer to the relative x position of the functions as they are being convolved together
    private float funct1xPos, funct2xPos;

    private int interval = 1;
    private float nextTime = 0;

    private UIDocument _doc;

    private AbstractWave _waveA, _waveB;
    private float[] _waveC = new float[STEPCOUNT * 2];
    private float _convolutionMask;

    private bool _redrawWaveFlag;
    private bool _redrawConvFlag;

    void Awake()
    {
        _height = Camera.main.orthographicSize;
        _width = _height * Camera.main.aspect;

        _height = _height - 0.5f;
        _width = _width - 0.5f;
        _xscale = 1.1f / (2 * _width);
        _yscale = 0.2f;

        Debug.LogFormat("_width: {0}, _height: {1}", _width, _height);

        xymargin = 0.5f;

        // Center points for the background grid topPlots
        xTopLeftPlot = -_width;
        yTopLeftPlot = _height / 2f;

        xTopRightPlot = 0 + xymargin;
        yTopRightPlot = _height / 2f;

        // height and width of topplots
        topPlotsWidth = (_width / 2f) - 0.2f;
        topPlotsHeight = _height / 2f;

        xConvolveGraph = (-_width / 4f) + xymargin;
        YConvolveGraph = 0f;
        convolveGraphWidth = _width * 0.8f;
        convolveGraphHeight = _height / 1.5f;

        topLeftPlot.CreateLGrid(xTopLeftPlot, yTopLeftPlot, topPlotsWidth, topPlotsHeight, Color.grey, defaultLineMaterial);

        topRightPlot.CreateLGrid(xTopRightPlot, yTopRightPlot, topPlotsWidth, topPlotsHeight, Color.grey, defaultLineMaterial);

        bottomPlot.CreateGrid(xConvolveGraph, YConvolveGraph, convolveGraphWidth, convolveGraphHeight, Color.grey, defaultLineMaterial);

        convPlot.CreateGrid(xConvolveGraph, -3f * _height / 4f, convolveGraphWidth, convolveGraphHeight, Color.grey, defaultLineMaterial);

        for (int i = 0; i < STEPCOUNT * 2; i++)
        {
            _waveC[i] = 0.0f;
        }

        _doc = GetComponent<UIDocument>();
        SetupButtonHandlers();
        var slider = _doc.rootVisualElement.Query<Slider>().First();
        slider.RegisterValueChangedCallback(ConvolveCallback);
    }

    // Start is called before the first frame update
    void Start()
    {
        _xscale = 1.1f / (2 * _width);
        _yscale = 0.2f;

        _waveA = new Boxcar();
        _waveB = new Triangle();

        // function 1 top
        lineContainer = new GameObject("Func1");
        lineContainer.transform.SetParent(transform, false);
        Color plotLeftColor = Color.red;
        _func1 = lineContainer.AddComponent<LineRenderer>();
        makeWave(lineContainer, plotLeftColor, _waveA);

        lineContainer.transform.localScale = new Vector3(0.5f, 0.5f, 0f);
        lineContainer.transform.position = new Vector3((xTopLeftPlot), (yTopLeftPlot), 0f);

        // function 2 top
        lineContainer2 = new GameObject("Func2");
        lineContainer2.transform.SetParent(transform, false);
        Color plotRightColor = Color.green;
        _func2 = lineContainer2.AddComponent<LineRenderer>();

        makeWave(lineContainer2, plotRightColor, _waveB);
        lineContainer2.transform.localScale = new Vector3(0.5f, 0.5f, 0f);
        lineContainer2.transform.position = new Vector3((xTopRightPlot), (yTopRightPlot), 0f);

        // function1 bottom
        lineContainer3 = new GameObject("Func3");
        lineContainer3.transform.SetParent(transform, false);
        _func3 = lineContainer3.AddComponent<LineRenderer>();
        makeWave(lineContainer3, plotLeftColor, _waveA);
        lineContainer3.transform.localScale = new Vector3(0.5f, 0.5f, 0f);
        lineContainer3.transform.position = new Vector3(xConvolveGraph, YConvolveGraph, 0f);

        // function2 bottom
        lineContainer4 = new GameObject("Func4");
        lineContainer4.transform.SetParent(transform, false);
        _func4 = lineContainer4.AddComponent<LineRenderer>();
        makeWave(lineContainer4, plotRightColor, _waveB);
        lineContainer4.transform.localScale = new Vector3(-0.5f, 0.5f, 0f);
        lineContainer4.transform.position = new Vector3(xConvolveGraph, YConvolveGraph, 0f);

        lineContainer5 = new GameObject("Func5 - Convolution");
        lineContainer5.transform.SetParent(transform, false);
        _func5 = lineContainer5.AddComponent<LineRenderer>();
        lineContainer5.transform.position = new Vector3(xConvolveGraph, -3 * _height / 4, 0f);
        lineContainer5.transform.localScale = new Vector3(0.5f, 0.5f, 0f);

        _redrawWaveFlag = true;
        _redrawConvFlag = true;
        //redrawGraphs();
    }

    // Update is called once per frame
    void Update()
    {
        if (_redrawWaveFlag)
        {
            redrawGraphs();
            _redrawWaveFlag = false;
        }
        if (_redrawConvFlag)
        {
            redrawConvGraph();
            _redrawConvFlag = false;
        }

    }

    private void SetupButtonHandlers()
    {
        var buttons = _doc.rootVisualElement.Query<Button>();
        buttons.ForEach(RegisterHandler);
    }

    private void RegisterHandler(Button button)
    {
        button.RegisterCallback<ClickEvent>(LoadWaveCallback);
    }

    private void LoadWaveCallback(ClickEvent evt)
    {
        Button button = evt.currentTarget as Button;
        string waveType = button.name.Split("-")[0];
        string plotName = button.name.Split("-")[1];

        bool isButtonA = plotName == "A";

        if (lineContainer.GetComponent<MeshRenderer>() != null)
        {
            Destroy(lineContainer.GetComponent<MeshRenderer>());
            Destroy(lineContainer.GetComponent<MeshFilter>());
            Destroy(lineContainer3.GetComponent<MeshRenderer>());
            Destroy(lineContainer3.GetComponent<MeshFilter>());
        }
        if (lineContainer2.GetComponent<MeshRenderer>() != null)
        {
            Destroy(lineContainer2.GetComponent<MeshRenderer>());
            Destroy(lineContainer2.GetComponent<MeshFilter>());
            Destroy(lineContainer4.GetComponent<MeshRenderer>());
            Destroy(lineContainer4.GetComponent<MeshFilter>());
        }

        switch (waveType)
        {
            case "Dirac":
                if (isButtonA)
                {
                    SetWaveA<Dirac>();
                }
                else
                {
                    SetWaveB<Dirac>();
                }
                break;
            case "Sawtooth":
                if (isButtonA)
                {
                    SetWaveA<Sawtooth>();
                }
                else
                {
                    SetWaveB<Sawtooth>();
                }
                break;
            case "Echo":
                if (isButtonA)
                {
                    SetWaveA<Echo>();
                }
                else
                {
                    SetWaveB<Echo>();
                }
                break;
            case "Boxcar":
                if (isButtonA)
                {
                    SetWaveA<Boxcar>();
                }
                else
                {
                    SetWaveB<Boxcar>();
                }
                break;
            case "Triangle":
                if (isButtonA)
                {
                    SetWaveA<Triangle>();
                }
                else
                {
                    SetWaveB<Triangle>();
                }
                break;
            case "Sine":
                if (isButtonA)
                {
                    SetWaveA<Sine>();
                }
                else
                {
                    SetWaveB<Sine>();
                }
                break;
        }
        _redrawWaveFlag = true;

        float multiplier = MAX_X / STEPCOUNT;
        for (int T = 0; T < STEPCOUNT * 2; T++)
        {
            float convsum = 0.0f;
            for (int i = 0; i <= T; i++)
            {
                // print("Convolving " + _waveA.get(i* multiplier).ToString() + " with " + _waveB.get(i * multiplier).ToString() + " with offset " + offset.ToString());
                convsum += _waveA.convolve(_waveB, T * multiplier, i * multiplier);
            }
            _waveC[T] = convsum;
        }
    }

    private void ConvolveCallback(ChangeEvent<float> evt)
    {
        // print(evt.newValue);
        int offset = (int)evt.newValue;
        lineContainer4.transform.position = new Vector3(xConvolveGraph + (offset / 100.0f), (0.0f), 0f);
        _convolutionMask = offset;
        
        _redrawConvFlag = true;

    }

    public void SetWaveA<T>() where T : AbstractWave, new()
    {
        _waveA = new T();
        _waveA.frequency(1);
        _waveA.amplitude(1);
    }

    public void SetWaveA(AbstractWave wave)
    {
        _waveA = wave;
    }

    public void SetWaveB<T>() where T : AbstractWave, new()
    {
        _waveB = new T();
        _waveB.frequency(1);
        _waveB.amplitude(1);
    }

    public void SetWaveB(AbstractWave wave)
    {
        _waveB = wave;
    }

    Vector2 ToScreenCoords(Vector2 funccoords)
    {
        return (new Vector3(-_width + funccoords.x / _xscale, funccoords.y / _yscale));
    }

    // A static function that transforms a gameobj with a Linerenderer into a gameobj with a mesh of the Linerenderer. This allows us
    // to use gamobj.transform to move our linerenderer "images" around
    public static void BakeLineDebuger(GameObject lineObj)
    {
        var lineRenderer = lineObj.GetComponent<LineRenderer>();

        // Unity Shenanagins not properly universally adding components without help
        MeshFilter meshFilter = TrueGetComponent<MeshFilter>(lineObj);

        Mesh mesh = new Mesh();
        lineRenderer.BakeMesh(mesh);
        meshFilter.sharedMesh = mesh;

        // Unity Shenanagins not properly universally adding components without help
        MeshRenderer meshRenderer = TrueGetComponent<MeshRenderer>(lineObj);

        meshRenderer.sharedMaterial = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));

        GameObject.Destroy(lineRenderer);
    }

    public void redrawGraphs()
    {
        // Draw graph of wave A
        if (_waveA == null) _waveA = new Boxcar();
        lineContainer.AddComponent<LineRenderer>();
        makeWave(lineContainer, Color.red, _waveA);

        // Draw graph of wave B
        if (_waveB == null) _waveB = new Boxcar();
        lineContainer2.AddComponent<LineRenderer>();
        makeWave(lineContainer2, Color.green, _waveB);

        // Draw both lines in the mixing graph
        lineContainer3.AddComponent<LineRenderer>();
        makeWave(lineContainer3, Color.red, _waveA);
        lineContainer4.AddComponent<LineRenderer>();
        makeWave(lineContainer4, Color.green, _waveB);
        
        
    }
    public void redrawConvGraph(){

        // Draw line in the result graph
        lineContainer5.AddComponent<LineRenderer>();
        drawConvolution();
    }

    public void makeWave(GameObject lineObj, Color color, AbstractWave wave)
    {
        float xScaled = -_width / 2;
        makeWave(lineObj, color, wave, xScaled);
    }

    public void makeWave(GameObject lineObj, Color color, AbstractWave wave, float xPos)
    {

        if (lineObj.GetComponent<MeshRenderer>() != null)
        {
            Destroy(lineObj.GetComponent<MeshRenderer>());
            Destroy(lineObj.GetComponent<MeshFilter>());
        }

        var lineRenderer = lineObj.GetComponent<LineRenderer>();

        lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        lineRenderer.useWorldSpace = true;
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        // The scaled value for which to increment the functions x value
        float incrementValue = MAX_X / STEPCOUNT;

        //                    [Game Screen]
        ///////////////////////////////////////////////////////
        //                         |                         //
        //                         |                         //
        // (-_width/2,0)           |                    (_width/2,0)
        //  |                      |                        |//
        //  v                      |                        V//
        // ------------------------+-------------------------//
        //                         |                         //
        //                         |                         //
        //                         |                         //
        //                         |                         //
        //                         |                         //
        ///////////////////////////////////////////////////////
        // The scaled minimum x value for the function; In this case we want our lines to have a length of 1/4 the screen
        float xScaled = xPos;

        // A list to stor our new scaled xvalues
        var xList = new List<float>(STEPCOUNT);

        for (int i = 0; i < STEPCOUNT; ++i)
        {

            xList.Add(i * incrementValue);
        }

        // A list of Vecter2s to store both the xy points we want linerenderer to connect
        List<Vector2> pointsList = new List<Vector2>();
        // passing string "str" in
        // switch statement
        lineRenderer.positionCount = STEPCOUNT;
        for (int i = 0; i < STEPCOUNT; ++i)
        {
            pointsList.Add(new Vector3(xList[i], wave.get(xList[i]), 0.0f));
        }

        for (int i = 0; i < pointsList.Count; i++)
        {
            lineRenderer.SetPosition(i, pointsList[i]);
        }
        BakeLineDebuger(lineObj);
    }

    public void drawConvolution()
    {

        _func5pts.Clear();

        var lineRenderer = lineContainer5.GetComponent<LineRenderer>();

        lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        lineRenderer.useWorldSpace = true;
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.startColor = Color.magenta;
        lineRenderer.endColor = Color.magenta;
        lineRenderer.positionCount = 2 * STEPCOUNT;

        // Scale the X value down to a displayable size; in this case dividing the screen width by the stepcount
        float incrementValue = _width / (2f * STEPCOUNT);
        float xScaled = 0f;
        var xList = new List<float>(2 * STEPCOUNT);
        for (int i = 0; i < 2 * STEPCOUNT; ++i)
        {
            xList.Add(i * incrementValue);
        }

        // We also need to scale down our y values; but somtimes the max will be 1 and other times the max could be hundreds
        //      we need to dynamically adjust the y value based on the largest value in _waveC

        float yMaxValue = 5f;
        if (_waveC.Max() != 0)
        {
            yMaxValue = _waveC.Max();
        }
        int yMaxIndex = _waveC.ToList().IndexOf(yMaxValue);

        //looking at our displayed graphs we see that the max height looks to be about 5 units (top of the grey line) so we will take our
        //      max and divide by yMaxValue/5 this means that the peak displayed value should never exceed the graph height.
        float[] yValuesScaled = new float[STEPCOUNT * 2];

        // make our nicely scaled array with our  ugly _waveC array
        for (int i = 0; i < _waveC.Length; i++)
        {
            yValuesScaled[i] = (5f * _waveC[i] / yMaxValue);
        }

        //Add our scaled values to a vector array for the line renderer to iterate over
        for (int i = 0; i < _waveC.Length; i++)
        {
            if (i < _convolutionMask) _func5pts.Add(new Vector3(xList[i], yValuesScaled[i], 0.0f));
            else _func5pts.Add(new Vector3(xList[i], 0.0f, 0.0f));
        }

        //Iterate the line renderer
        for (int i = 0; i < _func5pts.Count; i++)
        {
            lineRenderer.SetPosition(i, _func5pts[i]);
        }
        BakeLineDebuger(lineContainer5);

    }

    private static T TrueGetComponent<T>(GameObject obj) where T : Component
    {
        T comp = obj.GetComponent<T>();

        if (comp == null)
        {
            comp = obj.AddComponent<T>();
        }
        if (comp == null)
        {
            comp = obj.GetComponent<T>();
        }
        return comp;
    }
}
