using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Player player;
    ItemDataManager itemData;
    InventoryUI inventoryUI;

    public Player Player => player;
    public ItemDataManager ItemData => itemData;    // 게임메니저를 통해 ItemDate를 읽으면, ItemDataManager에 다다른다.
    public InventoryUI InventoryUI => inventoryUI;

    /// <summary>
    /// GameManager가 새로 만들어지거나 Scene이 로드되었을 때 실행될 초기화 함수
    /// </summary>
    protected override void Initialize()
    {
        itemData = GetComponent<ItemDataManager>();
        player = FindObjectOfType<Player>();
        inventoryUI = FindObjectOfType<InventoryUI>();
    }
}
