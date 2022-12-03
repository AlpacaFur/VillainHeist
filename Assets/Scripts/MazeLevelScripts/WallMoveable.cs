using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class WallMoveable : MonoBehaviour
{
    public int movingUp;
    private float topLimit;
    private bool atTopLimit;
    private float thisMuch;
    [SerializeField]
    private float moveSpeed;

    private string activationKey;

    private bool active;
    [SerializeField]
    private GameObject marker;

    [SerializeField] [Tooltip("Name of Marker")]
    private string markName;
    // Start is called before the first frame update
    void Start()
    {
        thisMuch = 3;
        atTopLimit = false;
        movingUp = 0;
        topLimit = transform.position.y + thisMuch;
        marker = GameObject.Find(markName);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (active)
        {
            WallMove();
        }
        else
        {
            active = marker.GetComponent<MarkerScript>().canFlip;
            activationKey = marker.GetComponent<MarkerScript>().keyActivator;
        }

    }

    void WallMove()
    {
        if (Input.GetKeyDown(activationKey))
        {
            if (!atTopLimit)
            {
                movingUp = 1;
            }
            else
            {
                movingUp = -1;
            }

        }

        if ((transform.position.y > topLimit && !atTopLimit)
            || (transform.position.y < (topLimit - thisMuch) && atTopLimit) )
        {
            movingUp = 0;
            atTopLimit = !atTopLimit;
        }

        Vector3 posn = transform.position;
        posn = posn + new Vector3(0, thisMuch * movingUp * moveSpeed, 0);
        transform.position = posn;
    }
}
