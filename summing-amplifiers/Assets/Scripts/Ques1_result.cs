using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
//using System.Windows.Forms;

public class Ques1_result : MonoBehaviour
{
    public Button buttonA;
    public Button buttonB;
    public TextMeshProUGUI resultBox;

	void Start () {
		Button btnA = buttonA.GetComponent<Button>();
        Button btnB = buttonB.GetComponent<Button>();
		btnA.onClick.AddListener(CorrectAnswer);
        btnB.onClick.AddListener(WrongAnswer);
	}

	void CorrectAnswer(){
		//Debug.Log ("CorrectAnswer!");
        resultBox.text = "CorrectAnswer!";
	}
    void WrongAnswer(){
		//Debug.Log ("WrongAnswer!");
        resultBox.text = "WrongAnswer! Correct Answer is A";
	}
}
