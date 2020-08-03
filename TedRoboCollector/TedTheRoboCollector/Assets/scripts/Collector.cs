using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A collecting game object
/// </summary>
public class Collector : MonoBehaviour
{
	#region Fields

    // targeting support
    SortedList<Target> targets = new SortedList<Target>();
    Target targetPickup = null;
    private int numPickUps = 0;

    // movement support
	const float BaseImpulseForceMagnitude = 2.0f;
    const float ImpulseForceIncrement = 0.3f;
	
	// saved for efficiency
    Rigidbody2D rb2d;

    #endregion

    #region Methods

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start()
    {
		// center collector in screen
		Vector3 position = transform.position;
		position.x = 0;
		position.y = 0;
		position.z = 0;
		transform.position = position;

		// save reference for efficiency
		rb2d = GetComponent<Rigidbody2D>();

        // add as listener for pickup spawned event
        EventManager.AddListener(HandleSpawnTargetEvent);
	}

    /// <summary>
    /// Called when another object is within a trigger collider
    /// attached to this object
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay2D(Collider2D other)
    {
        // only respond if the collision is with the target pickup
		if (other.gameObject == targetPickup.GameObject)
        {
	        // remove collected pickup from list of targets and game
	        targets.RemoveAt(targets.IndexOf(targetPickup));
			Destroy(targetPickup.GameObject);
	        
	        if (targets.Count != 0)
	        {
		        for (int i = 0; i < targets.Count; i++)
		        {
			        targets[i].UpdateDistance(transform.position);
		        }
		        targets.Sort();
	        }
	        
	        // go to next target if there is one
	        if (targets.Count == 0)
	        {
		     
		        targetPickup = null;
	        }
	        else
	        {
		        
		        SetTarget(targets[targets.Count - 1]);
	        }

	        numPickUps--;

        }
	}

    
	/// <summary>
	/// Sets the target pickup to the provided pickup
	/// </summary>
	/// <param name="pickup">Pickup.</param>
	void SetTarget(Target pickup)
    {
	    
		targetPickup = pickup;

		GoToTargetPickup();
	}


	/// <summary>
	/// Starts the teddy bear moving toward the target pickup
	/// </summary>
	void GoToTargetPickup()
    {
	    Debug.Log("GoToTargetPickUp: " + targetPickup.GameObject.transform.position);
        // calculate direction to target pickup and start moving toward it
		Vector2 direction = new Vector2(
			targetPickup.GameObject.transform.position.x - transform.position.x,
			targetPickup.GameObject.transform.position.y - transform.position.y);
		direction.Normalize();
		rb2d.velocity = Vector2.zero;
		rb2d.AddForce(direction * (BaseImpulseForceMagnitude + ImpulseForceIncrement * numPickUps), 
			ForceMode2D.Impulse);
	}



	void HandleSpawnTargetEvent(GameObject pickup)
	{
		Target target = new Target(pickup, transform.position);
	
		numPickUps++;

		if (targetPickup != null)
		{
			if (target.Distance < targets[targets.Count - 1].Distance)
			{
				targets.Add(target);
				SetTarget(target);
			}
			else
			{
				targets.Add(target);
			}
		}
		else
		{
			
			targets.Add(target);
			SetTarget(target);
		}
		
		
	}
	
	#endregion
}
