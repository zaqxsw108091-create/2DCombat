using System.Collections;
using CustomUtility;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// Cooldown cover drawn over a slot icon in the bottom bar.
public class TimerView : MonoBehaviour
{
    [SerializeField] Image cooldownBG;
    [SerializeField] TextMeshProUGUI cooldownText;

    public float duration;

    private Tween cooldownTween;
    private Coroutine textRoutine;
    private InputUser subscribedInput;

    public virtual void Awake()
    {
        // The icon is visible from the start; the cover only appears while cooling down.
        ResetView();
        Subscribe();
    }

    // Default: follow the basic melee attack (Left Ctrl).
    protected virtual void Subscribe()
    {
        subscribedInput = InputUser.Instance;
        subscribedInput.BaseAttack += AnimateCooldown;
    }

    protected virtual void OnDestroy()
    {
        if (subscribedInput != null) subscribedInput.BaseAttack -= AnimateCooldown;
        cooldownTween?.Kill();
    }

    protected void ResetView()
    {
        if (cooldownBG != null) cooldownBG.fillAmount = 0f;
        if (cooldownText != null) cooldownText.gameObject.SetActive(false);
    }

    public void AnimateCooldown()
    {
        AnimateCooldown(duration);
    }

    public void AnimateCooldown(float seconds)
    {
        if (seconds <= 0f || cooldownBG == null) return;
        if (cooldownTween != null && cooldownTween.IsActive()) return; // already cooling down

        cooldownBG.fillAmount = 1;
        cooldownTween = cooldownBG.DOFillAmount(0, seconds)
            .OnComplete(() => cooldownTween = null);

        if (textRoutine != null) StopCoroutine(textRoutine);
        textRoutine = StartCoroutine(TextCoolTime(seconds));
    }

    public IEnumerator TextCoolTime(float seconds)
    {
        if (cooldownText == null) yield break;
        // Cooldowns shorter than 1 second only show the cover, no number.
        if (seconds < 1f)
        {
            cooldownText.gameObject.SetActive(false);
            yield break;
        }

        float remain = seconds;
        cooldownText.gameObject.SetActive(true);
        while (remain > 0)
        {
            cooldownText.text = Mathf.CeilToInt(remain).ToString();
            yield return new WaitForSeconds(Mathf.Min(1f, remain));
            remain -= 1f;
        }
        cooldownText.gameObject.SetActive(false);
        textRoutine = null;
    }
}
