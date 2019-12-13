using SQLite4Unity3d;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System.Threading.Tasks;

public enum DataUpdateType
{
    PHONE_LIST
}

public class DataManager : Singleton<DataManager>
{
    private SQLiteConnection dbLink;

    public void Init()
    {
        string DatabaseName = "KioskDatabase.db";

        #region MULTI-PLATORM SETUP

#if UNITY_EDITOR
        var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID 
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
        var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
        // then save to Application.persistentDataPath
        File.Copy(loadDb, filepath);

#elif UNITY_STANDALONE_OSX
    var loadDb = Application.dataPath + "/Resources/Data/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
    // then save to Application.persistentDataPath
    File.Copy(loadDb, filepath);

#elif UNITY_STANDALONE
        //Logs.Instance.LogIt("Windows Standalone");
        //var loadDb = Application.stream/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
        var loadDb = Application.streamingAssetsPath +"/"+ DatabaseName;   
        // then save to Application.persistentDataPath
        File.Copy(loadDb, filepath);


#endif
            Debug.Log("Database written");
        }
         
        var dbPath = filepath;
#endif
        dbLink = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        //Debug.Log ("Final PATH: " + dbPath);

        #endregion

    }

    public async Task SQLcreate()
    {
        await CreateDB();
        await PhoneList();
        await CourseOffered();
        await ExamSchedule();
        await OfficeHours();
        await TAOfficeHours();
        await Tutorials();
    }

    public List<PhoneList> SearchPhoneList(string query)
    {
        List<PhoneList> people = dbLink.Query<PhoneList>(query);

        return people.Count > 0 ? people : null;
    }

    public List<CourseOffered> SearchCoursesOffered(string query)
    {
        List<CourseOffered> courses = dbLink.Query<CourseOffered>(query);

        return courses.Count > 0 ? courses : null;
    }

    public List<ExamSchedule> SearchExamSchedule(string query)
    {
        List<ExamSchedule> exams = dbLink.Query<ExamSchedule>(query);

        return exams.Count > 0 ? exams : null;
    }

    public List<OfficeHours> SearchOfficeHours(string query)
    {
        List<OfficeHours> offices = dbLink.Query<OfficeHours>(query);

        return offices.Count > 0 ? offices : null;
    }

    public List<TAOfficeHours> SearchTAOfficeHours(string query)
    {
        List<TAOfficeHours> TA = dbLink.Query<TAOfficeHours>(query);

        return TA.Count > 0 ? TA : null;
    }

    public List<Tutorials> SearchTutorials(string query)
    {
        List<Tutorials> Tut = dbLink.Query<Tutorials>(query);

        return Tut.Count > 0 ? Tut : null;
    }

    public IEnumerator CreateDB()
    {
        dbLink.DropTable<PhoneList>();
        dbLink.CreateTable<PhoneList>();

        dbLink.DropTable<CourseOffered>();
        dbLink.CreateTable<CourseOffered>();

        dbLink.DropTable<ExamSchedule>();
        dbLink.CreateTable<ExamSchedule>();

        dbLink.DropTable<OfficeHours>();
        dbLink.CreateTable<OfficeHours>();

        dbLink.DropTable<TAOfficeHours>();
        dbLink.CreateTable<TAOfficeHours>();

        dbLink.DropTable<Tutorials>();
        dbLink.CreateTable<Tutorials>();

        yield break;
    }

    #region DataPopulation
    public IEnumerator PhoneList()
    {
       PhoneList SQL_PL = new PhoneList();
       JSONNode JSON_PL = JSONReader.Read("GetPhoneList");
       List<Dictionary<string, object>> CSV_PL = CSVReader.Read("Doc_id_lut table");

       for (int i = 0; i < JSON_PL.Count; i++)
       {
           SQL_PL.StakeHolder_ID = int.Parse(JSON_PL[i]["Stakeholder_id"]);
           SQL_PL.Document_ID = 0;

           SQL_PL.PreferredFirstName = JSON_PL[i]["PreferredFirstName"];
           SQL_PL.FirstName = JSON_PL[i]["FirstName"];
           SQL_PL.LastName = JSON_PL[i]["LastName"];
           SQL_PL.ComputingID = JSON_PL[i]["ComputingID"];
           SQL_PL.Biographic = JSON_PL[i]["Biographic"];
           SQL_PL.EmailAddress = JSON_PL[i]["EmailAddress"];
           SQL_PL.Room = JSON_PL[i]["Room"];
           SQL_PL.PhoneNumber = JSON_PL[i]["Phonenumber"];
           SQL_PL.Title = JSON_PL[i]["PAD"];

           // --- PICTURE ---
           for (int j = 0; j < CSV_PL.Count; j++)
           {
               string ID = JSON_PL[i]["Stakeholder_id"].ToString();

               if (CSV_PL[j]["stakeholder_id"].ToString() == ID.Substring(1, ID.Length - 2))
               {
                   SQL_PL.Document_ID = int.Parse(CSV_PL[j]["document_id"].ToString());
                   break;
               }
           }

           dbLink.Insert(SQL_PL);
        }

        yield break;
    }

    public IEnumerator CourseOffered()
    {
        CourseOffered SQL_CO = new CourseOffered();
        JSONNode JSON_CO = JSONReader.Read("GetCoursesOffered");

        for (int i = 0; i < JSON_CO.Count; i++)
        {
            SQL_CO.CoursesOffered_ID = JSON_CO[i]["courses_offered_id"] != "null" ?
                int.Parse(JSON_CO[i]["courses_offered_id"]) : 0;
            SQL_CO.StakeHolder_ID = JSON_CO[i]["stakeholder_id"] != "null" ?
                int.Parse(JSON_CO[i]["stakeholder_id"]) : 0;
            SQL_CO.Unit = JSON_CO[i]["Unit"] != "null" ?
                int.Parse(JSON_CO[i]["Unit"]) : 0;

            SQL_CO.Section = JSON_CO[i]["Section"];
            SQL_CO.TermDisplay = JSON_CO[i]["TermDisplay"];
            SQL_CO.Location = JSON_CO[i]["Location"];
            SQL_CO.PreferredFirstName = JSON_CO[i]["PreferredFirstName"];
            SQL_CO.FirstName = JSON_CO[i]["FirstName"];
            SQL_CO.LastName = JSON_CO[i]["LastName"];
            SQL_CO.EmailAddress = JSON_CO[i]["EmailAddress"];
            SQL_CO.Day = JSON_CO[i]["Day"];
            SQL_CO.Time = JSON_CO[i]["Time"];
            SQL_CO.Room = JSON_CO[i]["Room"];
            SQL_CO.CourseTitle = JSON_CO[i]["CourseTitle"];
            SQL_CO.CourseSubject = JSON_CO[i]["CourseSubject"];
            SQL_CO.CourseNumber = JSON_CO[i]["CourseNumber"];

            dbLink.Insert(SQL_CO);
        }

        yield break;
    }

    public IEnumerator ExamSchedule()
    {
        ExamSchedule SQL_ES = new ExamSchedule();
        JSONNode JSON_ES = JSONReader.Read("GetExamSchedule");

        for (int i = 0; i < JSON_ES.Count; i++)
        {
            SQL_ES.CoursesOffered_ID = JSON_ES[i]["courses_offered_id"] != "null" ?
                int.Parse(JSON_ES[i]["courses_offered_id"]) : 0;

            SQL_ES.LastName = JSON_ES[i]["LastName"];
            SQL_ES.FirstName = JSON_ES[i]["FirstName"];
            SQL_ES.PreferredFirstName = JSON_ES[i]["PreferredFirstName"];
            SQL_ES.ExamDate = JSON_ES[i]["ExamDate"];
            SQL_ES.StartTime = JSON_ES[i]["StartTime"];
            SQL_ES.EndTime = JSON_ES[i]["EndTime"];
            SQL_ES.Room = JSON_ES[i]["Room"];
            SQL_ES.Day = JSON_ES[i]["Day"];
            SQL_ES.Section = JSON_ES[i]["Section"];
            SQL_ES.Location = JSON_ES[i]["Location"];
            SQL_ES.Subject = JSON_ES[i]["Subject"];
            SQL_ES.CourseNumber = JSON_ES[i]["CourseNumber"];

            dbLink.Insert(SQL_ES);
        }

        yield break;
    }

    public IEnumerator OfficeHours()
    {
        OfficeHours SQL_OH = new OfficeHours();
        JSONNode JSON_OH = JSONReader.Read("GetOfficeHours");

        for (int i = 0; i < JSON_OH.Count; i++)
        {
            SQL_OH.CoursesOffered_ID = JSON_OH[i]["courses_offered_id"] != "null" ?
                int.Parse(JSON_OH[i]["courses_offered_id"]) : 0;
            SQL_OH.StakeHolder_ID = JSON_OH[i]["stakeholder_id"] != "null" ?
                int.Parse(JSON_OH[i]["stakeholder_id"]) : 0;

            SQL_OH.LastName = JSON_OH[i]["LastName"];
            SQL_OH.FirstName = JSON_OH[i]["FirstName"];
            SQL_OH.PreferredFirstName = JSON_OH[i]["PreferredFirstName"];
            SQL_OH.Section = JSON_OH[i]["Section"];
            SQL_OH.Location = JSON_OH[i]["Location"];
            SQL_OH.Subject = JSON_OH[i]["Subject"];
            SQL_OH.CourseNumber = JSON_OH[i]["CourseNumber"];
            SQL_OH.TermDisplay = JSON_OH[i]["TermDisplay"];
            SQL_OH.StartTime = JSON_OH[i]["StartTime"];
            SQL_OH.EndTime = JSON_OH[i]["EndTime"];
            SQL_OH.Room = JSON_OH[i]["Room"];
            SQL_OH.Day = JSON_OH[i]["Day"];
            SQL_OH.OfficeLocation = JSON_OH[i]["OfficeLocation"];

            dbLink.Insert(SQL_OH);
        }

        yield break;
    }

    public IEnumerator TAOfficeHours()
    {
        TAOfficeHours SQL_TA = new TAOfficeHours();
        JSONNode JSON_TA = JSONReader.Read("GetTAOfficeHours");

        for (int i = 0; i < JSON_TA.Count; i++)
        {
            SQL_TA.CoursesOffered_ID = JSON_TA[i]["courses_offered_id"] != "null" ?
                int.Parse(JSON_TA[i]["courses_offered_id"]) : 0;
            SQL_TA.StakeHolder_ID = JSON_TA[i]["stakeholder_id"] != "null" ?
                int.Parse(JSON_TA[i]["stakeholder_id"]) : 0;

            SQL_TA.Course = JSON_TA[i]["Course"];
            SQL_TA.TAName = JSON_TA[i]["TaName"];
            SQL_TA.OfficeHour = JSON_TA[i]["OfficeHour"];

            dbLink.Insert(SQL_TA);
        }

        yield break;
    }

    public IEnumerator Tutorials()
    {
        Tutorials SQL_T = new Tutorials();
        JSONNode JSON_T = JSONReader.Read("GetTutorials");

        for (int i = 0; i < JSON_T.Count; i++)
        {
            SQL_T.CoursesOffered_ID = JSON_T[i]["courses_offered_id"] != "null" ?
                int.Parse(JSON_T[i]["courses_offered_id"]) : 0;

            SQL_T.CourseSubject = JSON_T[i]["CourseSubject"];
            SQL_T.CourseNumber = JSON_T[i]["CourseNumber"];
            SQL_T.Section = JSON_T[i]["Section"];
            SQL_T.TutorialSection = JSON_T[i]["TutorialSection"];
            SQL_T.Day = JSON_T[i]["Day"];
            SQL_T.StartTime = JSON_T[i]["StartTime"];
            SQL_T.EndTime = JSON_T[i]["EndTime"];
            SQL_T.Room = JSON_T[i]["Room"];
            SQL_T.TAName = JSON_T[i]["TAName"];
            SQL_T.InstructorName = JSON_T[i]["InstructorName"];

            dbLink.Insert(SQL_T);
        }

        yield break;
    }
    #endregion
}
