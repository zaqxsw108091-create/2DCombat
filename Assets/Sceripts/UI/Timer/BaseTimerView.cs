using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTimerView : TimerView
{
    public PlayerMeleeAttack baseAttack;

    public override void Awake()
    {
        base.Awake();
        duration = baseAttack.AttackCD;
    }
}
