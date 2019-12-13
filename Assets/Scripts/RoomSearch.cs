using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/* References:
MI = "More Info", UI/ResultPage/MoreInfo
SR = "Search Room", UI/SearchRoom(/Results)
CL = "Course List", UI/CourseInfo/CourseList(/Viewport)
IP = "Info Panel", UI/CourseInfo/InfoPanel(/Viewport)
PS = "Profile Switch", UI/Result Page/ProfileSwitch
*/

// Manages entries and data within Search Room, Search Result, and pass offs to CourseInfoData.cs

public class RoomSearch : MonoBehaviour
{
    public Transform SRprefab;
    public Transform SRprefabcontainer;

    public Text userinput;
    public Text displayresult;

    public Transform MI;
    public Transform MIprefab;
    public Transform MIprefabcontainer;
    public Transform MIphotos;

    public Transform CLprefabcontainer;

    public Transform IPprefabcontainer;

    public Transform PSprefab;
    public Transform PSprefabcontainer;

    private Text[] info;
    private int matchcounter;
    private string cache;

    private bool bypass = false;

    private List<PhoneList> phonelist;
    private List<CourseOffered> coursesoffered;

    void Start()
    {
        DataManager.Instance.Init(); // Allows for SQL usage
        InvokeRepeating("SearchUpdate", 1f, 0.2f);
    }

    public void DataPull() // Fill RoomSearch main page
    {
        // Data pulled from the PhoneList
        phonelist = DataManager.Instance.SearchPhoneList("SELECT * FROM PhoneList WHERE Room LIKE 'WMC%'");
        
        foreach (GameObject child in SRprefabcontainer)
        {
            Destroy(child);
        }

        if (phonelist == null)
        {
            return;
        }

        foreach (PhoneList person in phonelist)
        {
            // Create data entry
            Transform clone = Instantiate(SRprefab); 
            // Renaming to their respective name
            clone.name = person.LastName + ", " + person.PreferredFirstName;
            clone.transform.SetParent(SRprefabcontainer);

            info = clone.GetComponentsInChildren<Text>();

            // Room (i.e. WMC 3311)
            info[0].text = person.Room;

            // Prefered Name
            info[1].text = clone.name;

            // Reference for CourseInfo; full name
            info[1].name = person.FirstName == person.PreferredFirstName
                ? person.LastName + ", " + person.FirstName
                : person.LastName + ", " + person.FirstName + " (" + person.PreferredFirstName + ")";

            // Title
            info[2].text = person.Title == "null"
                ? ""
                : person.Title;

            // Email; only SFU email
            info[3].text = person.EmailAddress == "null"
                || person.EmailAddress.Substring(person.EmailAddress.Length - 7, 7) != "@sfu.ca"
                ? "Email:"
                : "Email: " + person.EmailAddress;

            // Phone
            info[4].text = person.PhoneNumber == "null"
                ? "Phone:"
                : "Phone: " + person.PhoneNumber;

            // StakeHolder_ID for reference later with ResultPageActivate() and SearchUpdate()
            clone.GetChild(5).name = person.StakeHolder_ID.ToString();

            // Document ID for get photo
            clone.GetChild(6).name = person.Document_ID.ToString();
        }

        SRprefab.gameObject.SetActive(false);
        displayresult.text = "Displaying " + SRprefabcontainer.childCount.ToString() + " results for \"" + userinput.text + "\"";

        // Keeps track of input updates
        cache = userinput.text; 

        foreach (Transform child in SRprefabcontainer)
        {
            Button btn = child.GetComponent<Button>();
            btn.onClick.AddListener(ResultPageTab);
            btn.onClick.AddListener(ResultPageActivate);
        }

        bypass = true;
        SearchUpdate();
    }

