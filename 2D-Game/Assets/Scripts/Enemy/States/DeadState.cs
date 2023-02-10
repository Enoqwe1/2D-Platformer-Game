using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State
{
    protected D_DeadState stateData;
    public DeadState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_DeadState stateData) : base(etity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        GameObject.Instantiate(stateData.deadBloodParticle, entity.aliveGO.transform.position, stateData.deadBloodParticle.transform.rotation);
        GameObject.Instantiate(stateData.deadChunkParticle, entity.aliveGO.transform.position, stateData.deadChunkParticle.transform.rotation);
        GameObject.Find("GameManager").GetComponent<GameManager>().CoinAddToScore(stateData.cash);
        GameObject.Instantiate(stateData.heart, entity.aliveGO.transform.position, stateData.heart.transform.rotation);
        entity.gameObject.SetActive(false);
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
