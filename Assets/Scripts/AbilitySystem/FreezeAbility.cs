using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeAbility : SelectableObjectAbility
{
    public Sprite icon;
    public string name = "Freeze";
    public string freezeTag = "";
    public AbilityInputs.AbilityTarget abilityTarget()
    {
        return AbilityInputs.AbilityTarget.Object;
    }

    public override AbilityInputs.AbilityType abilityType1stPerson()
    {
        return AbilityInputs.AbilityType.Shootable;
    }

    public override AbilityInputs.AbilityType abilityType3rdPerson()
    {
        return AbilityInputs.AbilityType.Clickable;
    }

    public override string appliedToTag()
    {
        return "Freezable";
    }

    public override void ApplyTo(GameObject spot)
    {
        FreezableObject freezable;
        if (spot.TryGetComponent<FreezableObject>(out freezable))
        {
            freezable.ToggleFreeze();
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
        return freezeTag;
    }
}
