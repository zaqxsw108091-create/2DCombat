using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GrassHealth : BaseHealth
{
    public override void Damage(int amount, Vector2 attackDirection)
    {
        HasTakenDamage = true;
        CurrentHealth -= amount;
        
        // sound
        //SoundManager.Instance.PlaySFXClip(damageClip, 1f);
        PlayRandomSFX();
        SoundManager.Instance.PlaySFXFromString("HurtClipName", 1f);
        //Effect
        SpawnDamageParticle(attackDirection);
        Die();
    }

}
