using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dogJump : MonoBehaviour
{
    public Rigidbody dogRb;
    public GameObject matchedText;
    // Start is called before the first frame update
    void Start()
    {
        dogRb=GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    void jump(){
        if(matchedText.activeSelf){
              
      dogRb.AddForce(transform.up*1000);
        }
      
    }
}
