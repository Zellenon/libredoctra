using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

using TMPro;

public class wave : MonoBehaviour
{
    public float _x0, _y0, _width, _height;   // center and size of the plot region
    
    private float _xscale, _yscale;
    
    // Defined as a list so we can create line shapes with different points
    private List<Vector3> points = new List<Vector3>();
    private GameObject topPlot;

    [SerializeField] LineRenderer lr;
    [SerializeField] private Color _axisColor;
    [SerializeField] Material defaultLineMat;
    

    void Awake(){
        lr = GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.widthMultiplier = 0.2f;
        
        
    }

    public void CreateGrid(float x0, float y0, float width, float height, Color axisColor, Material lineMaterial){
        
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
