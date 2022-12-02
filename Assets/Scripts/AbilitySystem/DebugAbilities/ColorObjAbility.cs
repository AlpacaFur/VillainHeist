using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorObjAbility : MonoBehaviour, Ability
{
    [SerializeField] private string name = "Color Object";
    [SerializeField] private Sprite icon;
    [SerializeField] private Material newMat;

    public AbilityInputs.AbilityTarget abilityTarget()
    {
        return AbilityInputs.AbilityTarget.Object;
    }

    public AbilityInputs.AbilityType abilityType1stPerson()
    {
        return AbilityInputs.AbilityType.Shootable;
    }

    public AbilityInputs.AbilityType abilityType3rdPerson()
    {
        return AbilityInputs.AbilityType.Clickable;
    }

    public string appliedToTag()
    {
        return "Untagged";
    }

    public void ApplyTo(GameObject spot)
    {
        spot.GetComponent<Renderer>().material = newMat;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public string GetName()
    {
        return name;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
