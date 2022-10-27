using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EquationText : MonoBehaviour
{
    private TextMeshPro textMesh;
    private float mag, phase, freq;


    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }


    public void updateEquationText(float magval, float freqval, float phaseval)
    {
        mag = magval;
        freq = freqval;
        phase = phaseval;

        String eqn = String.Format("{0:f2} cos(2\u03C0({1:f2}) t + {2:f0}\u00B0)", mag, freq, phase);
        textMesh.text = eqn;
    }

    // Update is called once per frame
    void Update()
    {        
    }



}
