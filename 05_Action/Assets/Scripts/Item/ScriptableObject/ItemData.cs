using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 에셋을 만들 수 있는 부모 클래스
[CreateAssetMenu(fileName = "New Item Data", menuName = "Scriptable Object/ItemData", order = 1)]
public class ItemData : ScriptableObject
{
    public uint id = 0;
    public string itemName = "아이템";
    public GameObject modelPrefab;          // 아이템의 외형을 표시할 프리펩
    public Sprite itemIcon;
    public uint value;
    public uint maxStackCount = 1;
    public string itemDescription;
}
