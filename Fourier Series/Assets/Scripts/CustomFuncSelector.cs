using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomFuncSelector : MonoBehaviour
{
    [SerializeField] private PlotObj _plotObj;
    [SerializeField] private Material lineMaterial;
    [SerializeField] private TextMeshPro labelTextTemplate;

    private float _width, _height;
    private float _xscale, _yscale;

    private Color _axisColor;

    private int _nAnchorObjs;
    private GameObject[] _anchorObjs;
    public Vector2[] _anchorpts;
    private LineRenderer _funclr;
    [SerializeField] private GameObject _anchorObjTemplate;

    public GameObject[] gameObjectArray;


    // Start is called before the first frame update
    void Start()
    {
        float ymax = Camera.main.orthographicSize;
        float xmax = ymax * Screen.width / Screen.height;

        _width =  0.9f * xmax;
        _height = 0.9f * ymax;

        _axisColor = new Color32(183, 183, 183, 100);

        gameObjectArray = GameObject.FindGameObjectsWithTag ("HideOnInit");
        foreach(GameObject gobj in gameObjectArray)
            gobj.SetActive(false);
       
        _xscale = 1.1f/(2*_width);
        _yscale = 0.2f;

        _plotObj.InitializePlot( -_width, 0, 2*_width, 2*_height, 2f, 1f, _xscale, _yscale, _axisColor, lineMaterial, labelTextTemplate);

        GameObject lineContainer = new GameObject("Custom Function");
        lineContainer.transform.SetParent(transform, false);

        Color plotColor = Color.red;
        _funclr = lineContainer.AddComponent<LineRenderer>();
        _funclr.material = lineMaterial;
        _funclr.useWorldSpace = true;
        _funclr.startWidth = 0.2f;
        _funclr.endWidth = 0.2f;
        _funclr.startColor = plotColor;
        _funclr.endColor = plotColor;

        _nAnchorObjs = 5;
        _anchorObjs = new GameObject[_nAnchorObjs];
        _funclr.positionCount = _nAnchorObjs;  // need at least 2
        _anchorpts = new Vector2[_nAnchorObjs];
/*
        _anchorpts[0] = ToScreenCoords(new Vector3(0,0));
        _anchorpts[1] = ToScreenCoords(new Vector3(0.5f,1));
        _anchorpts[2] = ToScreenCoords(new Vector3(1,-1));
*/
        for (int np=0; np<_nAnchorObjs; np++)
        {
            float xval = Mathf.Lerp(0, 1, np/((float) _nAnchorObjs - 1));

            _anchorObjs[np] = Instantiate(_anchorObjTemplate);
            _anchorObjs[np].name = "Anchor Object - " + np;
            _anchorObjs[np].transform.SetParent(transform, false);
            Vector2 newpos = new Vector2(xval, xval);
            Vector2 screenpos = ToScreenCoords(newpos);
            _anchorObjs[np].transform.position = screenpos;
            _anchorObjs[np].SetActive(true);
            _funclr.SetPosition(np, screenpos);

            _anchorpts[np] = newpos;
        }
    }


    // Update is called once per frame
    void Update()
    {        
    }



    Vector2 ToScreenCoords(Vector2 funccoords)
    {
        return(new Vector3(-_width + funccoords.x / _xscale, funccoords.y / _yscale));
    }

    Vector2 ToFuncCoords(Vector2 screencoords)
    {
        return(new Vector3( _xscale * (_width + screencoords.x), _yscale * screencoords.y));
    }

    public void UpdateAnchorPoints()
    {
        for (int np=0; np<_nAnchorObjs; np++)
        {
            Vector2 screenpos = _anchorObjs[np].transform.position;
            _anchorpts[np] = ToFuncCoords(_anchorObjs[np].transform.position);
            
            _funclr.SetPosition(np, screenpos);
        }
    }


}

