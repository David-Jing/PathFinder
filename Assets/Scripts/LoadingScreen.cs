using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

// Calls WebRequest.cs to pull data and pictures, then call various scripts 
// to start processing the data. 

// Attached to: Overlay/Loading Screen

public class LoadingScreen : MonoBehaviour
{
    public Transform WebReq;
    public Transform FloorPlan;
    public Transform RoomButtons;
    public Transform UI;
    public Transform Reset;

    public Text LoadingText;
    private bool isLoading = true;

    async Task Start()
    {
        LoadingText.text = "Loading...";
        await WebRequesting();
        LoadingText.text = "Creating entries...";
        await SysInit();
        LoadingText.text = "Starting...";
        Reset.GetComponent<ButtonTrigger>().TriggerButton();
        isLoading = false;
        gameObject.SetActive(false);
    }

    public IEnumerator WebRequesting() // Pull data from server
    {
        LoadingText.text = "Downloading data...";
        yield return WebReq.GetComponent<WebRequest>().DataRequest();
        LoadingText.text = "Downloading images...";
        yield return WebReq.GetComponent<WebRequest>().PhotoRequest();
    }

    public IEnumerator SysInit() // Data processing
    {
        // ------- Course/Person entries -------
        UI.GetComponent<CourseInfoData>().DataPull();
        UI.GetComponent<RoomSearch>().DataPull();
        UI.GetComponent<RoomSearch>().SearchUpdate();

        // ------- AgentNav ------
        FloorPlan.GetComponent<AgentControl>().Initiation();
        FloorPlan.GetComponent<AgentControl>().AddListener();
        RoomButtons.GetComponent<RoomButton>().RoomButtonAssignment();

        yield break;
    }

    void Update()
    {
        if (isLoading == true)
        {
            // pulse the transparency of the loading text to let the player know that the computer is still working.
            LoadingText.color = new Color(LoadingText.color.r, LoadingText.color.g, LoadingText.color.b, Mathf.PingPong(Time.time, 1));
        }
    }
}