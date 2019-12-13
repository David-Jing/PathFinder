using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Collections.Generic;

// Responsible for pulling profile pictures and data files every x minutes,
// and refreshing all existing entries based on the new data.

// Attached to: Web Request

public class WebRequest : MonoBehaviour 
{
    public Transform PictureContainer;

    private bool exit = false;

    public Transform LoadingScreen;

    void Start () 
    {
        InvokeRepeating("CallDataRequest", 1200f, 1200f);
	} 

    public async Task CallDataRequest()
    {
        await DataRequest();
        // Refresh all existing entries
        await LoadingScreen.GetComponent<LoadingScreen>().SysInit();
    }

    public IEnumerator DataRequest()
    {
        string path = "Assets/Resources/";
        string startURL = "http://interweb.bus.sfu.ca/webservices/index.php?service=GeneralOfficeRight&request=";
        string[] list = { "GetPhoneList", "GetCoursesOffered",
            "GetClassAndLabSchedule","GetTutorials", "GetOfficeHours", 
            "GetTAOfficeHours", "GetExamSchedule" };

        for (int i = 0; i < 7; i++)
        {
            UnityWebRequest request = new UnityWebRequest(startURL + list[i], UnityWebRequest.kHttpVerbGET)
            {
                downloadHandler = new DownloadHandlerFile(path + list[i] + ".json")
            };

            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.LogError(request.error);
                yield return new WaitForSeconds(30.0f);
                yield return DataRequest(); // Retrying

                exit = true;
                yield break;
            }
        }

        if (exit == true)
        {
            exit = false;
            yield break;
        }

        DataManager.Instance.Init();
        yield return DataManager.Instance.SQLcreate();
    }

    public IEnumerator PhotoRequest() // Dump photos in PhotoContainer and with StakeHolder_ID as their name
    {
        DataManager.Instance.Init();
        List<PhoneList> phonelist = DataManager.Instance.SearchPhoneList(
            "SELECT * FROM PhoneList WHERE(Document_ID NOT LIKE '0'AND Room LIKE 'WMC%')"
        );

        if (PictureContainer.childCount > 1) // Remove old entries
        {
            for (int i = 1; i < PictureContainer.childCount; i++)
            {
                Destroy(PictureContainer.GetChild(i).gameObject);
            }
        }

        foreach (PhoneList entry in phonelist)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(
                "https://beedie.sfu.ca/sms/admin/download.php?sid=" + entry.StakeHolder_ID.ToString()
                + "&docid=" + entry.Document_ID.ToString());

            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Texture photo = ((DownloadHandlerTexture)request.downloadHandler).texture as Texture;

                if (photo != null)
                {
                    Texture2D photo2D = photo as Texture2D;

                    Transform clone = Instantiate(PictureContainer.GetChild(0));
                    clone.SetParent(PictureContainer, false);
                    clone.name = entry.StakeHolder_ID.ToString();

                    Sprite sprite_photo = Sprite.Create(
                        photo2D, new Rect(0f, 0f, photo.width, photo.height), new Vector2(0.5f, 0.5f), 100.0f);

                    clone.GetComponent<Image>().sprite = sprite_photo;

                    clone.gameObject.SetActive(false);
                }
            }
        }
    }
}