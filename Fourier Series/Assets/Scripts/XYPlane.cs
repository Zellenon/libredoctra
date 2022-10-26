using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class XYPlane : MonoBehaviour
{
    [SerializeField] private Material lineMaterial;
    [SerializeField] private TextMeshPro labelTextTemplate;
    [SerializeField] private PlotObj _leftplot, _rightplot;

    private float x01, y01, x02, y02, width, height;   // center and size of the plot region(s)
    private float _gridspacex, _gridspacey;
    private float _xscale, _yscale;
    private Color _mygrey;


    void Awake()
    {
        float ymax = Camera.main.orthographicSize;
        float xmax = ymax * Screen.width / Screen.height;

        String msg = String.Format("xmax = {0:f1}, ymax = {1:f1}", xmax, ymax);
        Debug.Log(msg);

        // we will ue the top 1/3 of the screen for UI
        // bottom 2/3 of the screen will be split into
        // two columns each showing a plot
        float xymargin = 2f;

        _mygrey = new Color32(183, 183, 183, 100);

        // center of bottom left column
        x01 = -xmax + xymargin;
        y01 = -ymax/3f;

        // center of bottom right column
        x02 = xymargin;
        y02 = -ymax/3f;

        width = (xmax - 1.5f * xymargin);   // width of each bottom column
        height = (4f/3f * ymax - xymargin);

        _gridspacex = 2f;
        _gridspacey = 2f;
        _xscale = 0.2f;
        _yscale = 0.5f;

        msg = String.Format("Width = {0:f1}, Height = {1:f1}", width, height);
        Debug.Log(msg);

        _leftplot.InitializePlot(x01, y01, width, height, _gridspacex, _gridspacey, _xscale, _yscale, _mygrey, lineMaterial, labelTextTemplate);
        _rightplot.InitializePlot(x02, y02, width, height, _gridspacex, _gridspacey, _xscale, _yscale, _mygrey, lineMaterial, labelTextTemplate);

        // labelTextTemplate.gameObject.SetActive(false);

        _rightplot.SetScaleXAxis(2);
    }


    // Start is called before the first frame update
    void Start()
    {        
    }

    // Update is called once per frame
    void Update()
    {        
    }



}
