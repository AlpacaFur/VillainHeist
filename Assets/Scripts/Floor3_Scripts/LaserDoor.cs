using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using movement_and_Camera_Scripts;

public class LaserDoor : MonoBehaviour
{
    [SerializeField] private GameObject openObj;
    [SerializeField] private GameObject closedObj;
    private bool isOpen;
    [SerializeField] private bool isOpenTopDownStart = false;
    [SerializeField] private bool isOpenFirstPersonStart = false;
    private Collider closedCollider;
    private PlayerController player;

    private AudioSource LaserDoorAS1;

    // Start is called before the first frame update
    void Start()
    {
        closedCollider = GetComponentInChildren<Collider>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (player.isFirstPov)
        {
            isOpen = isOpenFirstPersonStart;
        }
        else
        {
            isOpen = isOpenTopDownStart;
            StartCoroutine(changeInFirstPerson());
        }
        setOpen(isOpen);

        LaserDoorAS1 = gameObject.GetComponent<AudioSource>();
    }

    private IEnumerator changeInFirstPerson()
    {
        yield return new WaitUntil(() => player.isFirstPov);
        setOpen(isOpenFirstPersonStart);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setOpen(bool open)
    {
        isOpen = open;
        closedCollider.enabled = !open;
        openObj.SetActive(open);
        closedObj.SetActive(!open);
    }

    public bool open()
    {
        return isOpen;
    }

    private void PlayOpenSound()
    {
        LaserDoorAS1.Play();
    }
}
