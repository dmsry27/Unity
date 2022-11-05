using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인벤토리의 정보만 가지는 클래스
/// </summary>
public class Inventory 
{
    public const int Default_Inventory_Size = 6;

    ItemSlot[] slots = null;
    ItemDataManager dataManager;

    public int SlotCount => slots.Length;

    // 생성자 : 클래스가 new될 때 실행되는 함수
    public Inventory(int size = Default_Inventory_Size)
    {
        slots = new ItemSlot[size];
        for (int i = 0; i < size; i++)
        {
            slots[i] = new ItemSlot((uint)i);
        }
        Debug.Log($"{size}칸짜리 인벤토리 생성");
        dataManager = GameManager.Inst.ItemData;
    }

    // 아이템 추가
    public bool AddItem(ItemIdCode code)
    {
        return AddItem(dataManager[code]);
    }

    public bool AddItem(ItemData data)
    {
        bool result = false;
        
        ItemSlot targetSlot = FindSameItem(data);

        if (targetSlot != null)
        {
            targetSlot.IncreaseSlotItem();
            result = true;
        }
        else
        {
            ItemSlot emptySlot = EmptyItemSlot();

            if (emptySlot != null)
            {
                // 비어있는 슬롯을 찾았다.
                emptySlot.AssignSlotItem(data);
                result = true;
            }
            else
            {
                // 인벤토리가 가득 찼다.
                Debug.Log("인벤토리가 가득찼습니다.");
            }
        }

        return result;
    }

    // 아이템 이동
    public bool AddItem(ItemIdCode code, uint index)
    {
        return AddItem(dataManager[code], index);
    }

    public bool AddItem(ItemData data, uint index)
    {
        bool result = false;

        if(IsValidSlotIndex(index))
        {
            ItemSlot slot = slots[index];
            if(slot.ItemData != null)
            {
                if(slot.ItemData == data)
                {
                    slot.IncreaseSlotItem();
                }
                else
                {

                }
            }
            else
            {
                slot.AssignSlotItem(data);
            }
        }
        else
        {
            Debug.Log($"실패 : {index}는 잘못된 인덱스입니다.");
        }

        return result;
    }

    public bool RemoveItem(uint slotIndex, uint decreaseCount = 1)
    {
        bool result = false;

        if(IsValidSlotIndex(slotIndex))
        {
            ItemSlot slot = slots[slotIndex];
            slot.DecreaseSlotItem(decreaseCount);
            result = true;
        }
        else
        {
            Debug.Log($"실패 : {slotIndex}는 잘못된 인덱스입니다.");
        }

        return result;
    }

    // 아이템 버리기
    public bool ClearItem(uint slotIndex)
    {
        bool result = false;

        if(IsValidSlotIndex(slotIndex))
        {
            //slots[slotIndex].ClearSlotItem();
            ItemSlot slot = slots[slotIndex];
            slot.ClearSlotItem();
            result = true;
        }
        else
        {
            Debug.Log($"실패 : {slotIndex}는 잘못된 인덱스입니다.");
        }

        return result;
    }


    // 아이템 사용

    bool IsValidSlotIndex(uint index) => (index < SlotCount);   // 배열의 크기보다 Index는 무조건 작다.

    ItemSlot FindSameItem(ItemData itemData)
    {
        ItemSlot findSlot = null;

        foreach(var slot in slots)
        {
            if(slot.ItemData == itemData)
            {
                findSlot = slot;
                break;
            }
        }

        return findSlot;
    }

    ItemSlot EmptyItemSlot()
    {
        // slots에는 번호정보밖에 없다.
        ItemSlot result = null;

        foreach(var slot in slots)
        {
            if(slot.IsEmpty)    
            {
                result = slot;
                break;
            }
        }
        return result;
    }

    public void PrintInventory()
    {
        string printText = "[ ";

        for (int i = 0; i < SlotCount - 1; i++) 
        {
            if (!slots[i].IsEmpty)
            {
                printText += $"{slots[i].ItemData.itemName}({slots[i].ItemCount}), ";
            }
            else
            {
                printText += "(빈칸), ";
            }
        }

        ItemSlot lastSlot = slots[SlotCount - 1];

        if (!lastSlot.IsEmpty)
        {
            printText += $"{lastSlot.ItemData.itemName}({lastSlot.ItemCount}) ]";
        }
        else
        {
            printText += "(빈칸) ]";
        }

        Debug.Log(printText);
    }
}
