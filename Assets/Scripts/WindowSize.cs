using UnityEngine;

// Used to change viewport size within SearchRoom when keyboard pops up

// Attached to Overlay/UI/Search Room/Search Results/Viewport

public class WindowSize : MonoBehaviour {

    public Vector3 newXYZ;
    public Vector2 newWH;

    public Vector3 originXYZ;
    public Vector2 originWH;

    public void WindowSizeChange()
    {
        gameObject.GetComponent<RectTransform>().localPosition = newXYZ;
        gameObject.GetComponent<RectTransform>().sizeDelta = newWH;
    }

    public void WindowSizeRevert()
    {
        gameObject.GetComponent<RectTransform>().localPosition = originXYZ;
        gameObject.GetComponent<RectTransform>().sizeDelta = originWH;
    }
}
