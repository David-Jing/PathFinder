using UnityEngine;

public class PopUpWindow : MonoBehaviour 
{
	public void Show () 
    {
        gameObject.SetActive(true);
	}

	public void Hide () 
    {
        gameObject.SetActive(false);
	}
}
