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
    private List<Vector2> _func4pts = new List<Vector2>();


    private GameObject lineContainer;
    public Transform _funct1position;
    
    //private Vector2[] _func1pts, _funct2pts, _resultpts;

    private LineRenderer _func1, _func2,_func3, _func4;



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

        //Center points for the background grid topPlots
        xTopLeftPlot = - _width / 2;
        yTopLeftPlot =  _height / 2;
        
        xTopRightPlot = _width / 2;
        yTopRightPlot =  _height / 2;

        //height and width of topplots
        topPlotsWidth = (_width / 2) - 0.2f;
        topPlotsHeight =  _height / 2;

        topPlot.CreateGrid(xTopLeftPlot, yTopLeftPlot, topPlotsWidth, topPlotsHeight, Color.grey, defaultLineMaterial);

        topRightPlot.CreateGrid(xTopRightPlot, yTopRightPlot, topPlotsWidth, topPlotsHeight, Color.grey, defaultLineMaterial);

        bottomPlot.CreateGrid(0,-_height/2, _width- 0.2f, _height/1.5f,Color.grey, defaultLineMaterial);
        //OriginLabel.gameObject.SetActive(false);


    }


    // Start is called before the first frame update
    void Start()
    {
        _xscale = 1.1f / (2 * _width);
        _yscale = 0.2f;

        lineContainer = new GameObject("Func1");
        lineContainer.transform.SetParent(transform, false);
        //lineContainer.position
        Color plotLeftColor = Color.red;
        _func1 = lineContainer.AddComponent<LineRenderer>();
        // _func1.material = defaultLineMaterial;
        // _func1.useWorldSpace = true;
        // _func1.startWidth = 0.2f;
        // _func1.endWidth = 0.2f;
        // _func1.startColor = plotLeftColor;
        // _func1.endColor = plotLeftColor;
        // _func1.positionCount = 5;  // need at least 2

        SetWavePoints(lineContainer, plotLeftColor, "blank");

        lineContainer.transform.localScale = new Vector3(0.5f, 0.5f, 0f);

        // lineContainer.AddComponent("MeshFilter");
        // lineContainer.AddComponent("MeshRenderer");
        // var mesh : Mesh = GetComponent(MeshFilter).mesh;
        // mesh.Clear();
        // mesh.vertices = [new Vector3(0,0,0),new Vector3(0,1,0),new Vector3(1, 1, 0)];
        // mesh.uv = [new Vector2 (0, 0), new Vector2 (0, 1), new Vector2 (1, 1)];
        // mesh.triangles = [0, 1, 2];



        //function 2 top
        GameObject lineContainer2 = new GameObject("Func2");
        lineContainer2.transform.SetParent(transform, false);
        Color plotRightColor = Color.green;
        _func2 = lineContainer2.AddComponent<LineRenderer>();
        
        SetWavePoints(lineContainer2, plotRightColor, "blank");
        


        //function1 bottom
        GameObject lineContainer3 = new GameObject("Func3");
        lineContainer3.transform.SetParent(transform, false);
        _func3 = lineContainer3.AddComponent<LineRenderer>();
        SetWavePoints(lineContainer3, plotLeftColor, "blank");

        //function2 bottom
        GameObject lineContainer4 = new GameObject("Func4");
        lineContainer4.transform.SetParent(transform, false);
        _func4 = lineContainer4.AddComponent<LineRenderer>();
        SetWavePoints(lineContainer4, plotRightColor, "blank");


        // //Sample sawtooth wave
        // _func1pts.Add(new Vector3(((xTopLeftPlot-(topPlotsWidth/2))),(yTopLeftPlot),0.0f));
        // _func1pts.Add(new Vector3((xTopLeftPlot),(yTopLeftPlot),0.0f));
        // _func1pts.Add(new Vector3((xTopLeftPlot),(yTopLeftPlot+(topPlotsHeight/2)),0.0f));
        // _func1pts.Add(new Vector3(((xTopLeftPlot+(topPlotsWidth/2))),(yTopLeftPlot),0.0f));
        // _func1pts.Add(new Vector3(((xTopLeftPlot+(topPlotsWidth))),(yTopLeftPlot),0.0f));

        //Sample sawtooth wave
        // _func2pts.Add(new Vector3(((xTopRightPlot-(topPlotsWidth/2))),(yTopRightPlot),0.0f));
        // _func2pts.Add(new Vector3((xTopRightPlot),(yTopRightPlot),0.0f));
        // _func2pts.Add(new Vector3((xTopRightPlot),(yTopRightPlot+(topPlotsHeight/2)),0.0f));
        // _func2pts.Add(new Vector3(((xTopRightPlot+(topPlotsWidth/2))),(yTopRightPlot),0.0f));
        // _func2pts.Add(new Vector3(((xTopRightPlot+(topPlotsWidth))),(yTopRightPlot),0.0f));


        //Sample sawtooth wave
       

        
        // for (int i = 0; i < _func1pts.Count; i++)
        // {
        //     _func1.SetPosition(i, _func1pts[i]);
        // }
        
        

        // for (int i = 0; i < _func2pts.Count; i++)
        // {
        //     _func2.SetPosition(i, _func2pts[i]);
        // }

        
        //lineContainer.transform.position = new Vector3(0, 0, 0);
       // BakeLineDebuger(lineContainer);
    }

    // Update is called once per frame
    void Update()
    {

        
        //SetWavePoints(lineContainer, Color.red, "blank");
       // Func1.transform.Translate(1, 1, 1);
        // for (int i = 0; i < _func1pts.Count; i++)
        // {
        //     _func1.SetPosition(i, _func1pts[i]);
        // }
        // for (int i = 0; i < _func2pts.Count; i++)
        // {
        //     _func2.SetPosition(i, _func2pts[i]);
        // }

        

        //topPlot.Update();

    }

    Vector2 ToScreenCoords(Vector2 funccoords)
    {
        return (new Vector3(-_width + funccoords.x / _xscale, funccoords.y / _yscale));
    }


    //A static function that transforms a gameobj with a Linerenderer into a gameobj with a mesh of the Linerenderer. This allows us
    // to use gamobj.transform to move our linerenderer "images" around
    public static void BakeLineDebuger(GameObject lineObj)
    {
        var lineRenderer = lineObj.GetComponent<LineRenderer>();
        var meshFilter = lineObj.AddComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        lineRenderer.BakeMesh(mesh);
        meshFilter.sharedMesh = mesh;
 
        var meshRenderer = lineObj.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        
        GameObject.Destroy(lineRenderer);
    }

    public void SetWavePoints(GameObject lineObj, Color color, string waveType){
        var lineRenderer = lineObj.GetComponent<LineRenderer>();


        lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        lineRenderer.useWorldSpace = true;
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.positionCount = 5;

        List<Vector2> pointsList = new List<Vector2>();

        pointsList.Add(new Vector3((-_width/2),(0),0.0f));
        pointsList.Add(new Vector3((0),(0),0.0f));
        pointsList.Add(new Vector3((0),(_height/2),0.0f));
        pointsList.Add(new Vector3((_width/2),(0),0.0f));
        pointsList.Add(new Vector3((_width-0.02f),(0),0.0f));

        for (int i = 0; i < pointsList.Count; i++)
        {
            lineRenderer.SetPosition(i, pointsList[i]);
        }

        BakeLineDebuger(lineObj);

    }
}
