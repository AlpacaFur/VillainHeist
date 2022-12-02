using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using movement_and_Camera_Scripts;

public class AbilityController : MonoBehaviour
{
	private PlayerController player;
    [Header("Ability Start")]
    [SerializeField]
    private string buttonToActivate = "Fire3";
    [SerializeField] private float maxRange = 3;
    [Header("Abilities")]
    private Ability[] abilities;
    public enum AbilityType { Clickable, Immediate, Shootable };
    public enum AbilityTarget { Position, Object }
    [Header("Ability Selection")]
    private GameObject curSelectedObj;
    private Material[] curSelectedObjPrevMats;
    [SerializeField] private Material highlightMat;
    private Dictionary<string, Ability> interactibleTagToAbility;
    [Header("Shootable Selection")]
    private bool selectedShootInput;
    private bool useShootable;
    [SerializeField]
    private GameObject shootableCanvas;
    [Header("Battery")]
    [Tooltip("Insert Battery Storage Object")]
    public Battery bat;
    public int cost = 1;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        ResetAbilityComponents();
    }

    // Update is called once per frame
    void Update()
    {
        if (curSelectedObj != null)
        {
            Debug.Log("selectedObjName = " + curSelectedObj.name);
        }
        else
        {
            Debug.Log("selectedObjName = null");
        }

        updateHighlightedObj();
        checkInteract();
    }

    private void updateHighlightedObj()
    {
        if (curSelectedObj != null)
        {
            curSelectedObj.GetComponentInChildren<Renderer>().materials = curSelectedObjPrevMats;
        }

        curSelectedObj = getNearestInteractibleObj();

        if (curSelectedObj != null)
        {
            curSelectedObjPrevMats = curSelectedObj.GetComponentInChildren<Renderer>().materials;
            int numOfMats = curSelectedObj.GetComponentInChildren<Renderer>().materials.Length;
            /*for (int i = 0; i < numOfMats; i++)
            {
                curSelectedObj.GetComponentInChildren<Renderer>().material = highlightMat;
            }*/
            curSelectedObj.GetComponentInChildren<Renderer>().material = highlightMat;
        }
        
    }

    private bool isInteractible(Collider c)
    {
        List<string> interactibleObjTags = new List<string>(interactibleTagToAbility.Keys);
        foreach (string tag in interactibleObjTags)
        {
            if (c.CompareTag(tag))
            {
                return true;
            }
        }
        return false;
    }

    private GameObject getNearestInteractibleObj()
    {
        Vector3 playerPos = player.transform.position;
        Collider[] nearestColliders = Physics.OverlapSphere(playerPos, maxRange);
        Collider closest = null;
        foreach (Collider c in nearestColliders)
        {
            //Debug.Log("c = " + c.name);
            if (closest == null && isInteractible(c))
            {
                closest = c;
            }
            else
            {
                if (isInteractible(c) && (c.transform.position - playerPos).magnitude < (closest.transform.position - playerPos).magnitude)
                {
                    closest = c;
                }
            }
        }
        if (closest != null)
        {
            return closest.gameObject;
        }
        else
        {
            return null;
        }
    }

    private void ResetAbilityComponents()
    {
        abilities = GetComponents<Ability>();
        interactibleTagToAbility = new Dictionary<string, Ability>();
        foreach (Ability a in abilities)
        {
            interactibleTagToAbility.Add(a.appliedToTag(), a);
        }
    }

    private void checkInteract()
    {
        if ((curSelectedObj != null) && Input.GetButtonDown(buttonToActivate) && bat.hasLeft(cost))
        {
            interactibleTagToAbility[curSelectedObj.tag].ApplyTo(curSelectedObj);
            bat.subFromCurrent(cost);
        }
    }
}
