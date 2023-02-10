using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E3_HurtState : HurtState
{
    private Enemy3 enemy;
    public E3_HurtState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_HurtState stateData, Enemy3 enemy) : base(etity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
