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


    public void CreateGrids(float x0, float y0, float width, float height, float gridspacex, float gridspacey, float xscale, float yscale, Color axisColor, Material lineMaterial, TextMeshPro labelTextTemplate, bool xcentered, bool ycentered)
    {
        // defacto constructor; call this before doing anything else
        _x0 = x0; _y0 = y0;
        _width = width; _height = height;
        _gridspacex = gridspacex; _gridspacey = gridspacey;
        _xscale = xscale; _yscale = yscale;
        _axisColor = axisColor;

        String gridObjName = transform.name + " grid";
        GameObject gridlines = new GameObject(gridObjName); // empty parent object to hold grid lines
        gridlines.transform.SetParent(transform, false);
        GameObject lrContainer = new GameObject("template obj with a line component");
        lrContainer.transform.SetParent(gridlines.transform, false);
        lrContainer.AddComponent<LineRenderer>();

        int nlines = (int) Mathf.Floor(width/gridspacex);
        int nleft;
        if (xcentered)
        {
            _gridlinesx = new GameObject[2 * nlines + 1];
            nleft = -nlines;
        }
        else
        {
            _gridlinesx = new GameObject[nlines + 1];
            nleft = 0;
        }

        TextMeshPro OriginLabel = Instantiate(labelTextTemplate);

        OriginLabel.transform.name = "Origin";
        OriginLabel.transform.SetParent(transform, false);
        // place origin label
        OriginLabel.GetComponent<RectTransform>().anchoredPosition = new Vector2(x0 + 0.2f, y0 - 0.2f);

        for (int nl = nleft; nl <= nlines; nl++)
        {
            float xval = nl * gridspacex * xscale;
            float xpos = x0 + nl * gridspacex;
            String xlabel = String.Format("x={0:f1}", xval);
            GameObject newGridLine = Instantiate(lrContainer);
            newGridLine.transform.SetParent(gridlines.transform, false);
            newGridLine.transform.name = xlabel;

            LineRenderer xline = newGridLine.GetComponent<LineRenderer>();
            xline.material = lineMaterial;
            xline.useWorldSpace = true;
            
            if (nl == 0)
            {
                xline.startWidth = 0.05f;
                xline.endWidth = 0.05f;
            }
            else
            {
                TextMeshPro newLabel = Instantiate(labelTextTemplate);
                newLabel.transform.SetParent(newGridLine.transform, false);
                newLabel.text = String.Format("{0:f1}", xval);
                newLabel.GetComponent<RectTransform>().anchoredPosition = new Vector2( xpos + 0.2f, y0 - 0.2f);

                xline.startWidth = 0.01f;
                xline.endWidth = 0.01f;
            }

            xline.startColor = axisColor;
            xline.endColor = axisColor;

            Vector3 pos = new Vector3(xpos, y0 + height, 0);
            xline.SetPosition(0, pos);

            if (ycentered)
                pos = new Vector3(xpos, y0 - height, 0);
            else
                pos = new Vector3(xpos, y0, 0);

            xline.SetPosition(1, pos);

            _gridlinesx[nl - nleft] = newGridLine;
        }

        nlines = (int) Mathf.Floor(height/ gridspacey);
        int ndown;
        if (ycentered)
        {
            _gridlinesy = new GameObject[2 * nlines + 1];
            ndown = -nlines;
        }
        else
        {
            _gridlinesy = new GameObject[nlines + 1];
            ndown = 0;
        }

        for (int nl = ndown; nl <= nlines; nl++)
        {
            float yval = nl * gridspacey * yscale;
            float ypos = y0 + nl * gridspacey;
            String ylabel = String.Format("y={0}", yval);
            GameObject newGridLine = Instantiate(lrContainer);
            newGridLine.transform.SetParent(gridlines.transform, false);
            newGridLine.transform.name = ylabel;

            LineRenderer yline = newGridLine.GetComponent<LineRenderer>();
            yline.material = lineMaterial;
            yline.useWorldSpace = true;

            if (nl == 0)
            {
                yline.startWidth = 0.05f;
                yline.endWidth = 0.05f;
            }
            else
            {
                TextMeshPro newLabel = Instantiate(labelTextTemplate);
                newLabel.transform.SetParent(newGridLine.transform, false);
                newLabel.text = String.Format("{0:f1}", yval);
                newLabel.GetComponent<RectTransform>().anchoredPosition = new Vector2(x0 - 0.2f, ypos + 0.2f);

                yline.startWidth = 0.01f;
                yline.endWidth = 0.01f;
            }

            yline.startColor = axisColor;
            yline.endColor = axisColor;

            Vector3 pos;
            if (xcentered)
                pos = new Vector3(x0 - width, ypos, 0);
            else
                pos = new Vector3(x0, ypos, 0);

            yline.SetPosition(0, pos);
            pos = new Vector3(x0 + width, ypos, 0);
            yline.SetPosition(1, pos);

            _gridlinesy[nl - ndown] = newGridLine;
        }

        Destroy(lrContainer);
    }



    public void SetScaleXAxis(float xscale, bool xcentered)
    {
        _xscale = xscale;
        int nlines = _gridlinesx.Length;
        int nleft = xcentered ? -nlines/2 : 0;

        for (int nl = nleft; nl < nleft + nlines; nl++)
        {
            float xval = nl * _gridspacex * _xscale;
            String xlabel = String.Format("{0:f1}", xval);
            GameObject gridlineObj = _gridlinesx[nl - nleft];
            gridlineObj.transform.name = xlabel;
            
            if (nl != 0)
            {
                TextMeshPro newLabel = gridlineObj.transform.GetChild(0).GetComponent<TextMeshPro>();
                newLabel.text = xlabel;
            }
        }
    }

 
    public void SetScaleYAxis(float yscale, bool ycentered)
    {
        _yscale = yscale;

        int nlines = _gridlinesy.Length;
        int ndown = ycentered ? -nlines/2 : 0;

        for (int nl = ndown; nl < ndown + nlines; nl++)
        {
            float yval = nl * _gridspacey * _yscale;
            String ylabel = String.Format("{0:f1}", yval);
            GameObject newGridLine = _gridlinesy[nl - ndown];
            newGridLine.transform.name = ylabel;

            // LineRenderer yline = newGridLine.GetComponent<LineRenderer>();

            if (nl != 0)
            {
                TextMeshPro newLabel = newGridLine.transform.GetChild(0).GetComponent<TextMeshPro>();
                newLabel.text = ylabel;
            }
        }
    }


    public Vector2 ToScreenCoords(Vector2 funccoords)
    {
        return(new Vector2(_x0 + funccoords.x / _xscale, _y0 + funccoords.y / _yscale));
    }

    public Vector2 ToFuncCoords(Vector2 screencoords)
    {
        return(new Vector2( (screencoords.x - _x0) * _xscale, (screencoords.y - _y0) * _yscale) );
    }



}
