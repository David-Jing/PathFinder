using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

// Distributes call responsibility among the agents (i.e. F2 agent should respond to F2 rooms)
// Room buttons are disabled if no entries exist for it
// Hands over to RoomSearch.cs to deal with Result Page results

// Attached to: Overlay/Room Button (and Map Elements)

public class RoomButton : MonoBehaviour {

    public Transform SRprefabcontainer;
    public Transform Roomcontainer;

    public Transform HomeBackButton;
    public Transform SRBackButton;

    public void RoomButtonAssignment() 
    {
        foreach (Transform group in Roomcontainer)
        {
            foreach (Transform room in group)
            {
                if (!room.name.Any(char.IsLetter))
                {
                    room.GetComponent<Button>().interactable = false;
                    foreach (Transform child in SRprefabcontainer)
                    {
                        string SRroom = child.GetComponentsInChildren<Text>()[0].text;

                        // Extract, i.e., "2301.5" from "WMC 2301.5" and other normal cases
                        if (SRroom.Substring(4, SRroom.Length - 4) == room.name)
                        {
                            room.GetComponent<Button>().interactable = true;
                            room.GetComponent<Button>().onClick.AddListener(RoomDetail);
                            continue;
                        }
                    }
                }
            }
        }
    }

    public void RoomDetail()
    {
        Transform selected = EventSystem.current.currentSelectedGameObject.transform;

        foreach (Transform child in SRprefabcontainer)
        {
            string SRroom = child.GetComponentsInChildren<Text>()[0].text;
            if (SRroom.Substring(4, SRroom.Length - 4) == selected.name)
            {
                // Load Result Page via Search Room
                EventSystem.current.SetSelectedGameObject(child.gameObject);
                child.GetComponent<Button>().onClick.Invoke();

                // Calling SR button will make the Result Page back button return to SR, we want home instead.
                HomeBackButton.gameObject.SetActive(true);
                SRBackButton.gameObject.SetActive(false);
            }
        }
    }
}
