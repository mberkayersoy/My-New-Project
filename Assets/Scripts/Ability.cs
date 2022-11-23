using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Ability/Make New Ability", order = 0)]
public class Ability : ScriptableObject
{
    [SerializeField] GameObject equippedPrefab = null;
}
