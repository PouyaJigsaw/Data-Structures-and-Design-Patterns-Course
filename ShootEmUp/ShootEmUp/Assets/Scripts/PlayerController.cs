using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    private BoxCollider2D _boxCollider2D;

    [SerializeField] [Range(1, 40)] private int DenominatorForSpeed = 15;
    // Start is called before the first frame update
    void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
       
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 previousPos = transform.position;
       transform.Translate(new Vector3(0 , Input.GetAxis("Vertical") / DenominatorForSpeed ));
       
       if (PlayerOutOfBorder())
       {
           //clamp
           transform.position = previousPos;
       }
    }

    private bool PlayerOutOfBorder()
    {
        return transform.position.y + _boxCollider2D.bounds.extents.x > ScreenUtils.topBorder || transform.position.y - _boxCollider2D.bounds.extents.x < ScreenUtils.bottomBorder;
    }
}
