using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;
//using System.Random;
public class UpdateIF : MonoBehaviour
{
    private GameObject RF;
    private GameObject LO;
    private GameObject IF;
    private GameObject DesiredIF;
    private float RFpositionX;
    private float RFScaleY;
    private float IFpositionX;
    private float IFScaleY;
    private float LOpositionX;
    private float LOScaleY;
    private float DesiredIFpositionX;
    private float DesiredIFScaleY;
    private float IFScaleY_temp;
    public ParticleSystem explotionParticle;
    bool particleSystemPlayed = false;
    // Start is called before the first frame update
    private AudioSource playerAudio;
    public AudioClip match_sound;
    float x;
    
    Vector3 pos;
    void Start()
    {
        
        RF=GameObject.Find("RF");
        IF=GameObject.Find("ParentIF");
        LO=GameObject.Find("ParentLO");
        DesiredIF=GameObject.Find("DesiredIF");
        x = Random.Range(0, 5);
      
        pos = new Vector3(x, -1.18f, 0);
        DesiredIF.transform.position = pos;
        
        

        DesiredIFpositionX=DesiredIF.transform.position.x;
        DesiredIFScaleY=DesiredIF.transform.localScale.y;
        playerAudio=GetComponent<AudioSource>();

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
        
        match();
       
        
        

    }

    void match(){
        IFScaleY_temp=IF.transform.localScale.y;
      if((Math.Abs(DesiredIFpositionX-IFpositionX)<0.05) && (Math.Abs(DesiredIFScaleY-IFScaleY)<0.09)){
        Debug.Log("IF matched to Desired IF");
        Debug.Log("Desired IF ");
        Debug.Log(DesiredIFScaleY);
        Debug.Log("IF ");
        Debug.Log(IFScaleY);
        explotionParticle.Play();
        playerAudio.PlayOneShot(match_sound,9.0f);
        StartCoroutine(StopParticleSystem(explotionParticle, 2));
        
        
       }
       
    }
    IEnumerator StopParticleSystem(ParticleSystem particleSystem, float time)
 {
     yield return new WaitForSeconds(time);
     
     particleSystem.Stop();
 }
      
  
    
}
