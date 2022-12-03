using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    [SerializeField] 
    private WallMoveable script;

    private GameObject marker;

    [SerializeField] [Tooltip("Name of Marker")]
    private string markName;

// Start is called before the first frame update
    void Start()
    {
        script = this.GetComponent<WallMoveable>();
        script.enabled = false;
        marker = GameObject.Find(markName);
    }

    // Update is called once per frame
    void Update()
    {
        if (marker.GetComponent<MarkerScript>().canFlip)
        {
            script.enabled = true;
        }
    }
}
