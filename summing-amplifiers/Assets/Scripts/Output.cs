using System;
using System.Collections;
using System.Collections.Generic;
using Complex = System.Numerics.Complex;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using MathNet.Numerics.IntegralTransforms;

public class Output : MonoBehaviour
{
    [SerializeField] private Material defaultLineMaterial;
    [SerializeField] private TextMeshPro OriginLabel;
    [SerializeField] private PlotObj topPlot, bottomPlot;
    [SerializeField] private EquationText eqnText;
    private float x01, y01, w1, h1, x02, y02, w2, h2;

    private int Nfft, Nfreq;
    private float _Fs, _tmax;
    private Vector2[] _funcpts, _fftpts;

    private LineRenderer _funclr, _fftlr;

    private GameObject wave1;
    private float amp1=1;
    private float freq1=1;
 /*
    //amplitute and freq of sinewave2
    private GameObject wave2;
    private float amp2=1;
    private float freq2=1; */

     private GameObject wave;
    private float amp=1;
    private float freqAdd=1;
    private float freqSub=1;

    private GameObject lrContainer;
    private GameObject lrContainerF;


    void Start()
    {
        float ymax = 4f;//Camera.main.orthographicSize;
        float xmax = 3f;//ymax * Screen.width / Screen.height;
        Debug.LogFormat("xmax: {0}, ymax: {1}", xmax, ymax);

        // we will ue the top left quadrant of the screen
        float xymargin = 0.5f;

        x01 = 6; 
        y01 = 1.5f / 3f * ymax - 0.3f;//0.05f / 3f * ymax - 0.3f;
        w1 = xmax - xymargin; 
        h1 = ymax / 3f - xymargin;

        x02 = 6; 
        y02 = -2f / 3f * ymax - 0.3f;
        w2 = xmax - xymargin; h2 = ymax * 2f / 3f - xymargin;


        Nfft = 1024;
        _tmax = 5f;
        _Fs = Nfft/(2 * _tmax); // sampling rate
        Nfreq = 50;    // how many harmonics to display

        wave1=GameObject.Find("SourceInput");
        //wave2=GameObject.Find("LocalOscillator");
        wave=GameObject.Find("MultiplyWaves");

        topPlot.CreateGrids(x01, y01, w1, h1, 1f, 1f, 1.1f * (_tmax / w1), 1.5f * (amp / h1), Color.grey, defaultLineMaterial, OriginLabel, true, true);
        bottomPlot.CreateGrids(x02, y02, w2, h2, 1f, 1f, 1f, 1f, Color.yellow, defaultLineMaterial, OriginLabel, true, false);
        OriginLabel.gameObject.SetActive(false);
      
         
       lrContainer = new GameObject("Function Plot");
              lrContainer.transform.SetParent(transform, false);
       _funclr = lrContainer.AddComponent<LineRenderer>();
           _funclr.positionCount = Nfft;
        _funclr.material = defaultLineMaterial;
        _funclr.useWorldSpace = true;
        _funclr.startWidth = 0.05f;
        _funclr.endWidth = 0.05f;
        _funclr.startColor = Color.yellow;
        _funclr.endColor = Color.yellow;
       
       lrContainerF = new GameObject("FFT Plot");
        lrContainerF.transform.SetParent(transform, false);
        _fftlr = lrContainerF.AddComponent<LineRenderer>();
        _fftlr.positionCount = 2 * Nfreq + 1;

        _fftlr.material = defaultLineMaterial;
        _fftlr.useWorldSpace = true;
        _fftlr.startWidth = 0.05f;
        _fftlr.endWidth = 0.05f;
        _fftlr.startColor = Color.white;
        _fftlr.endColor = Color.white;
   

    }


    // Update is called once per frame
    void Update()
    {   
       
        
          CreateFuncPlot(); 
          //CreateFFTPlot();
           
    }



    private void CreateFuncPlot()
    {
      //float amp, float freqAdd, float freqSub
       amp=wave.GetComponent<MultiplyWaves>().ampMul;
        freqAdd=wave.GetComponent<MultiplyWaves>().freqAdd;
        freqSub=wave.GetComponent<MultiplyWaves>().freqSub;
        /*
        amp1=wave1.GetComponent<SourceInput>()._magIS;
        freq1 =wave1.GetComponent<SourceInput>()._freqIS;
        Debug.LogFormat("amp: {0}, div: {1}", amp1, freq1);

        amp2=wave2.GetComponent<LocalOscillator>()._magLO;
        freq2 =wave2.GetComponent<LocalOscillator>()._freqLO;
        */
    

        _funcpts = new Vector2[Nfft];
        const float twoPI = 2 * Mathf.PI;

        for (int k=0; k<Nfft; k++)
        {
            float tval = Mathf.Lerp(-_tmax, _tmax, k/((float) Nfft-1));
            _funcpts[k].x = tval;
            _funcpts[k].y = 0.5f *amp * (Mathf.Cos( twoPI * freqAdd * tval )+ Mathf.Cos( twoPI * freqSub * tval ));
        }//Mathf.Cos( twoPI * (freq1-freq2)

    
          for (int k=0; k<Nfft; k++)
            _funclr.SetPosition(k, topPlot.ToScreenCoords(_funcpts[k]));
        
        //eqnText.updateEquationText(amp2, freq2 * 180 / Mathf.PI);

        

    }
/*
     private void CreateFFTPlot()
    {
        Complex[] samples = new Complex[Nfft];
        for (int k=0; k<Nfft; k++)
            samples[k] = _funcpts[k].y;

        // inplace bluestein FFT with default options
        //Fourier.Forward(samples, FourierOptions.AsymmetricScaling);

        _fftpts = new Vector2[2 * Nfreq + 1];
        _fftpts[Nfreq].x = 0;   // index for DC
        float maxyval = (float) samples[0].Magnitude;
        _fftpts[Nfreq].y = maxyval;   // DC value
       
        for (int k=0; k<Nfreq; k++)
        {    
            _fftpts[Nfreq + 1 + k].x = (k + 1) / (2 * _tmax);
            float mag = (float) samples[k + 1].Magnitude;
            _fftpts[Nfreq + 1 + k].y = mag;
            //added
            //_fftpts[Nfreq + 1 + k].x = _funcpts[k].x;

            if (mag > maxyval)
                maxyval = mag; 
        }
        
        bottomPlot.SetScaleXAxis(Nfreq/(2 * _tmax * w2), true);
         bottomPlot.SetScaleYAxis(maxyval, false);
        

        for (int k=0; k<=Nfreq; k++)
            _fftlr.SetPosition(k, bottomPlot.ToScreenCoords(_fftpts[k]));
  
        
        
    }

*/
 


}