using SQLite4Unity3d;

public class PhoneList
{
    [PrimaryKey, AutoIncrement]
    public int Index { get; set; }

    public int StakeHolder_ID { get; set; }
    public int Document_ID { get; set; }

    public string PreferredFirstName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ComputingID { get; set; }
    public string Biographic { get; set; }
    public string EmailAddress { get; set; }
    public string Room { get; set; }
    public string PhoneNumber { get; set; }
    public string Title { get; set; }

    public override string ToString()
    {
        return string.Format("{0} {1} ({2}) is in room {3}", PreferredFirstName, LastName, EmailAddress, Room);
    }
}

/* Data from JSON web: 
http://interweb.bus.sfu.ca/webservices/index.php?service=GeneralOfficeRight&request=GetPhoneList

Stakeholder_id  "227"
PreferredFirstName  "Andrew"
FirstName   "Andrew"
MiddleName  "Dennis"
LastName    "Flostrand"
emplid  "903017955"
ComputingID "flostran"
Credentials "BSc, MBA"
Biographic  "&lt;p&gt;Andrew holds an… the banjo.&lt;/p&gt;\n"
ResearchInterests   ""
Email_id    "31644"
EmailAddress    "flostran@sfu.ca"
Type    "Campus"
Preferred   "Yes"
Room    "WMC 4359"
Campus_Code "BURNABY"
PrimaryLocation "No"
Phonenumber "778.782.4199"
Rank    "Term Lecturer"
Position    "Sessional Lecturer II St… Operations Management "
WebDirectoryTitle   "Sessional Lecturer II St… Operations Management "
PAD

*/




