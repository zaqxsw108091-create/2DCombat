using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    [SerializeField] private Transform attackTransform;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private LayerMask attackableLayer;
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private float attackCD = 0.15f;


    RaycastHit2D[] hits;
    Animator anim;
    private float attackCoolTimeCheck;

    public bool ShouldBeDamage {  get;  set; }
    private List<Idamageable> idamageables = new List<Idamageable>();

    public float AttackCD => attackCD;

    private void Start()
    {
        
        anim = GetComponent<Animator>();    
    }

    private void Update()
    {
        if(InputUser.Instance.control.Attack.MeleeAttack.WasPressedThisFrame() && attackCoolTimeCheck >= attackCD)
        {
            attackCoolTimeCheck = 0;    

            anim.SetTrigger("attack");
        }
        attackCoolTimeCheck += Time.deltaTime;
        
    }
    private void Attack()
    {
        hits = Physics2D.CircleCastAll(attackTransform.position, attackRange, transform.right, 0, attackableLayer);

        for (int i = 0; i < hits.Length; i++)
        {
            Idamageable enemyHealth = hits[i].collider.GetComponent<Idamageable>();

            if (enemyHealth != null)
            {
                enemyHealth.Damage(damageAmount, transform.right);
            }
        }

    }

    public IEnumerator AttackAvailable()
    {
        ShouldBeDamage = true;

        while (ShouldBeDamage)
        {
            hits = Physics2D.CircleCastAll(attackTransform.position, attackRange, transform.right, 0, attackableLayer);

            for (int i = 0; i < hits.Length; i++)
            {
                Idamageable enemyHealth = hits[i].collider.GetComponent<Idamageable>();
                
                if (enemyHealth != null && !enemyHealth.HasTakenDamage)
                {
                    enemyHealth.Damage(damageAmount, transform.right);
                    idamageables.Add(enemyHealth);
                }
            }
            yield return null; 
        }

        ReturnAttackableState();
    }
    private void ReturnAttackableState()
    {
        foreach(var damagable in idamageables)
        {
            damagable.HasTakenDamage = false;
        }
        idamageables.Clear();
    }

    public void ShoulBeDamagetrue()
    {
        ShouldBeDamage = true;
    }

    public void ShouldBeDamageFalse()
    {
        ShouldBeDamage = false;   
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackTransform.position, attackRange);   

        
    }
}
