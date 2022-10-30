using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIdleSelector : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)    // Exit로 나갈때 실행되는 함수
    {
        //if(!animator.IsInTransition(0))       // 0번째 레이어가 트랜지션 중인지 아닌지 확인할때 사용 (Exit 타이밍이 아닌듯)
        animator.SetInteger("IdleSelect", RandomSelect());
    }

    int RandomSelect()
    {
        float num = Random.Range(0.0f, 1.0f);
        int select;

        if (num < 0.7f)
        {
            select = 0;
        }
        else if(num < 0.8f)
        {
            select = 1;
        }
        else if(num < 0.87f)
        {
            select = 2;
        }
        else if(num < 0.94f)
        {
            select = 3;
        }
        else
        {
            select = 4;
        }

        return select;
    }
}
