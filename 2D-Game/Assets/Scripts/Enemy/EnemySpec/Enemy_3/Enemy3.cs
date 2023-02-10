using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : Entity
{
    public E3_IdleState idleState { get; private set; }
    public E3_MoveState moveState { get; private set; }
    public E3_PlayerDetected playerDetectedState { get; private set; }
    public E3_LookForPlayer lookForPlayerState { get; private set; }
    public E3_MeleeAttackState meleeAttackState { get; private set; }
    public E3_PlayerDetectedAndWalk playerDetectedAndWalk { get; private set; }
    public E3_DeadState deadState { get; private set; }

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
    private Transform meleeAttackPosition;
    public override void Start()
    {
        base.Start();

        moveState = new E3_MoveState(this, stateMachine, "move", moveStateData, this);
        idleState = new E3_IdleState(this, stateMachine, "idle", idleStateData, this);
        playerDetectedState = new E3_PlayerDetected(this, stateMachine, "playerDetected", playerDetectedData, this);
        lookForPlayerState = new E3_LookForPlayer(this, stateMachine, "lookForPlayer", lookForPlayerData, this);
        meleeAttackState = new E3_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        playerDetectedAndWalk = new E3_PlayerDetectedAndWalk(this, stateMachine, "playerDetectedAndWalk", playerDetectedAndWalkData, this);
        deadState = new E3_DeadState(this, stateMachine, "dead", deadStateData, this);
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

        if (isDead)
        {
            stateMachine.ChangeState(deadState);
        }
    }
}