    public void SearchUpdate() // Search result update
    {
        int resultcount = 0;
        if (cache != userinput.text || bypass)
        {
            bypass = false;
            matchcounter = 0;

            if (userinput.text != "") // Note: if "", SQL returns empty; we want the opposite
            {
                // The entries that matches with the user entered Room, Name, or Title in WMC area
                phonelist = DataManager.Instance.SearchPhoneList("SELECT * FROM PhoneList " +
                                                     "WHERE (PreferredFirstName LIKE '%" + userinput.text + "%' OR FirstName LIKE '%" + userinput.text + "%' " +
                                                     "OR LastName LIKE '%" + userinput.text + "%' OR Room LIKE '%" + userinput.text + "%') " +
                                                     "AND Room LIKE 'WMC%'");

                // Deactivate all existing entries that does not match with the new phonelist created above in ~O(n) time
                if (phonelist != null) // Matches found
                {
                    int[] IDlist = new int[phonelist.Count + 1];
                    int i = 0;
                    foreach (PhoneList person in phonelist)
                    {
                        IDlist[i] = person.StakeHolder_ID;
                        i++;
                    }
                    IDlist[phonelist.Count] = -1;
                    i = 0;
                    foreach (Transform child in SRprefabcontainer)
                    {
                        if (int.Parse(child.GetChild(5).name) == IDlist[i])
                        {
                            child.gameObject.SetActive(true);
                            i++;
                        }
                        else
                        {
                            child.gameObject.SetActive(false);
                            matchcounter--;
                        }
                    }
                    resultcount = phonelist.Count;
                }
                else // No match entries
                {
                    foreach (Transform child in SRprefabcontainer)
                    {
                        child.gameObject.SetActive(false);
                    }
                    resultcount = 0;
                }
            }
            else // Blank input
            {
                foreach (Transform child in SRprefabcontainer)
                {
                    child.gameObject.SetActive(true);
                }
                resultcount = SRprefabcontainer.childCount;
            }

            displayresult.text = "Displaying " + resultcount.ToString() + " results for \"" + userinput.text + "\"";
            cache = userinput.text;
        }
    }

    public void ResultPageActivate() // Fill in data of selected individual in Result Page
    {
        Transform selected = EventSystem.current.currentSelectedGameObject.transform; // The selected button in SR

        int StakeHolder_ID = int.Parse(selected.GetChild(5).name);
        coursesoffered = DataManager.Instance.SearchCoursesOffered("SELECT * FROM CourseOffered WHERE StakeHolder_ID LIKE '" + StakeHolder_ID + "'");

        Text[] data = selected.GetComponentsInChildren<Text>();
        Text[] entries = MI.GetComponentsInChildren<Text>();

        entries[0].text = data[1].name; // Last, First (Preferred) Name
        entries[1].text = data[2].text; // Title
        entries[2].text = data[4].text; // Phone
        entries[3].text = data[3].text; // Email
        entries[4].text = "Room: " + data[0].text; // Room

        // ----- Tab System -----
        foreach (Transform child in PSprefabcontainer) // Change the matched tab's color to its respective values
        {
            child.GetComponent<Image>().color = child.name == selected.GetChild(5).name // Match Stakeholder_ID
                ? new Color32(233, 233, 233, 233)
                : new Color32(255, 255, 255, 255);
        }

        // ----- Profile Picture -----
        if (selected.GetChild(6).name != "0") // If profile exists
        {
            foreach (Transform child in MIphotos)
            {
                child.gameObject.SetActive(false);
            }

            MIphotos.Find(selected.GetChild(5).name).gameObject.SetActive(true);
        }
        else
        {
            foreach (Transform child in MIphotos)
            {
                child.gameObject.SetActive(false);
            }

            MIphotos.GetChild(0).gameObject.SetActive(true);
        }

        // ----- Courses Teaching -----
        foreach (Transform child in MIprefabcontainer) // Remove old course entries
        {
            Destroy(child.gameObject);
        }

        if (coursesoffered != null)
        {
            MIprefab.gameObject.SetActive(true);

            foreach (CourseOffered course in coursesoffered) // Courses the individual is teaching
            {
                Transform clone = Instantiate(MIprefab);
                clone.transform.SetParent(MIprefabcontainer);
                entries = clone.GetComponentsInChildren<Text>();

                clone.GetComponent<Button>().interactable &= int.Parse(course.CourseNumber.Substring(course.CourseNumber.Length - 3, 1)) <= 4;

                // Course name (i.e. BUS 200)
                entries[0].text = course.CourseSubject + " " + course.CourseNumber;

                // Course title
                entries[1].text = course.CourseTitle;

                // Course section
                entries[2].text = course.Section;

                // Course term
                entries[3].text = course.TermDisplay;
            }
        }

        MIprefab.gameObject.SetActive(false);
    }

