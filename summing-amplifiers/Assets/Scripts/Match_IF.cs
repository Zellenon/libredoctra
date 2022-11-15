using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Match_IF : MonoBehaviour
{
     private Slider scaleSlider;
    public float scaleMinValue;
    public float scaleMaxValue;
    
    private Slider scaleSliderX;
    public float scaleXMinValue;
    public float scaleXMaxValue;

    void Start() {
        scaleSlider = GameObject.Find("MagnitudeSlider").GetComponent<Slider>();
        scaleSlider.minValue = scaleMinValue;
        scaleSlider.maxValue = scaleMaxValue;
        scaleSlider.onValueChanged.AddListener(ScaleSliderUpdate);

        scaleSliderX = GameObject.Find("FreqSlider").GetComponent<Slider>();
        scaleSliderX.minValue = scaleXMinValue;
        scaleSliderX.maxValue = scaleXMaxValue;
        scaleSliderX.onValueChanged.AddListener(ShiftSliderUpdate);
    }
    
    void ScaleSliderUpdate(float value){transform.localScale = new Vector2(0.304793f,value);}
    void ShiftSliderUpdate(float value) {transform.localPosition = new Vector2(value,-1.91f);}
   

    
}
