using SQLite4Unity3d;

public class OfficeHours
{
    [PrimaryKey, AutoIncrement]
    public int Index { get; set; }

    public int StakeHolder_ID { get; set; }
    public int CoursesOffered_ID { get; set; }

    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string PreferredFirstName { get; set; }
    public string Section { get; set; }
    public string Location { get; set; }
    public string Subject { get; set; }
    public string CourseNumber { get; set; }
    public string TermDisplay { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public string Room { get; set; }
    public string Day { get; set; }
    public string OfficeLocation { get; set; }

    public override string ToString()
    {
        return string.Format("{0} {1}'s office hour is from {2} to {3} at {4} for {5} {6}", PreferredFirstName,
                             LastName, StartTime, EndTime, Room, Subject, CourseNumber);
    }
}

/* Data from JSON web: 
LastName    "Paschen"
FirstName   "Ulrich"
PreferredFirstName  "Ulrich"
stakeholder_id  "52682"
courses_offered_id  "10816"
Section "E300"
Location    "BBY"
Unit    "3"
Type    "Internal"
Subject "BUS"
CourseNumber    "361"
Term    "1187"
TermDisplay "Fall 2018"
Area    "MIS"
StartTime   null
EndTime null
Room    null
Day null
OfficeLocation  null
officeID    null
Notes   null
*/
