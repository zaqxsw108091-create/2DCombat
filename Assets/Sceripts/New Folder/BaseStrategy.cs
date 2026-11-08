using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStrategy : ScriptableObject
{
    [Tooltip("Seconds before this skill can be used again")]
    public float cooldown = 1f;

    public virtual float Cooldown => cooldown;

    public abstract void CastSpell(Transform origin);
}
