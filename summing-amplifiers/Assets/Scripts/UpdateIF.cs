using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UpdateIF : MonoBehaviour
{
    private GameObject RF;
    private GameObject LO;
    private GameObject IF;
    private float RFpositionX;
    private float RFScaleY;
    private float IFpositionX;
    private float IFScaleY;
    private float LOpositionX;
    private float LOScaleY;
    // Start is called before the first frame update
    void Start()
    {
        RF=GameObject.Find("RF");
        IF=GameObject.Find("ParentIF");
        LO=GameObject.Find("ParentLO");

    }

    // Update is called once per frame
    void Update()
    {
        //IF=RF - LO= RFpositionX - LOpositionX
        RFpositionX=RF.transform.position.x;
        RFScaleY=RF.transform.localScale.y;

        LOpositionX=LO.transform.position.x;
        LOScaleY=LO.transform.localScale.y;

       IFpositionX= RFpositionX - LOpositionX;

       IFScaleY= (RFScaleY*LOScaleY)/2;

        transform.position = new Vector3(IFpositionX, -3.51f,0);
        transform.localScale = new Vector2(0.304793f,IFScaleY);


        
    }
}
