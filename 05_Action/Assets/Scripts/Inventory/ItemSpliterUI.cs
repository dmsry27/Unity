using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSpliterUI : MonoBehaviour
{
    uint itemSplitCount = 1;

    ItemSlotUI targetSlotUI;
    TMP_InputField inputField;
    Slider slider;

    private void Awake()
    {
        inputField = GetComponentInChildren<TMP_InputField>();
        slider = GetComponentInChildren<Slider>();

        Button increase = transform.GetChild(1).GetComponent<Button>();
        Button decrease = transform.GetChild(2).GetComponent<Button>();
        Button ok = transform.GetChild(4).GetComponent<Button>();
        Button cancel = transform.GetChild(5).GetComponent<Button>();
    }

    private void Start()
    {
        Close();
    }

    public void Open(ItemSlotUI target)
    {
        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }
}
