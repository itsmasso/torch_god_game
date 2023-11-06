using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

//make this into abstract when needing more exclusive animations
public class EnemyAnimations : MonoBehaviour
{
    [Header("Animation Properties")]
    [SerializeField] protected Material flashMaterial;
    [SerializeField] protected Material originalMaterial;
    [SerializeField] protected float flashDuration = 0.2f;
    public Animator anim;
    [SerializeField] protected SpriteRenderer sprite;

    private Coroutine flashRoutine;

    private void Start()
    {
        sprite.material = originalMaterial;
    }

    private void OnEnable()
    {
        sprite.material = originalMaterial;
    }
    private IEnumerator _PlayAttackAnimation()
    {
        anim.SetBool("IsAttacking", true);
        yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        anim.SetBool("IsAttacking", false);
    }
    public void PlayAttackAnimation()
    {
        if(anim != null)
        {
            StartCoroutine(_PlayAttackAnimation());
        }
    }

    private IEnumerator FlashEffect()
    {
        //method for damage flash effect by changing materials for a brief second
        sprite.material = flashMaterial;
        yield return new WaitForSeconds(flashDuration);
        sprite.material = originalMaterial;
        flashRoutine = null;
    }

    public void PlayFlashEffect()
    {
        //knockback and animation
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(FlashEffect());
    }
}
