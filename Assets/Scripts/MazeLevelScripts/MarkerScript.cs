using System;
using System.Collections;
using System.Collections.Generic;
using movement_and_Camera_Scripts;
using UnityEngine;
using UnityEngine.UIElements;

public class MarkerScript : MonoBehaviour
{
    public bool canFlip;
    public GameObject player;
    
    [SerializeField]
    private float pickUpDistance;

    public string keyActivator;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Villain_Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position + Vector3.down, player.transform.position) < pickUpDistance)
        {
            canFlip = true;
            GetComponent<Renderer>().enabled = false;
        }
    }
}
