using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Reaper_Script : EnemyBaseScript
{
    [SerializeField]
    private float skillRadius, skillDuration, chanceToUseSkill, skillCooldown, lightChangeSpeed = 1f;

    private bool canUseSkill;

    [SerializeField]
    private Light2D light2D;

    private float originalLightIntensity;

    public static event Action onSkillCooldown;
    private float time = 0f;

    protected override void Start()
    {
        base.Start();
        canUseSkill = true;
        onSkillCooldown += ActiveSkillCooldown;
        light2D = LevelManager.Instance.lights;
        originalLightIntensity = light2D.intensity;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected IEnumerator PlaySkillAnimation()
    {
        enemyAnimations.anim.SetBool("UsingSkill", true);
        yield return new WaitForSeconds(enemyAnimations.anim.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        enemyAnimations.anim.SetBool("UsingSkill", false);
    }

    protected IEnumerator UseSkill()
    {
        canUseSkill = false;
        StartCoroutine(DimLights());
        StartCoroutine(PlaySkillAnimation());
        yield return new WaitForSeconds(skillDuration);
        StartCoroutine(BrightenLights());
        onSkillCooldown?.Invoke();
    }

    protected void ActiveSkillCooldown()
    {
        if (gameObject.activeSelf)
        {
            StartCoroutine(SkillCoolDown());
        }
    }

    protected IEnumerator SkillCoolDown()
    {
        canUseSkill = false;
        yield return new WaitForSeconds(skillCooldown);
        canUseSkill = true;
    }

    protected IEnumerator DimLights()
    {
        while (light2D.intensity > 0)
        {
            light2D.intensity -= 0.01f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    protected IEnumerator BrightenLights()
    {
        while (light2D.intensity <= originalLightIntensity)
        {
            light2D.intensity += 0.01f;
            yield return new WaitForSeconds(0.1f);
        }
    }


    protected override void Update()
    {
        base.Update();
        float randValue = UnityEngine.Random.value;
        if(Vector2.Distance(player.transform.position, transform.position) >= skillRadius && randValue < chanceToUseSkill)
        {
            if (canUseSkill)
            {
                StartCoroutine(UseSkill());
            }
        }

        
    }

    private void OnDestroy()
    {
        onSkillCooldown -= ActiveSkillCooldown;
    }


}
