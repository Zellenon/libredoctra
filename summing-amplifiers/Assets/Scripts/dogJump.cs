using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dogJump : MonoBehaviour
{
    public Rigidbody dogRb;
    public GameObject matchedText;
    public GameObject dog;
    // Start is called before the first frame update
    void Start()
    {
        //dogRb=GetComponent<Rigidbody>();
       //dog=GameObject.Find("dog");
       matchedText=GameObject.Find("matched");
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    void jump(){
        if(matchedText.activeSelf){
              
         dog.SetActive(true);
        }
      
    }
}
