using SQLite4Unity3d;

public class CourseOffered
{
    [PrimaryKey, AutoIncrement]
    public int Index { get; set; }

    public int CoursesOffered_ID { get; set; }
    public int StakeHolder_ID { get; set; }
    public int Unit { get; set; }

    public string Section { get; set; }
    public string TermDisplay { get; set; }
    public string Location { get; set; }
    public string PreferredFirstName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public string Day { get; set; }
    public string Time { get; set; }
    public string Room { get; set; }
    public string CourseTitle { get; set; }
    public string CourseSubject { get; set; }
    public string CourseNumber { get; set; }

    public override string ToString()
    {
        return string.Format("{0} {1} ({2}) is being taught in room {3}", CourseSubject, CourseNumber, CourseTitle, Room);
    }
}

/* Data from JSON web: 
http://interweb.bus.sfu.ca/webservices/index.php?service=GeneralOfficeRight&request=GetCoursesOffered

courses_offered_id  "5042"
Section "D100"
Term    "1184"
Location    "BBY"
ListAoL "No"
RecordStatus    "Active"
EnrollmentForScheduler  "168"
EnrollmentAllowed   "144"
EnrollmentActual    "126"
Unit    "3"
CourseTitle "Managerial Accounting I"
CourseOutlineSentShort  "Yes"
CourseOutlineReceivedShort  "Yes"
CourseOutlineSentDetailed   "Yes"
CourseOutlineReceivedDetailed   "Yes"
CourseOutlineGraduateSent   "No"
CourseOutlineGraduateReceived   "No"
GraduateReadingListSent "No"
GraduateReadingListReceived "No"
RequestSentScheduling   "Yes"
RequestReceivedScheduling   "Yes"
RequestDeadlineDate null
RequestCourseOutlineShortDeadlineDate   null
RequestCourseOutlineDetailedDeadlineDate    null
RequestReceivedOverload "No"
RequestSentOverload "Yes"
DeskCopyInstructorNeed  "0"
DeskCopyInstructorNeedNum   "0"
DeskCopySupplementaryNeed   "0"
DeskCopySupplementary   ""
AddsWillYouAdd  "Yes: Use the waitlist"
OverloadWillYouOverload "No"
WhatTutAreYouTeaching   null
OverloadNumberOfStudents    null
AddOverloadNotes    null
shortOutlineUrl "https://beedie.sfu.ca/sm…in/downloads.php?d=Yz62B"
outlineStatus   "Approved"
detailedOutlineUrl  "https://beedie.sfu.ca/sm…in/downloads.php?d=JwlKe"
graduateOutlineUrl  null
GraduateReadinglistOutlineUrl   null
AddOverloadDeadline "0000-00-00"
TermDisplay "Summer 2018"
course_id   "12"
ProgramType "Internal"
CourseSubject   "BUS"
CourseNumber    "254"
WQB "Q"
stakeholder_id  "48701"
FirstName   "Kwai Man Teresa"
PreferredFirstName  "Teresa"
LastName    "Fung"
emplid  "301324624"
ComputingID "kmfung"
Action  "REH"
Rank    "Lecturer"
RequestReceivedOfficeHour   "No"
RequestSentOfficeHour   "Yes"
PrimaryInstructor   "Yes"
ExamPackageSent "Yes"
course_s_id "16040"
Day "Tu"
Time    "12:30 PM-2:20 PM"
Room    "AQ3182"
StartDate   "2018-05-01"
EndDate "2018-08-31"
EmailAddress    "kmfung@sfu.ca"
Preferred   "Yes"
course_schedule_id  "20020"
StartTime   "12:30 PM"
Endtime "2:20 PM"
Capacity    "253"
course_schedule_option_id   "433"
Primary "Yes"
L_AreaAbb   "ACCT"
L_ProgramAbb    "BBA"
status  "Approved"
GraduateOutlineStatus   null
GraduateReadinglistStatus   null
GradesDueDate   "14 August, 2018"

*/