    private void ResultPageTab() // Manages tab entries
    {
        Transform selected = EventSystem.current.currentSelectedGameObject.transform; // The selected button in SR
        Text[] data = selected.GetComponentsInChildren<Text>();

        List<PhoneList> roomlookup = DataManager.Instance.SearchPhoneList("SELECT * FROM PhoneList " +
                                                                          "WHERE Room LIKE '%" + data[0].text + "'");

        foreach (Transform child in PSprefabcontainer) // Remove old tabs
        {
            Destroy(child.gameObject);
        }

        int tenants = roomlookup.Count;

        PSprefab.gameObject.SetActive(true);

        for (int i = 0; i < tenants; i++)
        {
            Transform tab = Instantiate(PSprefab);
            tab.transform.SetParent(PSprefabcontainer);

            Vector3 tabPos = tab.GetComponent<RectTransform>().position;
            tabPos.Set(-488.2f, 399.2f - i * 181.5f, 0f);

            tab.GetChild(0).GetComponent<Text>().text = roomlookup[i].LastName + ", " + roomlookup[i].PreferredFirstName;
            tab.name = roomlookup[i].StakeHolder_ID.ToString();

            tab.GetComponent<Button>().onClick.AddListener(ResultPageRefresh_Tab);
        }

        PSprefab.gameObject.SetActive(false);
    }

    private void ResultPageRefresh_Tab()
    {
        Transform selected = EventSystem.current.currentSelectedGameObject.transform; // The tab that was selected

        foreach (Transform child in SRprefabcontainer)
        {
            if (child.GetChild(5).name == selected.name)
            {
                EventSystem.current.SetSelectedGameObject(child.gameObject);
                ResultPageActivate();
                return;
            }
        }
    }

    public void AccessCourseList() // Pass off to Course List; triggering the course button in Course List
    {
        Transform entry = EventSystem.current.currentSelectedGameObject.transform;

        int l = 0;
        int r = CLprefabcontainer.childCount - 1;

        string course = entry.GetComponentsInChildren<Text>()[0].text;

        // The course name
        int target = int.Parse(entry.GetComponentsInChildren<Text>()[0].text.Substring(entry.GetComponentsInChildren<Text>()[0].text.Length - 3, 3));
        string section = entry.GetComponentsInChildren<Text>()[2].text;

        while (l <= r) //binary search; find the button in Course List that matches with the clicked course
        {
            int m = l + (r - l) / 2;

            if (int.Parse(CLprefabcontainer.GetChild(m).name.Substring(CLprefabcontainer.GetChild(m).name.Length - 3, 3)) == target)
            {
                EventSystem.current.SetSelectedGameObject(CLprefabcontainer.GetChild(m).gameObject);
                CLprefabcontainer.GetChild(m).GetComponent<Button>().onClick.Invoke(); // Invoke button

                foreach (Transform child in IPprefabcontainer) // Find the course that matches with its section
                {
                    if (section == child.GetComponentsInChildren<Text>()[0].text)
                    {
                        EventSystem.current.SetSelectedGameObject(child.gameObject); // Allows for coursedetails to be updated
                        child.GetComponent<Button>().onClick.Invoke(); // Invoke "more info" option in the Info
                        break;
                    }
                }

                break;
            }

            if (int.Parse(CLprefabcontainer.GetChild(m).name.Substring(CLprefabcontainer.GetChild(m).name.Length - 3, 3)) < target)
            {
                l = m + 1;
            }

            else
            {
                r = m - 1;
            }
        }
    }
}