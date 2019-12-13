using UnityEngine;
using UnityEngine.UI;

// Manages the filtering toggles in Course Info

// Attached to: Overlay/UI/Course Info/Course Filters (Toggle Container)/ (i.e. 2)

public class CourseInfoToggle : MonoBehaviour 
{
	public Transform prefabcontainer;
	public Text resultcounter;

    public Transform togglecontainer;

    private static int counter = 0; 

	void Start() 
    {
        Toggle CourseFilter = GetComponent<Toggle> ();
		CourseFilter.onValueChanged.AddListener(OnToggleValueChange);
    }

    public void OnToggleValueChange (bool ison)
    {
		if (ison) 
        {
            //--------ALL-------
            if (gameObject.name == "All")
            {
                for (int i = 0; i < togglecontainer.childCount - 1; i++)
                {
                    togglecontainer.GetChild(i).GetComponent<Toggle>().isOn = false;
                }

                foreach (Transform child in prefabcontainer)
                {
                    child.gameObject.SetActive(true);
                }

                resultcounter.GetComponent<Text>().text = 
                    "Displaying " + prefabcontainer.childCount + " results";
            }

            //--------200, 300, 400, etc-------
            else
            {
                // Turn off "All Toggle" which deactivates all clones in prefabcontainer
                togglecontainer.GetChild(togglecontainer.childCount - 1).GetComponent<Toggle>().isOn = false;
                string yearselected = gameObject.name;

                bool enteredsection = false; // The courses are sorted by year
                foreach (Transform child in prefabcontainer)
                {
                    if (child.name.Substring(child.name.Length - 3, 1) == yearselected)
                    {
                        // Activate matched courses
                        child.gameObject.SetActive(true);
                        enteredsection = true;
                        counter++; // Increase count;
                    }
                    else
                    {
                        if (enteredsection == true) // Will not find anymore matches
                        {
                            break;
                        }
                    }
                }
                resultcounter.GetComponent<Text>().text = 
                    "Displaying " + counter.ToString() + " results";
            }
		} 

		else 
        {
            //--------ALL-------
            if (gameObject.name == "All")
            {
                foreach (Transform child in prefabcontainer)
                {
                    child.gameObject.SetActive(false);
                }

                counter = 0;

                resultcounter.GetComponent<Text>().text = 
                    "Displaying " + counter.ToString() + " results";
            }

            //--------200, 300, 400, etc-------
            else
            {
                string yearselected = gameObject.name;

                bool enteredsection = false; // The courses are sorted by year

                foreach (Transform child in prefabcontainer)
                {
                    if (child.name.Substring(child.name.Length - 3, 1) == yearselected)
                    {
                        // Activate matched courses
                        child.gameObject.SetActive(false);
                        enteredsection = true;
                        counter--; // Decrease count;
                    }
                    else
                    {
                        if (enteredsection == true) // Will not find anymore matches
                        {
                            break;
                        }
                    }
                }
                resultcounter.GetComponent<Text>().text =
                    "Displaying " + counter.ToString() + " results";
            }
        }
    }
}
