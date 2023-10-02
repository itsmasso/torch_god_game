using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Torch_Character_Script : PlayerBaseScript
{
    [SerializeField]
    private GameObject weapon;

    private SpriteRenderer weaponSprite;
    private bool spriteFlipped;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        weaponSprite = weapon.GetComponent<SpriteRenderer>();
        base.Start();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void FlipSprite()
    {
        if (velocity.x < 0)
        {
            //face left
            facingLeft = true;
            sprite.flipX = facingLeft;
            weaponSprite.flipX = facingLeft;
            if (!spriteFlipped)
            {
                weapon.transform.localPosition = new Vector2(-weapon.transform.localPosition.x, weapon.transform.localPosition.y);
                spriteFlipped = true;
            }

        }
        else if (velocity.x > 0)
        {
            facingLeft = false;
            sprite.flipX = facingLeft;
            weaponSprite.flipX = facingLeft;
            if (spriteFlipped)
            {
                weapon.transform.localPosition = new Vector2(Mathf.Abs(weapon.transform.localPosition.x), weapon.transform.localPosition.y);
                spriteFlipped = false;
            }
        }
        else
        {
            sprite.flipX = facingLeft;
            weaponSprite.flipX = facingLeft;
        }
    }

    protected override void Update()
    {
        base.Update();
    }
}
