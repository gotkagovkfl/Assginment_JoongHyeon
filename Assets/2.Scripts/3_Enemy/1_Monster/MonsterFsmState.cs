using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region FsmState


public abstract class MonsterFsmState
{
    protected Monster monster;

    public MonsterFsmState(Monster monster)
    {
        this.monster = monster;
    }
    public abstract MonsterState targetState {get;}
    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit(); 
}


public class MFS_Idle  : MonsterFsmState
{
    public MFS_Idle(Monster monster) : base(monster)
    {
    }

    public override MonsterState targetState => MonsterState.Move;

    public override void Enter()
    {
        monster.SetIdle();
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        
    }
}

public class MFS_Move : MonsterFsmState
{
    public MFS_Move(Monster monster) : base(monster)
    {
    }

    public override MonsterState targetState => MonsterState.Move;

    public override void Enter()
    {
        monster.StartMove();
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        monster.Move();
    }
}

public class MFS_Jump : MonsterFsmState
{
    public MFS_Jump(Monster monster) : base(monster)
    {
    }

    public override MonsterState targetState => MonsterState.Jump;
    public override void Enter()
    {
        monster.Jump();
    }

    public override void Exit()
    {
        monster.FinisihJump();
    }

    public override void Update()
    {

    }

    //================
    


}


public class MFS_Attack : MonsterFsmState
{
    public MFS_Attack(Monster monster) : base(monster)
    {
    }

    public override MonsterState targetState => MonsterState.Attack;
    public override void Enter()
    {
        monster.StartAttack();
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        
    }
}


public class MFS_Die : MonsterFsmState
{
    public MFS_Die(Monster monster) : base(monster)
    {
    }

    public override MonsterState targetState => MonsterState.Die;
    public override void Enter()
    {
        monster.OnDie();
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        
    }
}

#endregion
