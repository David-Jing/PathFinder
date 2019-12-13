using UnityEngine;
using UnityEngine.UI;

public class ClearText : MonoBehaviour 
{
	public Text TEXTBOX;

	public void EmptyText () 
    {
		TEXTBOX.text = "";
	}
}
