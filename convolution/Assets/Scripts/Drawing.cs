using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Drawing : MonoBehaviour
{
   

    private float _width, _height, _xscale, _yscale, xymargin;

    private int _last_x;

    private UIDocument _doc;
    private Label _error_label;


    public GameObject _linePrefab;
    public GameObject _drawnObject;


    public LineRenderer _drawnLine;

    public List<Vector2> mousePos;

    private bool _redrawWaveFlag;

   

    void Awake()
    {
        
        /// @Zellenon this is causing issues on awake...
        // _doc = GetComponent<UIDocument>();
        // _error_label = new Label("Error: 0%");
        // _doc.rootVisualElement.Add(_error_label);

        _drawnObject = Instantiate(_linePrefab, Vector3.zero, Quaternion.identity);
        
        

        _drawnLine = _drawnObject.GetComponent<LineRenderer>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        _height = Camera.main.orthographicSize;
        _width = _height * Camera.main.aspect;

        _height = _height - 0.5f;
        _width = _width - 0.5f;
        _xscale = 1.1f / (2 * _width);
        _yscale = 0.2f;

        _last_x = -1;

        
        
    

        
    }

    // Update is called once per frame
    void Update()
    {

        int xmin = 41, xmax = 64, ymin = 17, ymax = 41;

        // Get the mouse position
        Vector2 tempPos = Vector2.zero;
        ScreenToWorld(ref tempPos);



        if (tempPos.x <= xmax && tempPos.x >= xmin && tempPos.y <= ymax && tempPos.y >= ymin)
            {
            if (Input.GetMouseButtonDown(0) ) // If mouse initially clicked
            {
                

            CreateLine();

                
            }
            if (Input.GetMouseButton(0) ) // If mouse is being held add more points
            {
            
                Vector2 tempMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (Vector2.Distance(tempMousePos, mousePos[mousePos.Count - 1]) > .1f){
                    UpdateLine(tempMousePos);
                }
                
            }
        }
    }
    void ScreenToWorld(ref Vector2 pos)
    {
        pos.x = 100f * (Input.mousePosition.x) / Screen.width;
        pos.y = 100f * (Input.mousePosition.y) / Screen.height;
    }

    void HandleInput()
    {
        // if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) // If we should actually do something
        // {
        //     int xmin = 41, xmax = 64, ymin = 17, ymax = 41;

        //     // Get the mouse position
        //     Vector2 mousePos = Vector2.zero;
        //     ScreenToWorld(ref mousePos);

        //     // If the user is clicking on the graph area
        //     if (mousePos.x <= xmax && mousePos.x >= xmin && mousePos.y <= ymax && mousePos.y >= ymin)
        //     {
        //         // Which point on the graph are they changing?
        //         int index = Mathf.RoundToInt(map(mousePos.x, xmin, xmax, 0, _conv.STEPCOUNT));
        //         // graphmax should be the max value of the convolution - the max value of waveC
        //         float graphmax = 5.0f; //TEMP, TODO: Change
        //         // the value we should set that point to
        //         float new_y = map(mousePos.y, ymin, ymax, 0, graphmax);
        //         // actually change the value
        //         _drawnPoints[index] = new_y;

        //         // Ignore this for now
        //         if (_last_x >= 0)
        //         {
        //             if (_last_x < index)
        //             {
        //                 for (int i = _last_x + 1; i < index; i++) { } //TODO
        //             }
        //             else
        //             {

        //                 for (int i = index + 1; i < _last_x; i++) { } //TODO
        //             }
        //         }
        //         _last_x = index;
        //         _redrawWaveFlag = true;
        //     }


        //     //_redrawWaveFlag = true;
        // }

        
        

    }

   

 
 
    private void CreateLine(){

        
        Destroy(GameObject.Find("Line(Clone)"));
           
        



        _drawnObject = Instantiate(_linePrefab, Vector3.zero, Quaternion.identity);
        
        

        _drawnLine = _drawnObject.GetComponent<LineRenderer>();

        mousePos.Clear();
        mousePos.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        mousePos.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        _drawnLine.SetPosition(0, mousePos[0]);
        _drawnLine.SetPosition(1, mousePos[1]);
    }

    private void UpdateLine(Vector2 newMousePos){
        mousePos.Add(newMousePos);
        _drawnLine.positionCount++;
        _drawnLine.SetPosition(_drawnLine.positionCount -1, newMousePos);
    }

    
}
