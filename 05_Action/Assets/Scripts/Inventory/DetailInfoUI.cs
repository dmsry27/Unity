using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class DetailInfoUI : MonoBehaviour
{

    TextMeshProUGUI itemName;
    TextMeshProUGUI itemValue;
    TextMeshProUGUI itemDesc;
    Image itemIcon;
    CanvasGroup canvas;

    bool isPause = false;
    public bool IsPause
    {
        get => isPause;
        set 
        {
            isPause = value;
            if(isPause)
            {
                Close();
            }
        }
    }
    public bool IsOpen => (canvas.alpha > 0.0f); 

    private void Awake()
    {
        itemIcon = transform.GetChild(0).GetComponent<Image>();
        itemName = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        itemValue = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        itemDesc = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        canvas = GetComponent<CanvasGroup>();
    }

    public void Open(ItemData itemData)
    {
        if (!isPause && itemData != null)       // 일시 정지 상태가 아니고
        {
            itemIcon.sprite = itemData.itemIcon;
            itemName.text = itemData.itemName;
            itemValue.text = itemData.value.ToString();
            itemDesc.text = itemData.itemDescription;
            canvas.alpha = 1.0f;                // 자식 전부 적용(Group)

            MovePosition(Mouse.current.position.ReadValue());
        }
    }

    public void Close()
    {
        canvas.alpha = 0.0f;
    }

    public void MovePosition(Vector2 pos)
    {
        RectTransform rect = (RectTransform)transform;
        if (pos.x + rect.sizeDelta.x > Screen.width)
        {
            pos.x -= rect.sizeDelta.x;
        }
        transform.position = pos;
    }
}
