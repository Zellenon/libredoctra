using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class wave : MonoBehaviour
{
    public float _x0, _y0, _width, _height;   // center and size of the plot region
    
    private float _xscale, _yscale;
    
    // Defined as a list so we can create line shapes with different points
    private List<Vector3> points = new List<Vector3>();


    [SerializeField] LineRenderer lr;
    [SerializeField] private Color _axisColor;
    [SerializeField] Material defaultLineMat;

    void Awake(){
        lr = GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.widthMultiplier = 0.2f;
        ParentComponent c = GetComponentInParent<ParentComponent>();
        if (c != null)
        {
           _x0 = c._x0;
           _y0 = c._y0;
           _width = c._width;
           _height = c._height;
           _xscale = c._xscale;
           _yscale = c._yscale;
        }
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
