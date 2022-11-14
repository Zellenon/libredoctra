// using System;
using System.Collections;
using System.Collections.Generic;
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

    private const int STEPCOUNT = 200;

    private float xTopLeftPlot, yTopLeftPlot, xTopRightPlot, yTopRightPlot, topPlotsWidth, topPlotsHeight;

    private int Nfft, Nfreq;
    private float _Fs, _tmax;
    public float _mag, _freq, _phase;

    private List<Vector2> _func1pts = new List<Vector2>();
    private List<Vector2> _func2pts = new List<Vector2>();
    private List<Vector2> _func3pts = new List<Vector2>();
    private List<Vector2> _func4pts = new List<Vector2>();

    private GameObject lineContainer,lineContainer2,lineContainer3,lineContainer4;
    public Transform _funct1position;
    
    // private Vector2[] _func1pts, _funct2pts, _resultpts;

    private LineRenderer _func1, _func2,_func3, _func4;

    // these variables will refer to the relative x position of the functions as they are being convolved together
    private float funct1xPos, funct2xPos;

    private int interval = 1;
    private float nextTime = 0;

    private UIDocument _doc;

    private AbstractWave _waveA, _waveB;

    private bool _redrawFlag;

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

        // Center points for the background grid topPlots
        xTopLeftPlot = - _width / 2;
        yTopLeftPlot =  _height / 2;
        
        xTopRightPlot = _width / 2;
        yTopRightPlot =  _height / 2;

        // height and width of topplots
        topPlotsWidth = (_width / 2) - 0.2f;
        topPlotsHeight =  _height / 2;

        topPlot.CreateGrid(xTopLeftPlot, yTopLeftPlot, topPlotsWidth, topPlotsHeight, Color.grey, defaultLineMaterial);

        topRightPlot.CreateGrid(xTopRightPlot, yTopRightPlot, topPlotsWidth, topPlotsHeight, Color.grey, defaultLineMaterial);

        bottomPlot.CreateGrid(0,0, _width- 0.2f, _height/1.5f,Color.grey, defaultLineMaterial);
        // OriginLabel.gameObject.SetActive(false);
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

        // function 1 top
        lineContainer = new GameObject("Func1");
        lineContainer.transform.SetParent(transform, false);
        Color plotLeftColor = Color.red;
        _func1 = lineContainer.AddComponent<LineRenderer>();
        makeWave<Sine>(lineContainer, plotLeftColor);

        lineContainer.transform.localScale = new Vector3(0.5f, 0.5f, 0f);
        lineContainer.transform.position = new Vector3((-_width/2f),(_height/2f),0f);

        // function 2 top
        lineContainer2 = new GameObject("Func2");
        lineContainer2.transform.SetParent(transform, false);
        Color plotRightColor = Color.green;
        _func2 = lineContainer2.AddComponent<LineRenderer>();
        
        makeWave<Boxcar>(lineContainer2, plotRightColor);
        lineContainer2.transform.localScale = new Vector3(0.5f, 0.5f, 0f);
        lineContainer2.transform.position = new Vector3((_width/2f),(_height/2f),0f);

        // function1 bottom
        lineContainer3 = new GameObject("Func3");
        lineContainer3.transform.SetParent(transform, false);
        _func3 = lineContainer3.AddComponent<LineRenderer>();
        makeWave<Sine>(lineContainer3, plotLeftColor);
        lineContainer3.transform.localScale = new Vector3(0.5f, 0.5f, 0f);
        lineContainer3.transform.position = new Vector3((0),(0),0f);

        // function2 bottom
        lineContainer4 = new GameObject("Func4");
        lineContainer4.transform.SetParent(transform, false);
        _func4 = lineContainer4.AddComponent<LineRenderer>();
        makeWave<Boxcar>(lineContainer4, plotRightColor);
        lineContainer4.transform.localScale = new Vector3(-0.5f, 0.5f, 0f);
        lineContainer4.transform.position = new Vector3((0),(-_height/2f),0f);

        _redrawFlag = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextTime) {
            lineContainer4.transform.position = new Vector3((nextTime),(0),0f);
            nextTime += interval;
        }
        if (_redrawFlag) {
            redrawGraphs();
            _redrawFlag = false;
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

        bool isButtonA = plotName=="A";

        switch(waveType){
        case "Dirac":
            if (isButtonA) {
                SetWaveA<Dirac>();
            } else {
                SetWaveB<Dirac>();
            }
            break;
        case "Sawtooth":
            if (isButtonA) {
                SetWaveA<Sawtooth>();
            } else {
                SetWaveB<Sawtooth>();
            }
            break;
        case "Echo":
            if (isButtonA) {
                SetWaveA<Echo>();
            } else {
                SetWaveB<Echo>();
            }
            break;
        case "Boxcar":
            if (isButtonA) {
                SetWaveA<Boxcar>();
            } else {
                SetWaveB<Boxcar>();
            }
            break;
        case "Triangle":
            if (isButtonA) {
                SetWaveA<Triangle>();
            } else {
                SetWaveB<Triangle>();
            }
            break;
        case "Sine":
            if (isButtonA) {
                SetWaveA<Sine>();
            } else {
                SetWaveB<Sine>();
            }
            break;
        }
        _redrawFlag = true;
        // Debug.Log("Button was clicked!");
    }

    private void ConvolveCallback(ChangeEvent<float> evt) {
        print(evt.newValue);
    }

    public void SetWaveA<T>() where T: AbstractWave, new(){
        _waveA = new T();
        _waveA.frequency(1);
        _waveA.amplitude(1);
    }

    public void SetWaveA(AbstractWave wave) {
        _waveA = wave;
    }

    public void SetWaveB<T>() where T: AbstractWave, new(){
        _waveB = new T();
        _waveB.frequency(1);
        _waveB.amplitude(1);
    }

    public void SetWaveB(AbstractWave wave) {
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
        MeshFilter meshFilter = lineObj.GetComponent<MeshFilter>();

        if (meshFilter == null){
            meshFilter = lineObj.AddComponent<MeshFilter>();
        }
        if (meshFilter == null){
            meshFilter = lineObj.GetComponent<MeshFilter>();
        }

        Mesh mesh = new Mesh();
        lineRenderer.BakeMesh(mesh);
        meshFilter.sharedMesh = mesh;
 
        // Unity Shenanagins not properly universally adding components without help
        MeshRenderer meshRenderer = lineObj.GetComponent<MeshRenderer>();
        if (meshRenderer == null){
            meshRenderer = lineObj.AddComponent<MeshRenderer>();
        }
        if (meshRenderer == null){
            meshRenderer = lineObj.GetComponent<MeshRenderer>();
        }

        meshRenderer.sharedMaterial = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        
        GameObject.Destroy(lineRenderer);
    }

    public void redrawGraphs() {
        // Draw graph of wave A
        if (_waveA == null) _waveA = new Boxcar();
        lineContainer.AddComponent<LineRenderer>();
        makeWave(lineContainer,Color.red, _waveA);

        // Draw graph of wave B
        if (_waveB == null) _waveB = new Boxcar();
        lineContainer2.AddComponent<LineRenderer>();
        makeWave(lineContainer2,Color.red, _waveB);

        // Draw both lines in the mixing graph

        // Draw line in the result graph
    }

    public void makeWave<T>(GameObject lineObj, Color color) where T: AbstractWave, new(){
        float xScaled = -_width/2;
        makeWave<T>(lineObj, color, xScaled);
    }

    public void makeWave<T>(GameObject lineObj, Color color, float xPos) where T: AbstractWave, new() {

        if (lineObj.GetComponent<MeshRenderer>() != null){
            Destroy(lineObj.GetComponent<MeshRenderer>());
            Destroy(lineObj.GetComponent<MeshFilter>());
            // Destroy(lineObj.material);
        }


        AbstractWave wave;
        var lineRenderer = lineObj.GetComponent<LineRenderer>();

        lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        lineRenderer.useWorldSpace = true;
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        
        // The scaled value for which to increment the functions x value
        float incrementValue = _width/STEPCOUNT;

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

        for (int i = 0; i < STEPCOUNT; ++i){
            
            xList.Add(xScaled+(i*incrementValue));
        }

        // A list of Vecter2s to store both the xy points we want linerenderer to connect
        List<Vector2> pointsList = new List<Vector2>();
        // passing string "str" in
        // switch statement
        lineRenderer.positionCount = STEPCOUNT;
        wave = new T();
        wave.frequency(1);
        wave.amplitude(1);
        for (int i = 0; i < STEPCOUNT; ++i){
            pointsList.Add(new Vector3(xList[i],wave.get(xList[i]),0.0f));
        }

        for (int i = 0; i < pointsList.Count; i++)
        {
            lineRenderer.SetPosition(i, pointsList[i]);
        }
        BakeLineDebuger(lineObj);
    }

    public void makeWave(GameObject lineObj, Color color, AbstractWave wave) {
        float xScaled = -_width/2;
        makeWave(lineObj, color, wave, xScaled);
    }

    public void makeWave(GameObject lineObj, Color color, AbstractWave wave, float xPos) {

        if (lineObj.GetComponent<MeshRenderer>() != null){
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
        float incrementValue = _width/STEPCOUNT;

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

        for (int i = 0; i < STEPCOUNT; ++i){

            xList.Add(i*incrementValue);
        }

        // A list of Vecter2s to store both the xy points we want linerenderer to connect
        List<Vector2> pointsList = new List<Vector2>();
        // passing string "str" in
        // switch statement
        lineRenderer.positionCount = STEPCOUNT;
        for (int i = 0; i < STEPCOUNT; ++i){
            pointsList.Add(new Vector3(xList[i],wave.get(xList[i]),0.0f));
        }

        for (int i = 0; i < pointsList.Count; i++)
        {
            lineRenderer.SetPosition(i, pointsList[i]);
        }
        BakeLineDebuger(lineObj);
    }
}
