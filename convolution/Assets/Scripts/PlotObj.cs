using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlotObj : MonoBehaviour
{
    public float _x0, _y0, _width, _height;   // center and size of the plot region

    [SerializeField] float _xMax;
    [SerializeField] float _yMax;
    [SerializeField] float _xMin;
    [SerializeField] float _yMin;
    [SerializeField] LineRenderer lr;
    [SerializeField] private Color _axisColor;
    [SerializeField] Material defaultLineMat;

    private GameObject _yAxisObject, _xAxisObject, _lineObject;

    private List<Vector3> points = new List<Vector3>();

    private int STEPCOUNT = 200;

    private float _xscale, _yscale;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.widthMultiplier = 0.1f;

        _height = Camera.main.orthographicSize;
        _width = _height * Camera.main.aspect;

        _height = _height - 0.5f;
        _width = _width - 0.5f;
        _xscale = 1.1f / (2 * _width);
        _yscale = 0.2f;

        Debug.LogFormat("_width: {0}, _height: {1}", _width, _height);

        var xymargin = 0.5f;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public void CreateGrid(float x0, float y0, float width, float height, Color axisColor, Material lineMaterial)
    {
        //create gridline points z axis determins overlap order with z axis pos going into the screen
        points.Add(new Vector3(((x0 - width)), (y0), 1.0f));
        points.Add(new Vector3((x0), (y0), 1.0f));
        points.Add(new Vector3((x0), (y0 + (height)), 1.0f));
        points.Add(new Vector3((x0), (y0), 1.0f));
        points.Add(new Vector3(((x0 + width)), (y0), 1.0f));

        // Tell it to make it default line material
        lr.material = lineMaterial;

        // For whatever reason, Unity requires user to define a gradient instead of just telling it a solid color >:( .... so we create a gradient from one color to another
        //      color (otherwise known as a solid color)
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(axisColor, 0.0f), new GradientColorKey(axisColor, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        this.lr.colorGradient = gradient;

        lr.positionCount = points.Count;
    }

    //Create a grid with the shape L instead of upsidedownT shape
    public void CreateLGrid(float x0, float y0, float width, float height, Color axisColor, Material lineMaterial)
    {
        //create gridline points z axis determins overlap order with z axis pos going into the screen
        points.Add(new Vector3((x0), (y0), 1.0f));
        points.Add(new Vector3((x0), (y0 + (height)), 1.0f));
        points.Add(new Vector3((x0), (y0), 1.0f));
        points.Add(new Vector3(((x0 + width)), (y0), 1.0f));

        // Tell it to make it default line material
        lr.material = lineMaterial;

        // For whatever reason, Unity requires user to define a gradient instead of just telling it a solid color >:( .... so we create a gradient from one color to another
        //      color (otherwise known as a solid color)
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(axisColor, 0.0f), new GradientColorKey(axisColor, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        this.lr.colorGradient = gradient;

        lr.positionCount = points.Count;
    }

    //not used yet
    public void CreateDottedLine(float x0, float y0, float x1, float y1, Color axisColor, Material lineMaterial)
    {
        // points.Add(new Vector3(((x0-width)),(y0),0.0f));
        // points.Add(new Vector3((x0),(y0),0.0f));
        // lr.positionCount = points.Count;
        // lr.material = new Material(Shader.Find("Sprites/Default"));
        //this.Update();
    }

    // Update is called once per frame
    public void Update()
    {
        // loop over the points to make a grid of points to connect the line to
        for (int i = 0; i < points.Count; i++)
        {
            lr.SetPosition(i, points[i]);
        }
    }
}
