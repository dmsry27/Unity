using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{

    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        GameManager.Inst.Player.ShowWeaponAndShield(true);
        GameManager.Inst.Player.WeaponEffectSwitch(true);
    }

    // SubStateMachine이 Exit로 갈 때 실행 (연결 필요)
    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        GameManager.Inst.Player.WeaponEffectSwitch(false);
        //Debug.Log($"{animator.GetInteger("ComboState")}");
        animator.SetInteger("ComboState", 0);
        animator.ResetTrigger("Attack");
    }
}
