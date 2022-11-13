using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    //ItemSlotUI가 있는 프리팹. 인벤토리 크기 변화에 대비해서 가지고 있기.
    public GameObject slotPrefab;

    Inventory inven;
    ItemSlotUI[] slotUIs;
    TempItemSlotUI tempSlotUI;
    DetailInfoUI detailUI;

    private void Awake()
    {
        // TempItemSlotUI가 ItemSlotUI를 상속받았기때문에 GetComponentsInChildren로 찾아진다?
        //slotUIs = GetComponentsInChildren<ItemSlotUI>();
        Transform slotParent = transform.GetChild(0);
        slotUIs = new ItemSlotUI[slotParent.childCount];
        for (int i = 0; i < slotParent.childCount; i++)
        {
            slotUIs[i] = slotParent.GetChild(i).GetComponent<ItemSlotUI>();
        }
        tempSlotUI = GetComponentInChildren<TempItemSlotUI>();
        detailUI = GetComponentInChildren<DetailInfoUI>();
    }

    // Player의 Inventory를 받아오기위해 만듬.
    public void InitializeInventory(Inventory playerInven)
    {
        inven = playerInven;
        Transform slotParent = transform.GetChild(0);
        GridLayoutGroup grid = slotParent.GetComponent<GridLayoutGroup>();

        if (Inventory.Default_Inventory_Size != inven.SlotCount)
        {
            // 기본 사이즈와 다르면 기존 슬롯을 모두 삭제하고 새로 만들기
            // 이런식으론 자식의 인덱스값이 삭제하면서 바뀌므로 이상해진다.
            //for( int i = 0; i < slotParent.childCount; i++)
            //{
            //    Destroy(slotParent.GetChild(i).gameObject);
            //}
            foreach(var slot in slotUIs)
            {
                Destroy(slot.gameObject);
            }

            RectTransform rectParent = (RectTransform)slotParent;
            float totalArea = rectParent.rect.width * rectParent.rect.height;
            float slotArea = totalArea / inven.SlotCount;

            float slotSideLength = Mathf.Floor(Mathf.Sqrt(slotArea)) - grid.spacing.x;
            grid.cellSize = new Vector2(slotSideLength, slotSideLength);

            slotUIs = new ItemSlotUI[inven.SlotCount];          // 배열 생성. 아직 null
            for(uint i = 0; i < inven.SlotCount; i++)
            {
                GameObject obj = Instantiate(slotPrefab, slotParent);
                obj.name = $"{slotPrefab.name}_{i}";
                slotUIs[i] = obj.GetComponent<ItemSlotUI>();    // obj. !!
            }
        }
        // 공통 처리부분
        for (uint i = 0; i < inven.SlotCount; i++)
        {
            slotUIs[i].InitializeSlot(i, inven[i]);             // Indexer (어짜피 Indexer로 배열의 인덱스를 알고있지않을까?)
            slotUIs[i].Resize(grid.cellSize.x * 0.75f);
            slotUIs[i].onDragBegin += OnItemMoveStart;
            slotUIs[i].onDragEnd += OnItemMoveEnd;
            slotUIs[i].onDragCancel += OnItemMoveEnd;
            slotUIs[i].onClick += OnItemMoveEnd;
            slotUIs[i].onPointerEnter += OnItemDetailOn;
            slotUIs[i].onPointerExit += OnItemDetailOff;
            slotUIs[i].onPointerMove += OnPointerMove;
        }
        tempSlotUI.InitializeSlot(Inventory.TempSlotIndex, inven.TempSlot);
        tempSlotUI.Close();
    }

    private void OnItemMoveStart(uint slotID)
    {
        inven.MoveItem(slotID, Inventory.TempSlotIndex);
        tempSlotUI.Open();
    }

    /// <summary>
    /// 드래그가 끝나거나 실패했을때 실행
    /// </summary>
    /// <param name="slotID">End : endSlotID, Cancel : 드래그 시작슬롯ID</param>
    private void OnItemMoveEnd(uint slotID)
    {
        inven.MoveItem(Inventory.TempSlotIndex, slotID);
        if(tempSlotUI.ItemSlot.IsEmpty)
        {
            tempSlotUI.Close();
        }
    }

    private void OnItemDetailOn(uint slotID)
    {
        detailUI.Open(slotUIs[slotID].ItemSlot.ItemData);
    }

    private void OnItemDetailOff(uint _)
    {
        detailUI.Close();
    }

    private void OnPointerMove(Vector2 pointerPos)
    {
        if (detailUI.IsOpen)
        {
            RectTransform rect = (RectTransform)detailUI.transform;
            if (pointerPos.x + rect.sizeDelta.x > Screen.width)
            {
                pointerPos.x -= rect.sizeDelta.x;
            }
            detailUI.transform.position = pointerPos;
        }
    }
}
