using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/* References: 
MI = "More Info", UI/ResultPage/MoreInfo
SR = "Search Room", UI/SearchRoom(/Results)
CL = "Course List", UI/CourseInfo/CourseList(/Viewport)
IP = "Info Panel", UI/CourseInfo/InfoPanel(/Viewport)
CD = "Course Details". IO/CourseInfo/InfoPanel/ViewPort/Prefab/CourseDetails
*/

// Manages entries and data within Course List, Info Panel, and pass offs to RoomSearch.cs

// Attached to: Overlay/UI

public class CourseInfoData : MonoBehaviour
{
    public Transform CLprefabcontainer;
    public Transform CLprefab;

    public Text IPcoursenumber;
    public Text IPcoursename;
    public Transform IPprefab;
    public Transform IPprefabcontainer;
    public Transform IPbackbutton;

    public Transform SRprefabcontainer;

    public Transform CourseFilter;

    private Button btn;
    private Text[] info;

    private List<PhoneList> phonelist;
    private List<CourseOffered> coursesoffered;

    void Start()
    {
        DataManager.Instance.Init(); // Allows for SQL usage
    }

    public void DataPull() // Fill courseinfo main page
    {
        // One entry per class instead of multiple entries due to different sections
        coursesoffered = DataManager.Instance.SearchCoursesOffered(
            "SELECT * FROM CourseOffered GROUP BY CourseNumber");

        foreach (Transform child in CLprefabcontainer)
        {
            Destroy(child.gameObject);
        }

        if (coursesoffered == null)
        {
            return;
        }

        CLprefab.gameObject.SetActive(true);

        foreach (CourseOffered course in coursesoffered)
        {
            int year = int.Parse(course.CourseNumber.Substring(0, 1));

            if (year < 5)
            {
                // Create entry
                Transform clone = Instantiate(CLprefab);
                clone.transform.SetParent(CLprefabcontainer);

                // Renaming to the course name
                clone.name = course.CourseSubject + " " + course.CourseNumber;

                // Course name
                clone.GetComponentsInChildren<Text>()[0].text = clone.name;
                // Course title
                clone.GetComponentsInChildren<Text>()[1].text = course.CourseTitle;
                // Course offerred ID for reference in InfoPanelActivated()
                clone.GetChild(2).name = course.CoursesOffered_ID.ToString();

                // Change button block color;
                ColorChange(year, clone); 
            }
        }

        // Ensure "All" Toggle is selected
        CourseFilter.GetComponent<CourseInfoToggleReset>().ToggleReset();

        foreach (Transform child in CLprefabcontainer)
        {
            btn = child.GetComponent<Button>();
            btn.onClick.AddListener(InfoPanelActivate);
        }

        CLprefab.gameObject.SetActive(false);
    }

    public void InfoPanelActivate() // Info panel content filling
    {
        IPprefab.gameObject.SetActive(true);

        Transform selected = EventSystem.current.currentSelectedGameObject.transform;
        IPcoursename.text = selected.GetChild(1).GetComponent<Text>().text; // i.e. "Business Fundamentals"
        IPcoursenumber.text = selected.GetChild(0).GetComponent<Text>().text; // i.e. BUS 200-3

        // Extracts the course number (i.e. 200)
        string coursenumber = IPcoursenumber.text.Substring(IPcoursenumber.text.Length - 3, 3);
        
        coursesoffered = DataManager.Instance.SearchCoursesOffered(
            "SELECT * FROM CourseOffered WHERE CourseNumber LIKE '" + coursenumber + "'");

        // Note that destroy does not occur immediately, i.e. problems with AccessCourseList
        foreach (Transform child in IPprefabcontainer)
        {
            child.GetComponentsInChildren<Text>()[0].text = "NUll"; // Resolves issues by overwriting section
            Destroy(child.gameObject);
        }

        if (coursesoffered == null)
        {
            return;
        }

        IPcoursenumber.text += "-" + coursesoffered[0].Unit; // Adding the number of units to the coursenumber

        string prev_section = "";
        int weeklyoccurance = 1;

        foreach (CourseOffered course in coursesoffered)
        {
            if (prev_section != course.Section) // New Entry
            {
                weeklyoccurance = 1;
                Transform clone = Instantiate(IPprefab);
                clone.transform.SetParent(IPprefabcontainer);
                clone.name = IPcoursenumber.text;

                info = clone.GetComponentsInChildren<Text>();

                // Section
                info[0].text = course.Section;
                // Name
                info[1].text = course.FirstName == course.PreferredFirstName
                    ? course.LastName + ", " + course.FirstName
                    : course.LastName + ", " + course.FirstName + " (" + course.PreferredFirstName + ")";
                // Time and location
                info[2].text = course.Day != null && course.Time != null && course.Room != null 
                    ? course.Day + ", " + course.Time + ", " + course.Room 
                    : "n/a";
                // Campus location
                info[5].text = course.Location;
                // Term
                info[6].text = course.TermDisplay;
                // Course ID
                clone.GetChild(0).GetChild(7).name = course.CoursesOffered_ID.ToString();

                // Check if RoomSearch has an entry for this lecturer
                phonelist = DataManager.Instance.SearchPhoneList(
                    "SELECT * FROM PhoneList WHERE (StakeHolder_ID LIKE '"
                        + course.StakeHolder_ID.ToString() + "' AND Room LIKE 'WMC%')");
                clone.GetChild(0).GetChild(1).GetComponent<Button>().interactable &= phonelist != null;

                clone.GetComponent<Button>().onClick.AddListener(CourseDetail);
            }
            else // New time
            {
                weeklyoccurance++;
                info[1 + weeklyoccurance].text = course.Day + ", " + course.Time + ", " + course.Room;
            }
            prev_section = course.Section;
        }

        IPprefab.gameObject.SetActive(false);
    }

