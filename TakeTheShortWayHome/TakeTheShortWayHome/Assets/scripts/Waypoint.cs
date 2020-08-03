using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

/// <summary>
/// A waypoint
/// </summary>
public class Waypoint : MonoBehaviour
{
    [SerializeField]
    int id;

    [SerializeField]
    GameObject explosionPrefab;
    private Traveler _traveler;

    
    private void Start()
    {
      
        _traveler = GameObject.FindGameObjectWithTag("Traveler").GetComponent<Traveler>();
        _traveler.AddPathTraversalCompleteListener(CheckForDestruction);
        
    }

    /// <summary>
    /// Changes waypoint to green
    /// </summary>
    /// <param name="other">other collider</param>
    void OnTriggerEnter2D(Collider2D other)
    {

    }

    /// <summary>
    /// Gets the position of the waypoint
    /// </summary>
    /// <value>position</value>
    public Vector2 Position
    {
        get { return transform.position; }
    }

    /// <summary>
    /// Gets the unique id for the waypoint
    /// </summary>
    /// <value>unique id</value>
    public int Id
    {
        get { return id; }
    }


    void CheckForDestruction()
    {
        if (gameObject.GetComponent<SpriteRenderer>().color == Color.green)

        {
            
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
