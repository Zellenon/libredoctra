using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlotObj : MonoBehaviour
{
    public float _x0, _y0, _width, _height;   // center and size of the plot region
    private float _gridspacex, _gridspacey;
    private float _xscale, _yscale;
    private GameObject[] _gridlinesx, _gridlinesy;

    private LineRenderer _dynamicLR;
    [SerializeField] private Color _axisColor;
    [SerializeField] Material defaultLineMat;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public float GetTopPos()
    {
        return (_y0 + _height / 2);
    }


    public void InitializePlot(float x0, float y0, float width, float height, float gridspacex, float gridspacey, float xscale, float yscale, Color axisColor, Material lineMaterial, TextMeshPro labelTextTemplate)
    {
        // defacto constructor; call this before doing anything else
        _x0 = x0; _y0 = y0;
        _width = width; _height = height;
        _gridspacex = gridspacex; _gridspacey = gridspacey;
        _xscale = xscale; _yscale = yscale;
        _axisColor = axisColor;

        // empty parent object to hold grid lines
        String gridObjName = transform.name + " grid";
        GameObject gridlines = new GameObject(gridObjName);
        gridlines.transform.SetParent(transform, false);

        GameObject lrContainer = new GameObject("template obj with a line component");
        lrContainer.transform.SetParent(gridlines.transform, false);
        lrContainer.AddComponent<LineRenderer>();

        int numLines = (int)Math.Floor(width / gridspacex);
        _gridlinesx = new GameObject[numLines + 1];

        TextMeshPro Origiiabel = Instantiate(labelTextTemplate);
        Origiiabel.transform.name = "Origin";
        Origiiabel.transform.SetParent(transform, false);
        //
        // place origin label
        Origiiabel.GetComponent<RectTransform>().anchoredPosition = new Vector2(x0 + 0.3f, y0 - 0.3f);

        // Drawing grid lines
        for (int i = 0; i <= numLines; i++)
        {
            float xval = i * gridspacex * xscale;
            float xpos = x0 + i * gridspacex;
            String xlabel = String.Format("x={0:f1}", xval); // i.ToString();
            GameObject newGridLine = Instantiate(lrContainer);
            newGridLine.transform.SetParent(gridlines.transform, false);
            newGridLine.transform.name = xlabel;

            LineRenderer xline = newGridLine.GetComponent<LineRenderer>();
            xline.material = lineMaterial;
            xline.useWorldSpace = true;

            if (i == 0)
            {
                xline.startWidth = 0.05f;
                xline.endWidth = 0.05f;
            }
            else
            {
                TextMeshPro newLabel = Instantiate(labelTextTemplate);
                newLabel.transform.SetParent(newGridLine.transform, false);
                newLabel.text = String.Format("{0:f1}", xval);
                newLabel.GetComponent<RectTransform>().anchoredPosition = new Vector2(xpos + 0.3f, y0 - 0.3f);

                xline.startWidth = 0.01f;
                xline.endWidth = 0.01f;
            }

            xline.startColor = axisColor;
            xline.endColor = axisColor;

            Vector3 pos = new Vector3(xpos, y0 + height / 2, 0);
            xline.SetPosition(0, pos);
            pos = new Vector3(xpos, y0 - height / 2, 0);
            xline.SetPosition(1, pos);

            _gridlinesx[i] = newGridLine;
        }

        numLines = (int)Math.Floor(height / (2 * gridspacey));
        _gridlinesy = new GameObject[2 * numLines + 1];
        for (int i = -numLines; i <= numLines; i++)
        {
            float yval = i * gridspacey * yscale;
            float ypos = y0 + i * gridspacey;
            String ylabel = String.Format("y={0}", yval);
            GameObject newGridLine = Instantiate(lrContainer);
            newGridLine.transform.SetParent(gridlines.transform, false);
            newGridLine.transform.name = ylabel;

            LineRenderer yline = newGridLine.GetComponent<LineRenderer>();
            yline.material = lineMaterial;
            yline.useWorldSpace = true;

            if (i == 0)
            {
                yline.startWidth = 0.05f;
                yline.endWidth = 0.05f;
            }
            else
            {
                TextMeshPro newLabel = Instantiate(labelTextTemplate);
                newLabel.transform.SetParent(newGridLine.transform, false);
                newLabel.text = String.Format("{0:f1}", yval);
                newLabel.GetComponent<RectTransform>().anchoredPosition = new Vector2(x0 - 0.3f, ypos + 0.3f);

                yline.startWidth = 0.01f;
                yline.endWidth = 0.01f;
            }

            yline.startColor = axisColor;
            yline.endColor = axisColor;

            Vector3 pos = new Vector3(x0, ypos, 0);
            yline.SetPosition(0, pos);
            pos = new Vector3(x0 + width, ypos, 0);
            yline.SetPosition(1, pos);

            _gridlinesy[numLines + i] = newGridLine;
        }

        Destroy(lrContainer);
    }



    public void SetScaleXAxis(float xscale)
    {
        _xscale = xscale;
        int numLines = (int)Math.Floor(_width / _gridspacex);

        for (int i = 1; i <= numLines; i++)
        {
            float xval = i * _gridspacex * _xscale;
            String xlabel = String.Format("x={0:f1}", xval);
            GameObject gridlineObj = _gridlinesx[i];
            gridlineObj.transform.name = xlabel;

            TextMeshPro newLabel = gridlineObj.transform.GetChild(0).GetComponent<TextMeshPro>();
            newLabel.text = String.Format("{0:f1}", xval);
        }
        _xscale = xscale;
    }


    public void SetScaleYAxis(float yscale)
    {
        _yscale = yscale;
        int numLines = (int)Math.Floor(_height / (2 * _gridspacey));
        for (int i = -numLines; i <= numLines; i++)
        {
            float yval = i * _gridspacey * _yscale;
            String ylabel = String.Format("y={0}", yval);
            GameObject newGridLine = _gridlinesy[numLines + i];
            newGridLine.transform.name = ylabel;

            LineRenderer yline = newGridLine.GetComponent<LineRenderer>();

            if (i != 0)
            {
                TextMeshPro newLabel = newGridLine.transform.GetChild(0).GetComponent<TextMeshPro>();
                newLabel.text = String.Format("{0:f1}", yval);
            }
        }
    }


    public void AddPlotLine(Vector2[] xypoints, float xrange, float yrange, Color plotColor)
    {
        Vector3[] scaledxypoints = new Vector3[xypoints.Length];
        GameObject newPlotLine = new GameObject("Ref Plot");
        newPlotLine.transform.SetParent(transform, false);

        LineRenderer plotlr = newPlotLine.AddComponent<LineRenderer>();
        plotlr.material = defaultLineMat;
        plotlr.useWorldSpace = true;
        plotlr.startWidth = 0.2f;
        plotlr.endWidth = 0.2f;
        plotlr.startColor = plotColor;
        plotlr.endColor = plotColor;

        float newxscale = xrange / _width;
        float newyscale = 1.2f * (2 * yrange) / _height;

        for (int np = 0; np < xypoints.Length; np++)
        {
            scaledxypoints[np].x = _x0 + xypoints[np].x / newxscale;
            scaledxypoints[np].y = _y0 + xypoints[np].y / newyscale;
            scaledxypoints[np].z = 0;
        }

        SetScaleXAxis(newxscale);
        SetScaleYAxis(newyscale);

        plotlr.positionCount = scaledxypoints.Length;
        plotlr.SetPositions(scaledxypoints);
    }


    public void AddPlotLineNoRescale(Vector3[] xypoints, Color plotColor)
    {
        Vector3[] scaledxypoints = new Vector3[xypoints.Length];
        GameObject newPlotLine = new GameObject("New Dynamic Plot");
        newPlotLine.transform.SetParent(transform, false);

        _dynamicLR = newPlotLine.AddComponent<LineRenderer>();
        _dynamicLR.material = defaultLineMat;
        _dynamicLR.useWorldSpace = true;
        _dynamicLR.startWidth = 0.2f;
        _dynamicLR.endWidth = 0.2f;
        _dynamicLR.startColor = plotColor;
        _dynamicLR.endColor = plotColor;

        for (int np = 0; np < xypoints.Length; np++)
        {
            scaledxypoints[np].x = _x0 + xypoints[np].x / _xscale;
            scaledxypoints[np].y = _y0 + xypoints[np].y / _yscale;
            scaledxypoints[np].z = 0;
        }

        _dynamicLR.positionCount = scaledxypoints.Length;
        _dynamicLR.SetPositions(scaledxypoints);
    }


    public void UpdatePlot(Vector3[] xypoints)
    {
        Vector3[] scaledxypoints = new Vector3[xypoints.Length];

        for (int np = 0; np < xypoints.Length; np++)
        {
            scaledxypoints[np].x = _x0 + xypoints[np].x / _xscale;
            scaledxypoints[np].y = _y0 + xypoints[np].y / _yscale;
            scaledxypoints[np].z = 0;
        }

        _dynamicLR.positionCount = scaledxypoints.Length;
        _dynamicLR.SetPositions(scaledxypoints);
    }



}
