using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAffine : MonoBehaviour
{
    [SerializeField] private PlotObj _plotObj;
    [SerializeField] private Color _plotColor;
    private int _npoints, _npoints_T0;
    private float _T0;   // time period of fundamental frequency
    private float _maxval;  // need this to determine correct scaling
    public Vector2[] _xypoints;

    [SerializeField] private CustomFuncSelector _funcSelector;
    public Vector2[] anchorpts, interparray;


    void Awake()
    {
        _T0 = 1;    // period in time units
        _npoints_T0 = 1000;  // points per period

        float deltax = _T0 / _npoints_T0;
        _npoints = 2 * _npoints_T0;  // total interval = two periods of the signal
        _xypoints = new Vector2[_npoints+1];

        // initialize to zero
        float xval = 0;
        for (int np = 0; np <= _npoints; np++)
        {
            _xypoints[np].x = xval;
            xval += deltax;
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        //_funcSelector.gameObject.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {        
    }


    public void FinishedCustomizing()
    {
        int nAnchorpts = _funcSelector._anchorpts.Length;
        anchorpts = new Vector2[nAnchorpts];
        anchorpts = _funcSelector._anchorpts;
        CreateCustomXY();

        _funcSelector.gameObject.SetActive(false);
        foreach (GameObject gobj in _funcSelector.gameObjectArray)
            gobj.SetActive(true);

        _plotObj.AddPlotLine(_xypoints, 2*_T0, _maxval, _plotColor);    // rescale, recenter and plot
    }




    void CreateCustomXY()
    {
        interparray = new Vector2[_npoints_T0];
        float deltax = _T0 / _npoints_T0;

        float xval = 0;
        for (int k=0; k<interparray.Length; k++)
        {
            interparray[k].x = xval;
            xval += deltax;            
        }
        
        CreateInterpolatedFunction(interparray, anchorpts);
        _maxval = 0;
        for (int np = 0; np <= _npoints; np++)
        {
            int modi = (np % interparray.Length);
            float yval = interparray[modi].y;

            if (Mathf.Abs(yval) > _maxval)
                _maxval = Mathf.Abs(yval);

            _xypoints[np].y = yval;
        }

    }


    public void CreateInterpolatedFunction(Vector2[] xypoints, Vector2[] anchorpoints)
    {
        // the vector xypoints should already contain x coordinates
        // this function will fill in the corresponding y coordinates by linearly
        // interpolating the two surrounding vectors in anchorpoints

        int lindex, rindex;
        float lxvalue, rxvalue, lyvalue, ryvalue;

        rindex = 1;

        for (int k=0; k<xypoints.Length; k++)
        {
            while (anchorpoints[rindex].x < xypoints[k].x)
            {
                if (rindex == anchorpoints.Length - 1)
                    break;
                rindex++;
            }
            lindex = rindex - 1;
            lxvalue = anchorpoints[lindex].x;
            rxvalue = anchorpoints[rindex].x;
            lyvalue = anchorpoints[lindex].y;
            ryvalue = anchorpoints[rindex].y;

            float t = (xypoints[k].x - lxvalue)/(rxvalue - lxvalue);
            xypoints[k].y = Mathf.Lerp(lyvalue, ryvalue, t);
        }
    }



}
