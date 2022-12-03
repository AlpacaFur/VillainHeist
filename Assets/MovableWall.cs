using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableWall : MonoBehaviour
{
    public int movingUp;
    private float topLimit;
    private bool atTopLimit;
    private float thisMuch;
    [SerializeField]
    private float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        thisMuch = 3;
        atTopLimit = false;
        movingUp = 0;
        topLimit = transform.position.y + thisMuch;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p"))
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
