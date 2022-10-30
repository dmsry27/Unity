using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_HP_Bar : MonoBehaviour
{
    Transform fill;

    private void Awake()
    {
        fill = transform.GetChild(1);
        IHealth target = GetComponentInParent<IHealth>();
        target.onHealthChange += Refresh;
    }

    private void Refresh(float ratio)
    {
        fill.localScale = new Vector3(ratio, 1, 1);
    }

    /// <summary>
    /// 모든 게임 오브젝트의 Update함수가 호출된 이후에 호출되는 Update함수 (카메라 관련은 이곳에서 처리)
    /// </summary>
    private void LateUpdate()
    {
        // 빌보드  
        //transform.forward = Camera.main.transform.forward;        
        transform.rotation = Camera.main.transform.rotation;
    }
}
