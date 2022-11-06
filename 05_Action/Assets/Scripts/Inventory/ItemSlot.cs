using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인벤토리 한칸의 정보를 나타내는 클래스
/// </summary>
public class ItemSlot
{
    uint itemCount = 0;

    ItemData slotItemData = null;

    uint slotIndex;

    public Action onSlotItemChange;

    public uint ItemCount
    {
        get => itemCount;
        private set
        {
            if(itemCount != value)
            {
                itemCount = value;
                onSlotItemChange?.Invoke();     // 주로 UI 갱신용
            }
        }
    }

    public ItemData ItemData
    {
        get => slotItemData;
        private set
        {
            if(slotItemData != value)
            {
                slotItemData = value;
                onSlotItemChange?.Invoke();
            }
        }
    }

    public bool IsEmpty => (slotItemData == null);
    public uint Index => slotIndex;

    public ItemSlot(uint index)
    {
        slotIndex = index;
    }

    /// <summary>
    /// 이 슬롯에 지정된 아이템을 설정된 갯수로 넣는 함수
    /// </summary>
    /// <param name="data">지정된 아이템</param>
    /// <param name="count">설정된 갯수</param>
    public void AssignSlotItem(ItemData data, uint count = 1)
    {
        if (data != null)
        {
            ItemCount = count;
            ItemData = data;
            Debug.Log($"인벤토리 {slotIndex}번 슬롯에 \"{ItemData.itemName}\" 아이템 {ItemCount}개 설정");
        }
        else
        {
            ClearSlotItem();
        }
    }

    public void ClearSlotItem()
    {
        ItemData = null;
        ItemCount = 0;
        Debug.Log($"인벤토리 {slotIndex}번 슬롯을 비웁니다.");
    }

    // 대응하는 함수도 짝을 맞춰 만들어야한다.
    public bool IncreaseSlotItem(out uint overCount, uint increaseCount = 1)
    {
        bool result;            // 다 넣는 것에 성공하면 true, 넘치면 false.
        int over = 0;
        uint newCount = ItemCount + increaseCount;
        over = (int)(newCount) - (int)ItemData.maxStackCount;

        if( over > 0)
        {
            // 아이템이 최대 갯수를 넘쳤다.
            ItemCount = ItemData.maxStackCount;
            overCount = (uint)over;
            result = false;
            Debug.Log($"인벤토리 {slotIndex}번 슬롯에 \"{ItemData.itemName}\" 아이템 최대치만큼 증가. 현재 {ItemCount}개, {over}개 넘침");
        }
        else
        {
            // 충분히 추가할 수 있다.
            ItemCount = newCount;
            overCount = 0;           // Underflow 방지용
            result = true;
            Debug.Log($"인벤토리 {slotIndex}번 슬롯에 \"{ItemData.itemName}\" 아이템 {increaseCount}개만큼 증가. 현재 {ItemCount}개");
        }
        
        return result;
    }

    public void DecreaseSlotItem(uint count = 1)
    {
        // 결과값이 0보다 작아지면 Underflow(표현 가능한 최대치에서 계산) 발생
        // ex) 1 - 3 = 4,294,967,294
        int newCount = (int)ItemCount - (int)count;

        if (newCount < 1)
        {
            ClearSlotItem();
        }
        else
        {
            ItemCount = (uint)newCount;
            Debug.Log($"인벤토리 {slotIndex}번 슬롯에 \"{ItemData.itemName}\" 아이템 {count}개만큼 감소. 현재 {ItemCount}개");
        }
    }
}
