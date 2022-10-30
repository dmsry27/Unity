using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponNotShow : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.Inst.Player.ShowWeaponAndShield(false);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.Inst.Player.ShowWeaponAndShield(true);

    }
}
