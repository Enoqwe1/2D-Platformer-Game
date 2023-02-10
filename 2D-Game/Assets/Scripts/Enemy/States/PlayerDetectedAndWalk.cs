using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectedAndWalk : State
{
    protected D_PlayerDetectedAndWalk stateData;
    protected bool isDetectingWall;
    protected bool isDetectingLedge;
    protected bool isPlayerInMinAgroRange;
    protected bool performCloseRangeAction;
    protected bool isWalkTimeOver;
    public PlayerDetectedAndWalk(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetectedAndWalk stateData) : base(etity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isDetectingLedge = entity.CheckLedge();
        isDetectingWall = entity.CheckWall();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
    }

    public override void Enter()
    {
        base.Enter();
        isWalkTimeOver = false;

        entity.SetVelocity(stateData.movementSpeed);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Time.time >= startTime + stateData.walktime)
        {
            isWalkTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
