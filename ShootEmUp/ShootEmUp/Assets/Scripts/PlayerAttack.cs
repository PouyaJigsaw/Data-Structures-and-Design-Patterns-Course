using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;

    private bool canAttack = false;
    private float time = 0;
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (time < 0.5f)
        {
            time += Time.deltaTime;
        }
        else
        {
            time = 0;
            canAttack = true;
        }
        if (Input.GetButtonDown("Jump") && canAttack)
        {
            GameObject gameObject = ObjectPool.GetProjectile();
            gameObject.transform.position = transform.position;
            gameObject.GetComponent<Projectile>().StartMoving();
            canAttack = false;
        }

    
    }
}
