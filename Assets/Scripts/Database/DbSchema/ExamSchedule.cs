using SQLite4Unity3d;

public class ExamSchedule
{
    [PrimaryKey, AutoIncrement]
    public int Index { get; set; }

    public int CoursesOffered_ID { get; set; }

    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string PreferredFirstName { get; set; }
    public string ExamDate { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public string Room { get; set; }
    public string Day { get; set; }
    public string Section { get; set; }
    public string Location { get; set; }
    public string Subject { get; set; }
    public string CourseNumber { get; set; }

    public override string ToString()
    {
        return string.Format("{0} {1}'s final exam is at room {2} on {3}", Subject, CourseNumber, Room, ExamDate);
    }
}

/* Data from JSON web: 
Instructors "Susan Bubra(Susan) "
LastName    "Bubra"
FirstName   "Susan"
PreferredFirstName  "Susan"
ExamDate    null
StartTime   null
EndTime null
Room    null
Day null
HasExam "No"
courses_offered_id  "4972"
Section "D100"
Location    "BBY"
EnrollmentAllowed   "168"
EnrollmentActual    "168"
Unit    "3"
Type    "Internal"
Subject "BUS"
CourseNumber    "251"
Area    "ACCT"
*/
