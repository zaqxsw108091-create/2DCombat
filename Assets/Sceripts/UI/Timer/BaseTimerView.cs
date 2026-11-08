using UnityEngine;

// Cooldown cover for one slot of the bottom bar.
// - Basic attack slot: follows the melee attack (Left Ctrl) cooldown.
// - Hotbar skill slots (Q / W): follow the cooldown of the skill used from that slot.
public class BaseTimerView : TimerView
{
    public PlayerMeleeAttack baseAttack;

    private int skillSlotIndex = -1;
    public int SkillSlotIndex => skillSlotIndex;

    public override void Awake()
    {
        var hotbar = GetComponentInParent<Hotbar>(true);
        var slot = GetComponentInParent<UISlot>(true);
        if (hotbar != null && slot != null) skillSlotIndex = hotbar.IndexOfSlot(slot);

        if (baseAttack != null) duration = baseAttack.AttackCD;
        base.Awake();
    }

    protected override void Subscribe()
    {
        if (skillSlotIndex >= 0) Hotbar.OnSkillCast += OnSkillCast;
        else base.Subscribe();
    }

    protected override void OnDestroy()
    {
        Hotbar.OnSkillCast -= OnSkillCast;
        base.OnDestroy();
    }

    private void OnSkillCast(int slotIndex, float cooldown)
    {
        if (slotIndex == skillSlotIndex) AnimateCooldown(cooldown);
    }
}
