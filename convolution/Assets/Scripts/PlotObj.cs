using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlotObj : MonoBehaviour
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
        
    }
    

    // Start is called before the first frame update
    void Start()
    {
        //
         
    }


    public void CreateGrid(float x0, float y0, float width, float height, Color axisColor, Material lineMaterial){
        
        points.Add(new Vector3(((x0-width)),(y0),0.0f));
        points.Add(new Vector3((x0),(y0),0.0f));
        points.Add(new Vector3((x0),(y0+(height)),0.0f));
        points.Add(new Vector3((x0),(y0),0.0f));
        points.Add(new Vector3(((x0+width)),(y0),0.0f));

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
    public void CreateDottedLine(float x0, float y0, float x1, float y1, Color axisColor, Material lineMaterial){
        

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
        for (int i = 0; i < points.Count; i++){
            lr.SetPosition(i, points[i]);
        }

    }

}
