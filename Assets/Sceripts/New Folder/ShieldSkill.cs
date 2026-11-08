using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Shield", menuName = "Skill/Shield")]
public class ShieldSkill : BaseStrategy
{
    public GameObject shieldPrefab;
    public float duration = 5f;

    // A new shield only after the previous one is gone (shields never stack).
    public override float Cooldown => Mathf.Max(cooldown, duration);

    public override void CastSpell(Transform origin)
    {
        var shield = Instantiate(shieldPrefab,
            origin.transform.position, Quaternion.identity);

        shield.transform.SetParent(origin);

        Destroy(shield,duration);
    }
}
