using UnityEngine;
using UnityEngine.UI;

// Inititates each of the agent and assign responsibility when Search Room (SR) entries
// are called.

// Attached to Nav/Floor Plan

public class AgentControl : MonoBehaviour
{
    public Transform SRprefabcontainer;
    public Transform A5, A4, A3, A2;
    public Transform YouAreHere;

    private AgentNav[] Agents;

    void Start()
    {
        InvokeRepeating("AddListener", 1210f, 1200f);
    }

    public void Initiation()
    {
        Agents = new AgentNav[4];

        Agents[3] = A5.GetComponent<AgentNav>();
        Agents[2] = A4.GetComponent<AgentNav>();
        Agents[1] = A3.GetComponent<AgentNav>();
        Agents[0] = A2.GetComponent<AgentNav>();

        foreach (AgentNav agent in Agents)
        {
            agent.Initiate();
        }
    }

    public void AddListener()
    {
        foreach (Transform child in SRprefabcontainer)
        {
            string room = child.GetComponentsInChildren<Text>()[0].text;

            Button btn = child.GetComponent<Button>();

            btn.onClick.AddListener(Agents[int.Parse(room.Substring(4,1)) - 2].AgentNavigation);
        }
    }
}
