using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

// 슬롯 하나를 표현 = 슬롯 배열의 정보는 없다 = 델리게이트를 사용하게 된다
public class ItemSlotUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    protected ItemSlot itemSlot;    // 이 UI와 연결된 ItemSlot

    private uint id;                // 몇번째 슬롯인가?
    private Image itemImage;
    private TextMeshProUGUI itemCountText;

    public uint ID => id;
    public ItemSlot ItemSlot => itemSlot;

    public Action<uint> onDragBegin;
    public Action<uint> onDragEnd;
    public Action<uint> onDragCancel;
    public Action<uint> onClick;
    public Action<uint> onPointerEnter;
    public Action<uint> onPointerExit;
    public Action<Vector2> onPointerMove;

    private void Awake()
    {
        itemImage = transform.GetChild(0).GetComponent<Image>();
        itemCountText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void InitializeSlot(uint id, ItemSlot slot)
    {
        this.id = id;
        this.itemSlot = slot;       // 이 UI가 보여줄 ItemSlot. 할당 작업(연결 작업)
        this.itemSlot.onSlotItemChange = Refresh;

        onDragBegin = null;
        onDragEnd = null;
        onDragCancel = null;
        onClick = null;
        onPointerEnter = null;
        onPointerExit = null;
        onPointerMove = null;

        Refresh();
    }

    public void Resize(float iconSize)
    {
        RectTransform rectTransform = (RectTransform)itemImage.gameObject.transform;
        rectTransform.sizeDelta = new Vector2(iconSize, iconSize);
    }

    private void Refresh()
    {
        if (itemSlot.IsEmpty)
        {
            itemImage.sprite = null;
            itemImage.color = Color.clear;
            itemCountText.text = null;
            //itemCountText.text = "";          // 빈 문자열도 가비지 생성
        }
        else
        {
            itemImage.sprite = itemSlot.ItemData.itemIcon;
            itemImage.color = Color.white;
            itemCountText.text = itemSlot.ItemCount.ToString();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // IBeginDragHandler, IEndDragHandler 구동에 필요

        // eventData.position : 마우스 포인터의 스크린좌표값
        // eventData.delta    : 마우스 포인터의 위치 변화량    
        // eventData.button == PointerEventData.InputButton.Left : 마우스 왼쪽 버튼이 눌려져 있다.
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        onDragBegin?.Invoke(ID);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;
        if (obj != null)
        {
            // 피킹된 것이 있다. (대부분 UI)
            ItemSlotUI endSlot = obj.GetComponent<ItemSlotUI>();
            if (endSlot != null)
            {
                Debug.Log($"드래그 종료 : {endSlot.ID}번 슬롯");
                onDragEnd?.Invoke(endSlot.ID);
            }
            else
            {
                Debug.Log($"드래그 실패 : {ID}번 슬롯에서 실패");
                onDragCancel?.Invoke(ID);
            }
        }
    }

    /// <summary>
    /// EventSystems에서 클릭이 감지되면 실행되는 함수
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        onClick.Invoke(ID);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter?.Invoke(ID);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExit?.Invoke(ID);
    }
    
    /// <summary>
    /// EventSystems은 마우스 포인터가 이 UI 영역안에서 움직이면 함수를 실행시킨다. 
    /// </summary>
    public void OnPointerMove(PointerEventData eventData)
    {
        onPointerMove?.Invoke(eventData.position);
    }

    /// <summary>
    /// 버튼 클릭을 떼는 순간 실행
    /// Down : 누르는 순간 실행
    /// </summary>
    //public void OnPointerUp(PointerEventData eventData)     
    //{
    //}
}
