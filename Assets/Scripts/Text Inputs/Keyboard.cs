using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class Keyboard : MonoBehaviour {

	public Text Output;
	public bool DeletePressed = false;
	public float Timer = 0;

	public void GeneralKeys () {
		
		Output.text = Output.text + EventSystem.current.currentSelectedGameObject.name.ToLower ();
	}

	public void spacebar() {

		Output.text = Output.text + " ";

	}

	public void delete() {

		if (Output.text.Length > 0) {

			Output.text = Output.text.Substring (0, Output.text.Length - 1);

		}
	}

	public void clear() {

		Output.text = "";

	}

	void Update () {

		if (DeletePressed == true) {

			Timer += Time.deltaTime;

			if (Timer > 0.5 && Output.text.Length > 0) {

				Output.text = Output.text.Substring (0, Output.text.Length - 1);

			}
		}
	}

	public void onPointerDown() {

		DeletePressed = true;

	}
		
	public void onPointerUp() {
		
		DeletePressed = false;
		Timer = 0;

	}
}

