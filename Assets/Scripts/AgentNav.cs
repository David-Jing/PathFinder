using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using System.Collections;

// The individual NavAgent script that allows the agent to use linerenderer to
// display the various paths and change floors as needed. 
// These are initiated and managed via AgentControl.cs
// The comment out section includes a work-in-progress linerenderer animation
// Includes the clear linerenderer function

// Attached to: Nav/Floor Plan/ ( i.e. F2/F2 Agent)

public class AgentNav : MonoBehaviour
{
    public Transform StartAgent;
    public Transform locationcontainer;
    public Vector3 origin;
    public Vector3 destination;

    public Transform floorswitchcontainer;

    public static AgentNav instantance;

    public float lineDrawSpeed = 10f;

    private NavMeshAgent agent;
    private LineRenderer line;

    //private float counter, distance;
    //private int i = 0;
    //private bool drawing = false;

    void Awake() // So functions can be called in AgentControl
    {
        instantance = this;
    }

    public void Initiate()
    {
        line = GetComponent<LineRenderer>();
        agent = GetComponent<NavMeshAgent>();
        origin = agent.transform.position;
    }

    public void AgentNavigation() // Response to Search Room entries
    {
        string room = EventSystem.current.currentSelectedGameObject.GetComponentsInChildren<Text>()[0].text;
        string roomnumber = room.Substring(4, room.Length - 4);

        foreach (Transform child in floorswitchcontainer) // Change the map to the floor that contains the room.
        {
            if (child.name.Substring(0, 1) == roomnumber.Substring(0, 1))
            {
                child.GetComponent<Button>().onClick.Invoke();
                break;
            }
        }

        for (int i = 0; i < locationcontainer.childCount; i++)
        {
            if (roomnumber == locationcontainer.GetChild(i).name)
            {
                agent.Warp(origin); // Make sure the agent does not move
                destination = locationcontainer.GetChild(i).transform.position;

                StartCoroutine("GetPath");

                break;
            }
        }
    }

    public void SimpleAgentNavigation() // Used by the washroom calls
    {
        string room = EventSystem.current.currentSelectedGameObject.name;

        for (int i = 0; i < locationcontainer.childCount; i++)
        {
            if (room == locationcontainer.GetChild(i).name)
            {
                agent.Warp(origin); // Make sure the agent does not move
                destination = locationcontainer.GetChild(i).transform.position;

                StartCoroutine("GetPath");

                break;
            }
        }
    }

    IEnumerator GetPath()
    {
        agent.SetDestination(destination); // Initiate path calcuation
        agent.isStopped = true; // We don't want the agent to move

        yield return new WaitUntil(() => agent.hasPath); // Wait for path calcuation to complete

        line.positionCount = agent.path.corners.Length; // Allocate space for corner points
        line.SetPositions(agent.path.corners); // Draw (connect the corner points)
        line.enabled = true;
        /*
        line.SetPosition(0, origin);
        distance = Vector3.Distance(origin, destination);

        i = 0;

        drawing = true;
        */
    }
    /*
    void Update()
    {
        if (drawing)
        {
            if (distance > counter)
            {
                counter += 0.1f / lineDrawSpeed;
                float x = Mathf.Lerp(0, distance, counter);
                Vector3 p0 = origin;
                Vector3 p1 = destination;

                Vector3 pointAlongLine = x * Vector3.Normalize(p1 - p0) + p0;

                line.SetPosition(1, pointAlongLine);
            }
            else
            {
                if (i < line.positionCount - 1)
                {
                    Debug.Log("Next: " + i);
                    i++;
                    counter = 0;
                    distance = Vector3.Distance(agent.path.corners[i], agent.path.corners[i + 1]);
                }
                else
                {
                    drawing = false;
                }
            }
        }
    }
    */

    public void ClearLineRenderer()
    {
        if (gameObject.activeInHierarchy)
        {
            line.enabled = false;
        }
    }
}