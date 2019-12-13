using UnityEngine;
using UnityEngine.UI;

// Reset to home screen if cursor location has not been moved

// Attached to: Idle Counter

public class IdleDetection : MonoBehaviour {

    public Button resetbutton;
    private float countdowntime = 60f;
    private Vector3 prevMousePosition = Vector3.zero;

	void Start () 
    {
        InvokeRepeating("Countdown", countdowntime, countdowntime); 
	}

	void Countdown () 
    {
        if (Input.mousePosition == prevMousePosition)
        {
            resetbutton.onClick.Invoke();
        }
        prevMousePosition = Input.mousePosition;
	}
}
