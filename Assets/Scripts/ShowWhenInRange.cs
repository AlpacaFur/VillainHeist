using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowWhenInRange : MonoBehaviour
{
    [SerializeField] private GameObject showable;
    [SerializeField] private bool hideWhenNotInRange = true;
	// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            showable.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (hideWhenNotInRange && other.CompareTag("Player"))
        {
            showable.SetActive(false);
        }
    }
}
