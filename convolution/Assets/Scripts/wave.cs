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

    
    

    void Awake(){
        
        
    }

    public void CreateGrid(float x0, float y0, float width, float height, Color axisColor, Material lineMaterial){
        
    }

    // public List<Vector2> triangleWave(){
    //     this.points.Clear();
    //     //this.points.add (new Vector3((),(),0.0f));
    // }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
