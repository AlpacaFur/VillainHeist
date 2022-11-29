using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using movement_and_Camera_Scripts;

public class FirstPersonOnlyObj : MonoBehaviour
{
    private PlayerController player;
    private bool show = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        showInFirstPerson();
        gameObject.SetActive(show); 
    }

    private IEnumerator showInFirstPerson()
    {
        yield return new WaitUntil(() => player.isFirstPov);
        show = true;
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
