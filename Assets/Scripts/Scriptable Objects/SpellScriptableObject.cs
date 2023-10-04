using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "ScriptableObjects/Spell", order = 1)]
public class SpellScriptableObject : ScriptableObject
{
    public Spells spellName;
    public int manaCost;
    public float coolDownInSeconds;
    public string tip;

}