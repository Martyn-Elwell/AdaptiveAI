using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    Attack,
    Defence,
    Idle
}

[CreateAssetMenu(fileName = "New Action", menuName = "Combat/Action")]
public class Actions : ScriptableObject
{
    public int ID;
    public string Name;
    public string Description;
    public float animationTime;
    public float range;
    public Type type;
    public List<Actions> counters;
    public List<Actions> defences;
}
