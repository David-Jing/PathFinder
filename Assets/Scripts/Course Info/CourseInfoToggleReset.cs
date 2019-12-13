using UnityEngine;
using UnityEngine.UI;

// Resets Toggle Filter

// Attached to Overlay/UI/Course Info/Course Fitlers (Toggle Container)

public class CourseInfoToggleReset : MonoBehaviour {

    public Transform togglecontainer;
    public Text resultcounter;
    public Transform CLprefabcontainer;

    public void ToggleReset()
    {
        foreach (Transform child in togglecontainer)
        {
            child.GetComponent<Toggle>().isOn = child.name == "All";
        }

        resultcounter.GetComponent<Text>().text =
                    "Displaying " + CLprefabcontainer.childCount + " results";
    }
}