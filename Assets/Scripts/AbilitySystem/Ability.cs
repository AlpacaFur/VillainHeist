using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Ability
{
	/**
     * Where this Ability is to be used.
     */
	public void ApplyTo(GameObject spot);

	public AbilityInputs.AbilityType abilityType3rdPerson();

	public AbilityInputs.AbilityType abilityType1stPerson();

	public AbilityInputs.AbilityTarget abilityTarget();

	public string GetName();

	public Sprite GetIcon();

	public string appliedToTag();
}

public abstract class SelectableObjectAbility : MonoBehaviour, Ability
{
	public AbilityInputs.AbilityTarget abilityTarget()
	{
		return AbilityInputs.AbilityTarget.Object;
	}

	public abstract string selectableTag();

	public abstract AbilityInputs.AbilityType abilityType1stPerson();

	public abstract AbilityInputs.AbilityType abilityType3rdPerson();

	public abstract void ApplyTo(GameObject spot);

	public abstract Sprite GetIcon();

	public abstract string GetName();

    public abstract string appliedToTag();
}