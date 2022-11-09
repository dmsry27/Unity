using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.InputSystem;

public class TempItemSlotUI : ItemSlotsUI
{
    private void Update()
    {
        //RectTransform rect = (RectTransform)gameObject.transform;
        //rect.position = Mouse.current.position.ReadValue();

        transform.position = Mouse.current.position.ReadValue();
    }

    public void Open()
    {
        if(!itemSlot.IsEmpty)
        {
            transform.position = Mouse.current.position.ReadValue();
            gameObject.SetActive(true);
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
