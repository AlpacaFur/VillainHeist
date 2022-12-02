using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using movement_and_Camera_Scripts;

public class HideShowEscape : MonoBehaviour
{
    [SerializeField] private bool showInEscape;
    private PlayerController player;
    // Start is called before the first frame update
    void Awake()
    {
        gameObject.SetActive(!showInEscape);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.StartCoroutine(hideShowOnEscape());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator hideShowOnEscape()
    {
        yield return new WaitUntil(() => player.isFirstPov);
        gameObject.SetActive(showInEscape);
    }
}
