using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The HUD
/// </summary>
public class HUD : MonoBehaviour
{
    [SerializeField]
    Text pathLengthText;

    [SerializeField] private Traveler traveler;

    private void Awake()
    {
	    traveler.AddPathFoundListener(SetPathLength);
    }

    /// <summary>
	/// Use this for initialization
	/// </summary>
	void Start()
	{
		
	}

    /// <summary>
    /// Sets the path length in the hud
    /// </summary>
    /// <param name="length">path length</param>
    void SetPathLength(float length)
    {
	    pathLengthText.text = "Path Length: " + length;
    }
}
