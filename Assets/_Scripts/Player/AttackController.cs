using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [Header("Attack")]
    public float attackTime; // For how long the attack lasts
    public bool isAttacking; // Is the player attacking?
    public Animator PlayerAnimator;

    private RailMovement _railMovement;

    void Start()
    {
        _railMovement = GetComponent<RailMovement>();
        PlayerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        InputController();
    }

    private void InputController()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _railMovement.IsGrounded)
        {
            StopCoroutine(Attack());
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        PlayerAnimator.SetTrigger("Attack");
        isAttacking = true;

        this.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        yield return new WaitForSeconds(attackTime);
        
        isAttacking = false;
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
