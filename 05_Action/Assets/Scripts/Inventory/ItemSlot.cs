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
        ItemCount = count;
        ItemData = data;
        Debug.Log($"인벤토리 {slotIndex}번 슬롯에 \"{ItemData.itemName}\" 아이템 {ItemCount}개 설정");
    }

    public void ClearSlotItem()
    {
        ItemData = null;
        ItemCount = 0;
        Debug.Log($"인벤토리 {slotIndex}번 슬롯을 비웁니다.");
    }

    // 대응하는 함수도 짝을 맞춰 만들어야한다.
    public void IncreaseSlotItem(uint count = 1)
    {
        if (!IsEmpty)
        {
            ItemCount += count;
            Debug.Log($"인벤토리 {slotIndex}번 슬롯에 \"{ItemData.itemName}\" 아이템 {count}개만큼 증가. 현재 {ItemCount}개");
        }
    }

    public void DecreaseSlotItem(uint count = 1)
    {
        if (!IsEmpty)
        {
            // 결과값이 0보다 작아지면 언더플로우(표현 가능한 최대치에서 계산) 발생
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
}
