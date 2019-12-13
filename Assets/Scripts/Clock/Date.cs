using System;
using UnityEngine;
using UnityEngine.UI;

public class Date : MonoBehaviour {

	private Text textDate;

	String[] Months = {"January", "Feburary", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"};

	void Start () 
    {
		textDate = GetComponent<Text> ();
		InvokeRepeating ("CurrentDate", 1f, 60f); // Check for date every minute.
	}

	void CurrentDate () 
    {
		textDate.text = DateTime.Now.Day + " " + Months[DateTime.Now.Month - 1] + ", " + DateTime.Now.Year; 
	}
}
