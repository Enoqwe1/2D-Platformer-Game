using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Entity
{
    public E2_IdleState idleState { get; private set; }
    public E2_MoveState moveState { get; private set; }
    public E2_PlayerDetected playerDetectedState { get; private set; }
    public E2_LookForPlayer lookForPlayerState { get; private set; }
    public E2_MeleeAttackState meleeAttackState { get; private set; }
    public E2_PlayerDetectedAndWalk playerDetectedAndWalk { get; private set; }
    public E2_DeadState deadState { get; private set; }
    public E2_HurtState hurtState { get; private set; }

    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_PlayerDetected playerDetectedData;
    [SerializeField]
    private D_LookForPlayer lookForPlayerData;
    [SerializeField]
    private D_MeleeAttack meleeAttackStateData;
    [SerializeField]
    private D_PlayerDetectedAndWalk playerDetectedAndWalkData;
    [SerializeField]
    private D_DeadState deadStateData;
    [SerializeField]
    private D_HurtState hurtStateData;


    [SerializeField]
    private Transform meleeAttackPosition;
    public override void Start()
    {
        base.Start();

        moveState = new E2_MoveState(this, stateMachine, "move", moveStateData, this);
        idleState = new E2_IdleState(this, stateMachine, "idle", idleStateData, this);
        playerDetectedState = new E2_PlayerDetected(this, stateMachine, "playerDetected", playerDetectedData, this);
        lookForPlayerState = new E2_LookForPlayer(this, stateMachine, "lookForPlayer", lookForPlayerData, this);
        meleeAttackState = new E2_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        playerDetectedAndWalk = new E2_PlayerDetectedAndWalk(this, stateMachine, "playerDetectedAndWalk", playerDetectedAndWalkData, this);
        deadState = new E2_DeadState(this, stateMachine, "dead", deadStateData, this);
        hurtState = new E2_HurtState(this, stateMachine, "hurt", hurtStateData, this);
        stateMachine.Initialize(moveState);
    }
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }
    public override void Damage(AttackDetails attackDetails)
    {
        base.Damage(attackDetails);
        if (isHurt)
        {
            stateMachine.ChangeState(hurtState);
        }
        else if (isDead)
        {
            stateMachine.ChangeState(deadState);
        }
    }
}
