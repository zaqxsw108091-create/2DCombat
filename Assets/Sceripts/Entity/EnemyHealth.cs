using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(CinemachineImpulseSource))]
public class EnemyHealth : BaseHealth, Idamageable
{
    // Ã¤·Â

    

    private CinemachineImpulseSource _impulseSource;
    [SerializeField] private ScreenShakeSO profile;

    HPBar _hpBar;



   protected override void Start()
    {
        base.Start();

        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _hpBar = GetComponentInChildren<HPBar>();   
    }
    public override void Damage(int amount, Vector2 attackDirection)
    {
        HasTakenDamage = true;
        CurrentHealth -= amount;
        _hpBar.UpdateHPBar(MaxHealth,CurrentHealth);

        CameraShakeManager.Instance.CameraShakeFromProfile(_impulseSource, profile);
        // sound
        //SoundManager.Instance.PlaySFXClip(damageClip, 1f);
        PlayRandomSFX();
        SoundManager.Instance.PlaySFXFromString("HurtClipName", 1f);
        //Effect
        SpawnDamageParticle(attackDirection);

        Die();
    }
}
