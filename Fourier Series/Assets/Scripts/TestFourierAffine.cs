using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestFourierAffine : MonoBehaviour
{
    [SerializeField] private PlotObj _plotObj, _errPlotObj;
    [SerializeField] private Color _plotColor, _errorColor;
    [SerializeField] private TextMeshPro _eqnText, _errorValue;
    [SerializeField] private TMP_Dropdown _coeffDropdown;
    [SerializeField] private Slider _coeffSlider;
    [SerializeField] private CustomAffine _refFunc;
    private int _npoints, _npoints_T0;
    private int _nharmonics;
    private float _T0;   // time period of fundamental frequency
    private float _maxval, _maxerr, _rmserr;  // need this to determine correct scaling
    public Vector3[] _xypoints, _xyerr;

    public float a0;  // DC coeff
    public float[] ak, bk; // fundamentals and harmonics


    private bool isStale = false;


    void OnDisable()
    {
        isStale = true;
    }


    void OnEnable()
    {
        if (isStale)
        {
            CoeffsChanged();
            isStale = false;
        }
    }





    // Start is called before the first frame update
    void Start()
    {
        _T0 = 1;    // period in time units
        _npoints_T0 = 1000;  // points per period
        _nharmonics = 6;
        float deltax = _T0 / _npoints_T0;
        _npoints = 2 * _npoints_T0;  // total interval = two periods of the signal
        _xypoints = new Vector3[_npoints+1];
        _xyerr = new Vector3[_npoints+1];

        // initialize to zero
        float xval = 0;
        for (int np = 0; np <= _npoints; np++)
        {
            _xypoints[np].z = 0;
            _xypoints[np].y = 0;
            _xypoints[np].x = xval;
            xval += deltax;
        }

        a0 = 0;
        ak = new float[_nharmonics];
        bk = new float[_nharmonics];

        _eqnText.color = _plotColor;
        _eqnText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, _plotObj.GetTopPos() + 2f);
        UpdateEquationString();


        // plotobj should already have a ref function setting the axis scales etc.
        // make sure this function matches parameters of the ref function
        _plotObj.AddPlotLineNoRescale(_xypoints, _plotColor);

        CalculateErrorFunction();
        _errPlotObj.SetScaleXAxis(1.2f * 2 * _T0 / _errPlotObj._width);
        float errscale = 3f * _maxerr / _errPlotObj._height;
        if (errscale < 0.1f)
            errscale = 0.1f;

        _errPlotObj.SetScaleYAxis(errscale);
        _errPlotObj.AddPlotLineNoRescale(_xyerr, _errorColor);
        _errorValue.color = _errorColor;
        _errorValue.text = String.Format("RMS Error: {0:f4}", _rmserr);


        // setup UI elements
        for (int k=0; k<_nharmonics; k++)
        {
            List<String> optstr = new List<String>();
            optstr.Add(String.Format("a<sub>{0}</sub>", k+1));
            optstr.Add(String.Format("b<sub>{0}</sub>", k+1));
            _coeffDropdown.AddOptions(optstr);
        }

    }


    // Update is called once per frame
    void Update()
    {        
    }


    public void UpdateEquationString()
    {
        int selectedCoeff = _coeffDropdown.value;

        String eqnstr;
        if (selectedCoeff == 0)
            eqnstr = String.Format("<size=+4><b>{0:f2}</b></size>", a0);
        else
            eqnstr = String.Format("{0:f2}", a0);


        for (int k=0; k<_nharmonics; k++)
        {
            String akstr;
            float coeff = ak[k];
            if (coeff < 0)
            {
                akstr = " - ";
                coeff = -coeff;
            }
            else
                akstr = " + ";

            if (selectedCoeff == (2*k+1) )
                akstr += String.Format("<size=+4><b>{0:f2}</b></size> cos({1}\u03c0f<sub>0</sub>t)", coeff, 2*(k+1));
            else
                akstr += String.Format("{0:f2} cos({1}\u03c0f<sub>0</sub>t)", coeff, 2*(k+1));
            eqnstr += akstr;

            String bkstr;
            coeff = bk[k];
            if (coeff < 0)
            {
                bkstr = " - ";
                coeff = -coeff;
            }
            else
                bkstr = " + ";

            if (selectedCoeff == (2*k+2))
                bkstr += String.Format("<size=+4><b>{0:f2}</b></size> sin({1}\u03c0f<sub>0</sub>t)", coeff, 2*(k+1));
            else
                bkstr += String.Format("{0:f2} sin({1}\u03c0f<sub>0</sub>t)", coeff, 2*(k+1));
            eqnstr += bkstr;
        }

        _eqnText.text = eqnstr;
    }


    public void SetSliderToCoeff()
    {
        int selectedCoeff = _coeffDropdown.value;
        if (selectedCoeff == 0)
            _coeffSlider.value = a0;
        else if ( (selectedCoeff % 2) == 1)
            _coeffSlider.value = ak[ (selectedCoeff - 1) / 2 ];
        else
            _coeffSlider.value = bk[ selectedCoeff / 2 - 1 ];

    }


    public void CoeffsChanged()
    {
        int selectedCoeff = _coeffDropdown.value;
        if (selectedCoeff == 0)
            a0 = _coeffSlider.value;
        else if ( (selectedCoeff % 2) == 1)
            ak[ (selectedCoeff - 1) / 2 ] = _coeffSlider.value;
        else
            bk[ selectedCoeff / 2 - 1 ] =  _coeffSlider.value;

        UpdateEquationString();
        UpdatePlotValues();

        CalculateErrorFunction();
        float errscale = 3f * _maxerr / _errPlotObj._height;
        if (errscale < 0.1f)
            errscale = 0.1f;

        _errPlotObj.SetScaleYAxis(errscale);
        _errPlotObj.UpdatePlot(_xyerr);
        _errorValue.text = String.Format("RMS Error: {0:f2} %", 100f * _rmserr);
    }

    public void UpdatePlotValues()
    {
        const float twoPI = 2 * Mathf.PI;
        float f0 = 1/_T0;
        float deltax = 2f * _T0 / _npoints;


        float xval = 0;
        for (int np = 0; np <= _npoints; np++)
        {
            float yval = a0;
            _xypoints[np].z = 0;
            _xypoints[np].x = xval;

            for (int k=0; k<_nharmonics; k++)
            {
                yval += ak[k] * Mathf.Cos(twoPI * f0 * (k+1) * xval );
                yval += bk[k] * Mathf.Sin(twoPI * f0 * (k+1) * xval ); 
            }
            _xypoints[np].y = yval;
            xval += deltax;
        }
        _plotObj.UpdatePlot(_xypoints);

    }


    void CalculateErrorFunction()
    {
        _maxerr = 0;
        float sumSqErr = 0;
        for (int np = 0; np <= _npoints; np++)
        {
            _xyerr[np].z = _xypoints[np].z;
            _xyerr[np].x = _xypoints[np].x;
            float err = _refFunc._xypoints[np].y - _xypoints[np].y;

            sumSqErr += err * err;
            _xyerr[np].y = err;

            if ( Mathf.Abs(err) > _maxerr)
                _maxerr = Mathf.Abs(err);
        }

        _rmserr = Mathf.Sqrt(sumSqErr / _npoints);
    }


}
