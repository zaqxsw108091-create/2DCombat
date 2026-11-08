using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotbar : Storage
{
    public Transform player;

    // (slot index, cooldown seconds) - raised whenever a skill is used from the hotbar
    public static event Action<int, float> OnSkillCast;

    private readonly Dictionary<int, float> readyTime = new Dictionary<int, float>();

    private void Update()
    {
        HotKey();
    }

    private void HotKey()
    {
        KeyCode[] hotkeys = { KeyCode.Q, KeyCode.W };
        for (int i = 0; i < hotkeys.Length; i++)
        {
            if (Input.GetKeyDown(hotkeys[i]))
            {
                TryCast(i);
            }
        }
    }

    // Uses the skill in hotbar slot "index". Returns false when it cannot be used right now.
    public bool TryCast(int index)
    {
        if (Time.timeScale == 0f) return false;                      // menu is open (game paused)
        if (index < 0 || index >= items.Count) return false;
        Item item = items[index];
        if (item == null || item.SkillData == null) return false;   // empty slot / item without a skill
        float ready;
        if (readyTime.TryGetValue(index, out ready) && Time.time < ready) return false; // still cooling down

        BaseStrategy skill = item.SkillData;
        skill.CastSpell(player);
        readyTime[index] = Time.time + skill.Cooldown;
        OnSkillCast?.Invoke(index, skill.Cooldown);
        return true;
    }

    public int IndexOfSlot(UISlot slot) => slots.IndexOf(slot);
}
