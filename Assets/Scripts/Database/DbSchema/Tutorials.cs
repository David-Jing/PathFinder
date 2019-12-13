using SQLite4Unity3d;

public class Tutorials
{
    [PrimaryKey, AutoIncrement]
    public int Index { get; set; }

    public int CoursesOffered_ID { get; set; }

    public string CourseSubject { get; set; }
    public string CourseNumber { get; set; }
    public string Section { get; set; }
    public string TutorialSection { get; set; }
    public string Day { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public string Room { get; set; }
    public string TAName { get; set; }
    public string InstructorName { get; set; }

    public override string ToString()
    {
        return string.Format("{0} is TA for {1} {2} for {3}; starts at {4} and ends at {5} in {6}", 
                             TAName, CourseSubject, CourseNumber, InstructorName, StartTime, EndTime,
                             Room);
    }
}

/* Data from JSON web: 
CourseSubject   "BUS"
CourseNumber    "201"
course_id   "7"
courses_offered_id  "6801"
Section "D100"
Term    "1187"
tutorial_schedule_id    "2266"
TutorialSection "D101"
Type    "SEM"
Day "Fr"
StartTime   "12:30 PM"
EndTime "1:20 PM"
Room    "WMC2210"
EnrollmentActual    "1"
TAName  null
InstructorName  "Spector, Stephen"
InstructorID    "20321"
*/

