using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AutoScroll : MonoBehaviour 
{
    public ScrollRect scroll;
    public float scrollSpeed = 0.01f;

	void Update ()
    {
        
        if (scroll.horizontalNormalizedPosition >= 1.46)
        {
            scroll.horizontalNormalizedPosition = 0;
        }
        else
        {
            scroll.horizontalNormalizedPosition += scrollSpeed * Time.deltaTime;
        }
	}
}
