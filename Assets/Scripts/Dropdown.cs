using UnityEngine;

// Effectively minimize an attached panel on a object by changing scaling

// Attached to: Overlay/UI/Course Info/Info Panel/Viewport/Prefab/Course Details

public class Dropdown : MonoBehaviour 
{
    public Transform container;
    private RectTransform panel;

    public void DropPanel ()
    {
        panel = transform.GetChild(2).GetComponent<RectTransform>();
        Vector3 scale = panel.localScale;

        if (scale.y > 0.5f) // Close drop down
        {
            scale.y = 0;
            foreach (Transform child in container)
            {
                child.gameObject.SetActive(true);
            }
        }

        else // Open drop down
        {
            scale.y = 1;
            foreach (Transform child in container)
            {
                child.gameObject.SetActive(false);
            }

            gameObject.SetActive(true);
        }

        panel.localScale = scale; // Update
    }
}