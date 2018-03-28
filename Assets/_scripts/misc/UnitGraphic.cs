using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ericson;


public class UnitGraphic : MonoBehaviour {

    Animator animator = null;
    SpriteRenderer sprite = null;
    private bool isAnimating = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }
    public void Walk()
    {
        if (animator == null || !animator.gameObject.activeInHierarchy) return;
        animator.SetBool("isWalking", true);
        animator.SetBool("isIdle", false);
        animator.SetBool("isAttacking", false);
    }
    public void Idle()
    {
        if (animator == null || !animator.gameObject.activeInHierarchy || isAnimating) return;
        animator.SetBool("isIdle", true);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isWalking", false);
    }
    public void Attack()
    {
        if (animator == null || !animator.gameObject.activeInHierarchy) return;
        isAnimating = true;
        animator.SetBool("isAttacking", true);
        animator.SetBool("isWalking", false);
        animator.SetBool("isIdle", false);
    }

    
    public void Sleep()
    {
        if (isAnimating) return;
        Color newColor = Color.white * 0.25f;
        newColor.a = 1;
        ChangeColor(newColor);
    }
    public void WakeUp()
    {
        Color newColor = Color.white;
        ChangeColor(newColor);
    }
    public void ChangeColor(Color newColor)
    {
        if(sprite == null) sprite = GetComponent<SpriteRenderer>();
        sprite.color = newColor;
    }
    public void DoneAnimating()
    {
        isAnimating = false;
        GameScene.ins.ConfirmAttack();
    }
    public void FlipSprite()
    {
        if (sprite == null) sprite = GetComponent<SpriteRenderer>();
        sprite.flipX = true;
    }
    public void JuiceCamera(float juice)
    {
        Camera.main.gameObject.GetComponent<eCameraJuice2D>().AddJuice(juice);
    }
}
