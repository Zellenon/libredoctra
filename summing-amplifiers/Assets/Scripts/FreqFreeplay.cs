using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreqFreeplay : MonoBehaviour
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
        freqAdd=wave.GetComponent<MultiplyWaves>().freqAdd;
        //freqSub=wave.GetComponent<MultiplyWaves>().freqSub;
        transform.position = new Vector2(freqAdd+3.75f, -2.634f);
        //void ShiftSliderUpdate(float value) {transform.localPosition = new Vector2(value,-3.49f);}
        
    }
}
