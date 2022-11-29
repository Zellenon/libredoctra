using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawing : MonoBehaviour
{

    private float _width, _height, _xscale, _yscale, xymargin;

    // Start is called before the first frame update
    void Start()
    {
        _height = Camera.main.orthographicSize;
        _width = _height * Camera.main.aspect;

        _height = _height - 0.5f;
        _width = _width - 0.5f;
        _xscale = 1.1f / (2 * _width);
        _yscale = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
