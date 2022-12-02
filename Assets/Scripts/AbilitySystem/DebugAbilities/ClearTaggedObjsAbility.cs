using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearTaggedObjsAbility : MonoBehaviour, Ability
{
    [SerializeField] private string name = "ClearTaggedObjsAbility";
    [SerializeField] private Sprite icon;
    [SerializeField] private string tag;

    public AbilityInputs.AbilityType abilityType3rdPerson()
    {
        return AbilityInputs.AbilityType.Immediate;
    }

    public AbilityInputs.AbilityType abilityType1stPerson()
    {
        return AbilityInputs.AbilityType.Immediate;
    }

    public AbilityInputs.AbilityTarget abilityTarget()
    {
        return AbilityInputs.AbilityTarget.Position;
    }

    public void ApplyTo(GameObject spot)
    {
        GameObject[] taggedObjs = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in taggedObjs)
        {
            Destroy(obj);
        }
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

    public string appliedToTag()
    {
        throw new System.NotImplementedException();
    }
}
