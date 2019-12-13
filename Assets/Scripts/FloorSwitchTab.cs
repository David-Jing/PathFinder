using UnityEngine;
using UnityEngine.UI;

// When a floor is selected the background color changes

// Attached to Overlay/Floor Switch/(i.e. 2F Button)

public class FloorSwitchTab : MonoBehaviour {

    public Transform container;

    public void FloorTabActivate()
    {
        foreach (Transform child in container)
        {
            child.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

        gameObject.GetComponent<Image>().color = new Color32(229, 229, 229, 225);
    }
}
