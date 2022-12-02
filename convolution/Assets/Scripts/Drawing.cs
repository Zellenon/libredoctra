using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Drawing : MonoBehaviour
{
    private Convolution _conv;

    private float _width, _height, _xscale, _yscale, xymargin;

    private float[] _drawnPoints;
    private int _last_x;

    private UIDocument _doc;
    private Label _error_label;

    private GameObject _drawnObject;
    private LineRenderer _drawnLine;

    private bool _redrawWaveFlag;

    private int STEPCOUNT = 200;
    private float MAX_X;

    void Awake()
    {
        
        _doc = GetComponent<UIDocument>();
        _error_label = new Label("Error: 0%");
        _doc.rootVisualElement.Add(_error_label);
    }

    // Start is called before the first frame update
    void Start()
    {
        _height = Camera.main.orthographicSize;
        _width = _height * Camera.main.aspect;

        _height = _height - 0.5f;
        _width = _width - 0.5f;
        _xscale = 1.1f / (2 * _width);
        _yscale = 0.2f;

        _last_x = -1;

        _conv = GetComponent<Convolution>();
        _drawnPoints = new float[_conv.STEPCOUNT * 2];
        for (int i = 0; i < _conv.STEPCOUNT * 2; i++)
        {
            _drawnPoints[i] = 5.0f;
        }

        _drawnObject = new GameObject("Drawn Line");
        _drawnObject.transform.SetParent(transform, false);
        _drawnLine = _drawnObject.AddComponent<LineRenderer>();
        drawWave(_drawnObject);
        _drawnObject.transform.localScale = new Vector3(0.5f, 0.5f, 0f);
        _drawnObject.transform.position = new Vector3(30f, 30f, 0f);

        _redrawWaveFlag = true;
        redrawGraphs();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        if (_redrawWaveFlag)
        {
            redrawGraphs();
            _redrawWaveFlag = false;
        }

    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            int xmin = 68, xmax = 108, ymin = 47, ymax = 131;
            Vector2 mousePos = Vector2.zero;
            ScreenToWorld(ref mousePos);
            if (mousePos.x <= xmax && mousePos.x >= xmin && mousePos.y <= ymax && mousePos.y >= ymin)
            {
                int index = Mathf.RoundToInt(map(mousePos.x, xmin, xmax, 0, _conv.STEPCOUNT));
                float graphmax = 5.0f; //TEMP, TODO: Change
                float new_y = map(mousePos.y, ymin, ymax, 0, graphmax);
                _drawnPoints[index] = new_y;
                if (_last_x >= 0)
                {
                    if (_last_x < index)
                    {
                        for (int i = _last_x + 1; i < index; i++) { } //TODO
                    }
                    else
                    {

                        for (int i = index + 1; i < _last_x; i++) { } //TODO
                    }
                }
                _last_x = index;
                Debug.Log(mousePos);
                _redrawWaveFlag = true;
            }
            //_redrawWaveFlag = true;
        }
    }

    void ScreenToWorld(ref Vector2 pos)
    {
        pos.x = (Input.mousePosition.x - _width) * _xscale;
        pos.y = (Input.mousePosition.y - _height) * _yscale;
    }

    Vector2 ToScreenCoords(Vector2 funccoords)
    {
        return (new Vector3(-_width + funccoords.x / _xscale,
                            funccoords.y / _yscale));
    }

    float map(float val, float in_min, float in_max, float out_min, float out_max)
    {
        return Mathf.Lerp(out_min, out_max, Mathf.InverseLerp(in_min, in_max, val));
    }

    public void redrawGraphs()
    {
        // Draw graph of wave A
        _drawnObject.AddComponent<LineRenderer>();
        drawWave(_drawnObject);
    }

    public void drawWave(GameObject lineObj)
    {
        if (lineObj.GetComponent<MeshRenderer>() != null)
        {
            Destroy(lineObj.GetComponent<MeshRenderer>());
            Destroy(lineObj.GetComponent<MeshFilter>());
        }

        //Color color = Color.grey + Color.blue;
        Color color = Color.green;

        var lineRenderer = lineObj.GetComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        lineRenderer.useWorldSpace = true;
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        // The scaled value for which to increment the functions x value
        float incrementValue = _conv.MAX_X / _conv.STEPCOUNT;

        // A list to stor our new scaled xvalues
        var xList = new List<float>(STEPCOUNT);

        // A list of Vecter2s to store both the xy points we want linerenderer to connect
        List<Vector2> pointsList = new List<Vector2>();
        lineRenderer.positionCount = STEPCOUNT;
        for (int i = 0; i < STEPCOUNT; ++i)
        {
            pointsList.Add(new Vector3(i, _drawnPoints[i], 0.0f));
        }

        for (int i = 0; i < pointsList.Count; i++)
        {
            lineRenderer.SetPosition(i, pointsList[i]);
        }
        BakeLineDebuger(lineObj);
    }

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
