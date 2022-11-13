using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.InputSystem;

public class TempItemSlotUI : ItemSlotUI
{
    private void Update()
    {
        // 오브젝트의 transform이 스크린좌표가 적용되있기에 마우스의 현재 위치를 스크린좌표로 받아오는 함수를 적용할 수 있다.
        transform.position = Mouse.current.position.ReadValue();
    }

    public void Open()
    {
        if(!itemSlot.IsEmpty)
        {
            transform.position = Mouse.current.position.ReadValue();
            gameObject.SetActive(true);     // 가볍지만, 빠르진 않다. (컴포넌트 일일이 비활성화함)
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
