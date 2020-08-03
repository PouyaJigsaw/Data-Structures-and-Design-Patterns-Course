using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Builds the graph
/// </summary>
public class GraphBuilder : MonoBehaviour
{
    static Graph<Waypoint> graph;
    private GameObject start;
    private GameObject end;
    private List<GameObject> waypoints;
    /// <summary>
    /// Awake is called before Start
    /// </summary>
    void Awake()
    {
        graph = new Graph<Waypoint>();
        waypoints = new List<GameObject>();
        // add nodes (all waypoints, including start and end) to graph
        start = GameObject.FindGameObjectWithTag("Start");
        end = GameObject.FindGameObjectWithTag("End");

        graph.AddNode(start.GetComponent<Waypoint>());
        graph.AddNode(end.GetComponent<Waypoint>());

        waypoints.Add(start);
        waypoints.Add(end);
    

        foreach (var waypoint in GameObject.FindGameObjectsWithTag("Waypoint"))
        {
            graph.AddNode(waypoint.GetComponent<Waypoint>());
            waypoints.Add(waypoint);
        }


        float horizontal;
        float vertical;
        float distance;
        // add edges to graph
        for (int i = 0; i < graph.Count; i++)
        {
            for (int j = 0; j < graph.Count; j++)
            {
                horizontal = Math.Abs(waypoints[i].transform.position.x - waypoints[j].transform.position.x);
                vertical = Math.Abs(waypoints[i].transform.position.y - waypoints[j].transform.position.y);
                distance = Vector3.Distance(waypoints[i].transform.position, waypoints[j].transform.position);
                if (horizontal < 3.5 && vertical < 3)
                {
                    graph.AddEdge(waypoints[i].GetComponent<Waypoint>(), waypoints[j].GetComponent<Waypoint>(), distance);
                }
            }
        }
        
            
        
    }

    /// <summary>
    /// Gets the graph
    /// </summary>
    /// <value>graph</value>
    public static Graph<Waypoint> Graph
    {
        get { return graph; }
    }
}
