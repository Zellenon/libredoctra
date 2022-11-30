using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreqPeak2 : MonoBehaviour
{
        public float freqSub=1;
        public float freqAdd=1;
        private GameObject wave;
        
    // Start is called before the first frame update
    void Start()
    {
        wave=GameObject.Find("MultiplyWaves");
    }

    // Update is called once per frame
    void Update()
    {
        //amp=wave.GetComponent<MultiplyWaves>().ampMul;
        freqSub=wave.GetComponent<MultiplyWaves>().freqSub;
        //freqSub=wave.GetComponent<MultiplyWaves>().freqSub;
        transform.position = new Vector2(freqSub+7f, -2.35f);
        //void ShiftSliderUpdate(float value) {transform.localPosition = new Vector2(value,-3.49f);}
        
    }
}
