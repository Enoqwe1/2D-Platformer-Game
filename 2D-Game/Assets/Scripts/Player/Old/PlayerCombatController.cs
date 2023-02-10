using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField]
    private bool combatEnabled;
    [SerializeField]
    private Transform attack1HitBoxPos;
    [SerializeField]
    private LayerMask whatIsDamageable;
    [SerializeField]
    private float inputTimer, attack1Radius, attack1Damage;

    private bool gotInput, isAttacking, isFirstAttack;
    private float lastInputTime = Mathf.NegativeInfinity;
    private GameManager GM;
    private AttackDetails attackDetails;
    private Animator anim;
    private Bandit player;
    private PlayerStats Ps;
    private Quiz quiz;
    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("canAttack", combatEnabled);
        player = GetComponent<Bandit>();
        Ps = GetComponent<PlayerStats>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        quiz = FindObjectOfType<Quiz>();
    }
    private void Update()
    {
        CheckCombatInput();
        CheckAttacks();
    }
    private void CheckCombatInput()
    {
        if (Input.GetButtonDown("Fire1") && quiz.canPlayerMove)
        {
            if (combatEnabled)
            {
                gotInput = true;
                lastInputTime = Time.time;
            }
        }
    }
    private void CheckAttacks()
    {
        if (gotInput)
        {
            if (!isAttacking)
            {
                gotInput = false;
                isAttacking = true;
                isFirstAttack = !isFirstAttack;
                anim.SetBool("attack1", true);
                anim.SetBool("firstAttack", isFirstAttack);
                anim.SetBool("isAttacking", isAttacking);
            }
        }
        if (Time.time >= lastInputTime + inputTimer)
        {
            gotInput = false;
        }
    }
    private void CheckAttackHitBox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attack1HitBoxPos.position, attack1Radius, whatIsDamageable);

        attackDetails.damageAmount = attack1Damage;
        attackDetails.position = transform.position;

        foreach (Collider2D collider in detectedObjects)
        {
            collider.transform.parent.SendMessage("Damage", attackDetails);
        }
    }
    private void FinishAttack1()
    {

        isAttacking = false;
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("attack1", false);
    }
    private void Damage(AttackDetails attackDetails)
    {
        if (!player.GetDashStatus())
        {
            int direction;
            Ps.DecreaseHealth((int)attackDetails.damageAmount);

            if (attackDetails.position.x < transform.position.x)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }
            player.Knockback(direction);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Deadzone")
        {
            Ps.DecreaseHealth(Ps.currentHealth);
            //GM.FallDown();
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Hazards")
        {
            Ps.DecreaseHealth(4.0f);
            int direction;
            if (other.transform.position.x < transform.position.x)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }
            player.Knockback(direction);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attack1HitBoxPos.position, attack1Radius);
    }
}
