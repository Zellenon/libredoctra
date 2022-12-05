using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreqFreeplay : MonoBehaviour
{
        
        public float freqAdd=1;
        public float ampMul=1;
        private GameObject wave;
    // Start is called before the first frame update
    void Start()
    {
        wave=GameObject.Find("MultiplyWaves");
    }

    // Update is called once per frame
    void Update()
    {
        freqAdd=wave.GetComponent<MultiplyWaves>().freqAdd;
        ampMul=wave.GetComponent<MultiplyWaves>().ampMul;
        
        transform.position = new Vector2(freqAdd+3.75f, -2.35f);
        //transform.localScale=new Vector2(0.065625f,ampMul);
        
    }
}
