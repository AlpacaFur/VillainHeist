using System.Collections;
using System.Collections.Generic;
using AbilitySystem;
using movement_and_Camera_Scripts;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class FirstPersonOnlyBattery : MonoBehaviour
{
    private GameObject player;

    [SerializeField]
    private bool active;
    private Renderer rend;
    [SerializeField]
    private BatteryPickupItem mainScript;
    private float realDistance;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Villain_Player");
        rend = GetComponent<Renderer>();
        mainScript = GetComponent<BatteryPickupItem>();
        realDistance = mainScript.killDistance;
        mainScript.killDistance = 0;
        mainScript.killGuy = player;
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
        {
            active = player.GetComponent<PlayerController>().isFirstPov;
            rend.enabled = active;
            if (active)
            {
                mainScript.killDistance = realDistance;
            }
        }
    } 
}

