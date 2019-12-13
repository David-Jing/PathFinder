using SQLite4Unity3d;

public class TAOfficeHours
{
    [PrimaryKey, AutoIncrement]
    public int Index { get; set; }

    public int StakeHolder_ID { get; set; }
    public int CoursesOffered_ID { get; set; }

    public string Course { get; set; }
    public string TAName { get; set; }
    public string OfficeHour { get; set; }

    public override string ToString()
    {
        return string.Format("{0} is in TAing for {1}, office hour: {2}", TAName, Course, OfficeHour);
    }
}

/* Data from JSON web: 
Course  "BUS 251-3 D100"
courses_offered_id  "4972"
stakeholder_id  "37872"
TaName  "Kwok, Danielle"
OfficeHour  null
*/

