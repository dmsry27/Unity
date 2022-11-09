using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인벤토리의 정보만 가지는 클래스
/// </summary>
public class Inventory 
{
    public const int Default_Inventory_Size = 6;
    public const uint TempSlotIndex = 99999;        // 어떤 숫자든 상관없다. slots의 인덱스가 될 수 있는 값만 아니면 된다.

    ItemSlot[] slots = null;
    ItemSlot tempSlot = null;       // 드래그 중인 아이템을 임시 저장하는 슬롯
    ItemDataManager dataManager;

    public int SlotCount => slots.Length;
    public ItemSlot TempSlot => tempSlot;
    public ItemSlot this[uint index] => slots[index];

    // 생성자 : 클래스가 new될 때 실행되는 함수
    public Inventory(int size = Default_Inventory_Size)
    {
        slots = new ItemSlot[size];
        for (int i = 0; i < size; i++)
        {
            slots[i] = new ItemSlot((uint)i);
        }
        Debug.Log($"{size}칸짜리 인벤토리 생성");
        tempSlot = new ItemSlot(TempSlotIndex);
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
            result = targetSlot.IncreaseSlotItem(out uint _);
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

    public bool AddItem(ItemIdCode code, uint index)
    {
        return AddItem(dataManager[code], index);
    }

    public bool AddItem(ItemData data, uint index)
    {
        bool result = false;

        if (IsValidSlotIndex(index))
        {
            ItemSlot slot = slots[index];

            if (slot.IsEmpty)
            {
                slot.AssignSlotItem(data);
                result = true;
            }
            else
            {
                if (slot.ItemData == data)
                {
                    
                    result = slot.IncreaseSlotItem(out uint _);
                }
                else
                {
                    Debug.Log($"실패 : 인벤토리 {index}슬롯에 다른 아이템이 존재합니다.");
                }
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

    public void ClearInventory()
    {
        foreach(var slot in slots)
        {
            slot.ClearSlotItem();
        }
    }

    // 아이템 이동
    public void MoveItem(uint from, uint to)
    {
        // from에 아이템이 있고, to에 있다.
        // from에 아이템이 있고, to에 없다.
        // from에 아이템이 없고, to에 있다.   -> 아무 변화 없음
        // from에 아이템이 없고, to에 없다.   -> 아무 변화 없음

        if (IsValidAndNotEmptySlotIndex(from) && IsValidSlotIndex(to))
        {
            ItemSlot fromSlot = (from == Inventory.TempSlotIndex) ? TempSlot : slots[from];
            ItemSlot toSlot = (to == Inventory.TempSlotIndex) ? TempSlot : slots[to];

            if(fromSlot.ItemData == toSlot.ItemData)
            {
                // from과 to가 같은 아이템을 가지고 있으면 to에서 합치기
                toSlot.IncreaseSlotItem(out uint overCount, fromSlot.ItemCount);    // 아이템 증가 시도한 후 넘친 갯수 받아오기
                fromSlot.DecreaseSlotItem(fromSlot.ItemCount - overCount);          // from에서 to에 증가한만큼 감소시키기
                Debug.Log($"인벤토리의 {from}슬롯에서 {to}슬롯으로 아이템 합치기 성공");
            }
            else
            {
                ItemData tempData = fromSlot.ItemData;
                uint tempCount = fromSlot.ItemCount;

                fromSlot.AssignSlotItem(toSlot.ItemData, toSlot.ItemCount);
                toSlot.AssignSlotItem(tempData, tempCount);
                Debug.Log($"인벤토리의 {from}슬롯과 {to}슬롯의 아이템 교체 성공");
            }
        }
    }

    // 아이템 사용

    bool IsValidAndNotEmptySlotIndex(uint index)
    {
        if (IsValidSlotIndex(index))
        {
            ItemSlot testSlot = (index == TempSlotIndex) ? TempSlot : slots[index];

            return !testSlot.IsEmpty;
        }
        return false;
    }

    bool IsValidSlotIndex(uint index) => (index < SlotCount) || (index == Inventory.TempSlotIndex);   // 배열의 크기보다 Index는 무조건 작다.

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
