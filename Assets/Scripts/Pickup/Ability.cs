using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability 
{
    public float abilityTime = 45f;
    public GameObject abilityPrefab;
    protected Player owner;

    public abstract void GiveAbility(Player player);
    public abstract void TakeAbility();

}
