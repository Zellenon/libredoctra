using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplyWaves : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject wave1;
    private float amp1=1;
    private float freq1=1;

    //amplitute and freq of sinewave2
    private GameObject wave2;
    private float amp2=1;
    private float freq2=1;
    public float ampMul=1;
    public float freqSub=1;
    public float freqAdd=1;

    void Start()
    {
   
    wave1=GameObject.Find("SourceInput");
    wave2=GameObject.Find("LocalOscillator");
        
    }

    // Update is called once per frame
    void Update()
    {
         amp1=wave1.GetComponent<SourceInput>()._magIS;
        freq1 =wave1.GetComponent<SourceInput>()._freqIS;

        amp2=wave2.GetComponent<LocalOscillator>()._magLO;
        freq2 =wave2.GetComponent<LocalOscillator>()._freqLO;
        ampMul=amp1*amp2;
        freqSub=freq1-freq2;
        freqAdd=freq1+freq2;
        

        
    }
    
}
