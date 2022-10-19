using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPickupItem : MonoBehaviour
{

    [Tooltip("Insert Battery Storage Object")]
    public Battery bat;

    [Tooltip("The Player")]
    public GameObject killGuy;

    [SerializeField]
    [Tooltip("Distance Until the item is picked up")]
    private float killDistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnDestroy()
    {
        this.bat.addToCurrent(2);
    }

    // Update is called once per frame
    void Update()
    {
        if ( Vector3.Distance(killGuy.transform.position, this.transform.position) < this.killDistance)
        {
            Destroy(this.gameObject);
        }
    }
}