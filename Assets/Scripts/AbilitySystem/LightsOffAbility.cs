using System.Collections;
using System.Collections.Generic;
using Lighting_Scripts;
using UnityEngine;

public class LightsOffAbility : SelectableObjectAbility
{
    [SerializeField] private string name = "Lights Off Ability";
    [SerializeField] private Sprite icon;
    [SerializeField] private string tag = "Selectable";

    public override AbilityInputs.AbilityType abilityType1stPerson()
    {
        return AbilityInputs.AbilityType.Shootable;
    }

    public override AbilityInputs.AbilityType abilityType3rdPerson()
    {
        return AbilityInputs.AbilityType.Clickable;
    }

    public override void ApplyTo(GameObject spot)
    {
        OvalLightController spotsLight;
        if (spot.TryGetComponent(out spotsLight))
        {
            spotsLight.ToggleLight();
        }
    }

    public override Sprite GetIcon()
    {
        return icon;
    }

    public override string GetName()
    {
        return name;
    }

    public override string selectableTag()
    {
        return tag;
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
