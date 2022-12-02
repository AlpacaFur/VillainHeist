using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceABoxAbility : MonoBehaviour, Ability
{
    [SerializeField] private string name;
    [SerializeField] private Sprite icon;
    [SerializeField] private GameObject obj;
    [SerializeField] private float yRaycastPositionInc = 2f;

    public void ApplyTo(GameObject spot)
    {
        RaycastHit hit;
        Vector3 position = spot.transform.position;
        Vector3 raycastStartPos = position;
        raycastStartPos.y += yRaycastPositionInc;
        bool hitFloor = Physics.Raycast(raycastStartPos, Vector3.down, out hit);
        GameObject newObj = Instantiate(obj);
        if (hitFloor) {
            position.y = hit.transform.position.y;
            position.y += hit.collider.bounds.extents.y;
            position.y += newObj.transform.localScale.y / 2;
        }
        newObj.transform.position = position;
        newObj.transform.tag = "Respawn";
    }

    public AbilityInputs.AbilityType abilityType3rdPerson()
    {
        return AbilityInputs.AbilityType.Clickable;
    }

    public AbilityInputs.AbilityType abilityType1stPerson()
    {
        return AbilityInputs.AbilityType.Shootable;
    }

    public string GetName()
    {
        return name;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AbilityInputs.AbilityTarget abilityTarget()
    {
        return AbilityInputs.AbilityTarget.Position;
    }

    public string appliedToTag()
    {
        throw new System.NotImplementedException();
    }
}
