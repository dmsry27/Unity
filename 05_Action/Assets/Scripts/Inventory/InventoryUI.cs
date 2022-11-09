using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    //ItemSlotUI가 있는 프리팹. 인벤토리 크기 변화에 대비해서 가지고 있기.
    public GameObject slotPrefab;

    Inventory inven;
    ItemSlotsUI[] slotsUIs;
    TempItemSlotUI tempSlotUI;

    private void Awake()
    {
        //Transform slotParent = transform.GetChild(0);
        slotsUIs = GetComponentsInChildren<ItemSlotsUI>();
        tempSlotUI = GetComponentInChildren<TempItemSlotUI>();
    }

    /// <summary>
    /// 입력받은 인벤토리에 맞게 각종 초기화 작업을 하는 함수
    /// </summary>
    /// <param name="playerInven">이 UI로 표시할 인벤토리</param>
    public void InitailizeInventory(Inventory playerInven)
    {
        inven = playerInven;

        Transform slotParent = transform.GetChild(0);
        GridLayoutGroup grid = slotParent.GetComponent<GridLayoutGroup>();

        if (Inventory.Default_Inventory_Size != inven.SlotCount)
        {
            // 기존 사이즈와 다르면 기존 슬롯은 전부 삭제하고 새로 만들기
            Debug.Log("인벤토리의 사이즈가 다르다.");
            foreach (var slot in slotsUIs)
            {
                Destroy(slot.gameObject);
            }

            RectTransform rectParent = (RectTransform)slotParent;
            float totalArea = rectParent.rect.width * rectParent.rect.height;
            float slotArea = totalArea / inven.SlotCount;
            float slotSideLength = Mathf.Floor(Mathf.Sqrt(slotArea)) - grid.spacing.x;
            grid.cellSize = new Vector2(slotSideLength, slotSideLength);

            slotsUIs = new ItemSlotsUI[inven.SlotCount];

            for (uint i = 0; i < inven.SlotCount; i++)
            {
                GameObject obj = Instantiate(slotPrefab, slotParent);
                obj.name = $"{slotPrefab.name}_{i}";
                slotsUIs[i] = obj.GetComponent<ItemSlotsUI>();
            }
        }
       
        for (uint i = 0; i < inven.SlotCount; i++)
        {
            slotsUIs[i].InitializeSlot(i, inven[i]);        // Indexer
            slotsUIs[i].Resize(grid.cellSize.x * 0.75f);
            slotsUIs[i].onDragStart += OnItemDragStart;
            slotsUIs[i].onDragEnd += OnItemDragEnd;
        }

        tempSlotUI.InitializeSlot(Inventory.TempSlotIndex, inven.TempSlot);
        tempSlotUI.Close();
    }

    private void OnItemDragStart(uint slotID)
    {
        inven.MoveItem(slotID, Inventory.TempSlotIndex);
        tempSlotUI.Open();
    }

    private void OnItemDragEnd(uint slotID)
    {
        tempSlotUI.Close();
        inven.MoveItem(Inventory.TempSlotIndex, slotID);
    }
}
