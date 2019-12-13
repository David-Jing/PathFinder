using UnityEngine;
using UnityEngine.UI;
using System;

public class DigitalClock : MonoBehaviour 
{
	private Text textClock;
	String ampm = "am";
	int Hour = 0;
	int Minute = 0;

	void Start()
    {
		textClock = GetComponent<Text> ();
        InvokeRepeating("Time", 1f, 1f); // Check every second.
	}

	void Time()
    {
		Minute = DateTime.Now.Minute;
        Hour = DateTime.Now.Hour;

        if (Hour >= 12)
        {
            ampm = "pm";

            if (Hour >= 13)
            {
                Hour -= 12;
            }
        }
        else if (Hour == 0)
        {
            Hour = 12;
        }
        else
        {
            ampm = "am";
        }


        textClock.text = Hour.ToString() + ":" + Minute.ToString().PadLeft(2, '0') + " " + ampm;
	}
}