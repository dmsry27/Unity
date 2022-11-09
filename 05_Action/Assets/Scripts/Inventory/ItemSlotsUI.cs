using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

public class ItemSlotsUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private uint id;                // 몇번째 슬롯인가?

    protected ItemSlot itemSlot;    // 이 UI와 연결된 ItemSlot

    private Image itemImage;
    private TextMeshProUGUI itemCountText;

    public uint ID => id;
    public ItemSlot ItemSlot => itemSlot;

    public Action<uint> onDragStart;
    public Action<uint> onDragEnd;

    private void Awake()
    {
        itemImage = transform.GetChild(0).GetComponent<Image>();
        itemCountText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// 슬롯 초기화 함수
    /// </summary>
    /// <param name="id">슬롯의 Index</param>
    /// <param name="slot">이 UI가 보여줄 슬롯</param>
    public void InitializeSlot(uint id, ItemSlot slot)
    {
        this.id = id;
        this.itemSlot = slot;
        this.itemSlot.onSlotItemChange = Refresh;

        Refresh();
    }

    public void Resize(float iconSize)
    {
        RectTransform rectTransform = (RectTransform)itemImage.gameObject.transform;
        rectTransform.sizeDelta = new Vector2(iconSize, iconSize);
    }

    private void Refresh()
    {
        if(itemSlot.IsEmpty)
        {
            itemImage.sprite = null;
            itemImage.color = Color.clear;
            itemCountText.text = null;
            //itemCountText.text = "";      // 빈 문자열도 가비지 생성
        }
        else
        {
            itemImage.sprite = itemSlot.ItemData.itemIcon;
            itemImage.color = Color.white;
            itemCountText.text = $"{itemSlot.ItemCount}";
            //itemCountText.text = itemSlot.ItemCount.ToString();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // IBeginDragHandler, IEndDragHandler 구동에 필요

        //eventData.position : 마우스 포인터의 스크린좌표 값
        //eventData.delta    : 마우스 포인터의 위치 변화량 
        //eventData.button == PointerEventData.InputButton.Left : 마우스 왼쪽 버튼이 눌러져 있다.
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"드래그 시작 : {ID}번 슬롯");
        onDragStart?.Invoke(ID);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;
        ItemSlotsUI endSlot = obj.GetComponent<ItemSlotsUI>();
        Debug.Log($"드래그 종료 : {endSlot.ID}번 슬롯");
        onDragEnd?.Invoke(endSlot.ID);
    }
}