    void ColorChange(int course_year, Transform item)
    {
        if (course_year <= 5)
        {
            // SFU red to SFU gray
            item.GetComponent<Image>().color = new Color(0.651f - 0.110f * (course_year - 2), 0.098f + 0.082f * (course_year - 2), 0.180f + 0.055f * (course_year - 2), 1f);
        }
        else
        {
            // SFU gray to SFU blue
            item.GetComponent<Image>().color = new Color(0.329f - 0.066f * (course_year - 5), 0.349f + 0.019f * (course_year - 5), 0.353f + 0.047f * (course_year - 5), 1f);
        }
    }

    public void CourseDetail()
    {
        Transform entry = EventSystem.current.currentSelectedGameObject.transform;

        string CourseID = entry.GetChild(0).GetChild(7).name;

        Transform container = entry.GetChild(2).GetChild(0);

        Transform CDexam = container.GetChild(0);
        Transform CDtut = container.GetChild(1);
        Transform CDoff = container.GetChild(2);

        // ------ Exam -------
        List<ExamSchedule> exam = DataManager.Instance.SearchExamSchedule(
            "SELECT * FROM ExamSchedule WHERE CoursesOffered_ID LIKE '" + CourseID + "'");

        if (exam != null)
        {
            CDexam.GetChild(0).GetComponent<Text>().text = exam[0].ExamDate != null 
                ? exam[0].Room + ", " + exam[0].StartTime + " - " + exam[0].EndTime + ", " + exam[0].ExamDate 
                : "n/a";
        }

        // ------ Tutorial -------
        List<Tutorials> tut = DataManager.Instance.SearchTutorials(
            "SELECT * FROM Tutorials WHERE CoursesOffered_ID LIKE '" + CourseID + "'");
            
        Transform prefab = CDtut.GetChild(0);
        Transform prefabcontainer = CDtut.GetChild(1);

        foreach (Transform child in prefabcontainer)
        {
            Destroy(child.gameObject);
        }

        prefab.gameObject.SetActive(true);

        if (tut != null)
        {
            foreach (Tutorials sessions in tut)
            {
                Transform clone = Instantiate(prefab);
                clone.SetParent(prefabcontainer);

                info = clone.GetComponentsInChildren<Text>();

                info[0].text = sessions.TutorialSection;
                info[1].text = sessions.TAName;
                info[2].text = sessions.Day;
                info[3].text = sessions.StartTime + " - " + sessions.EndTime;
                info[4].text = sessions.Room;
            }
        }

        prefab.gameObject.SetActive(false);

        // ------ Office Hour (Prof) -------
        List<OfficeHours> officehour = DataManager.Instance.SearchOfficeHours(
            "SELECT * FROM OfficeHours WHERE (CoursesOffered_ID LIKE '" + CourseID + "' "
            + "AND StartTime NOT LIKE '00:00%' AND EndTime NOT LIKE '00:00%' AND Room NOT LIKE '%NULL%')");
            
        prefab = CDoff.GetChild(0);
        prefabcontainer = CDoff.GetChild(1);

        foreach (Transform child in prefabcontainer)
        {
            Destroy(child.gameObject);
        }

        prefab.gameObject.SetActive(true);

        if (officehour != null)
        {
            foreach (OfficeHours sessions in officehour)
            {
                Transform clone = Instantiate(prefab);
                clone.SetParent(prefabcontainer);

                info = clone.GetComponentsInChildren<Text>();

                info[0].text = sessions.LastName + ", " + sessions.PreferredFirstName;
                info[1].text = 
                    sessions.StartTime + " - " + sessions.EndTime 
                    + ", " + sessions.Day + ", " + sessions.Room
                    + "   (" + sessions.OfficeLocation + ")";
                info[2].text = "LECT";
            }
        }

        // ------ Office Hour (TA) -------
        List<TAOfficeHours> TAofficehour = DataManager.Instance.SearchTAOfficeHours(
            "SELECT * FROM TAOfficeHours WHERE (CoursesOffered_ID LIKE '" + CourseID + "' " +
            "AND OfficeHour NOT LIKE '00:00%' AND OfficeHour NOT LIKE '%NULL%')");

        if (TAofficehour != null)
        {
            foreach (TAOfficeHours sessions in TAofficehour)
            {
                Transform clone = Instantiate(prefab);
                clone.SetParent(prefabcontainer);

                info = clone.GetComponentsInChildren<Text>();

                info[0].text = sessions.TAName;
                info[1].text = sessions.OfficeHour;
                info[2].text = "TA";
            }
        }

        prefab.gameObject.SetActive(false);
    }

    public void AccessRoomSearch() // Pass off to RoomSearch; triggering the buttons in RoomSearch.
    {
        Transform entry = EventSystem.current.currentSelectedGameObject.transform;

        foreach (Transform child in SRprefabcontainer)
        {
            string Name = child.GetChild(1).name;

            if (entry.GetComponent<Text>().text == Name)
            {
                EventSystem.current.SetSelectedGameObject(child.gameObject);
                child.GetComponent<Button>().onClick.Invoke();
                break;
            }
        }
    }
}