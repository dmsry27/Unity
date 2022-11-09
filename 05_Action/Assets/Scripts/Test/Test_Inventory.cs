using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Inventory : TestBase
{
    public InventoryUI inventoryUI;
    [Range(1, 30)]
    public int invenSize = 10;

    Inventory inventory;

    private void Start()
    {
        inventory = new Inventory(invenSize);
        inventoryUI.InitailizeInventory(inventory);
    }

    protected override void Test1(InputAction.CallbackContext _)
    {
        inventory.PrintInventory();
        inventoryUI.InitailizeInventory(inventory);
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        Test_AddItemForUI();
    }

    protected override void Test3(InputAction.CallbackContext _)
    {
        inventory.MoveItem(0, 3);
    }

    protected override void Test4(InputAction.CallbackContext _)
    {
        inventory.MoveItem(3, 0);
    }

    protected override void Test5(InputAction.CallbackContext _)
    {
        inventory.AddItem(ItemIdCode.Ruby, 9);
        inventory.AddItem(ItemIdCode.Emerald, 8);
        inventory.AddItem(ItemIdCode.Emerald, 20);
    }

    void Test_AddItemForUI()
    {
        inventory.ClearInventory();

        inventory.AddItem(ItemIdCode.Ruby);
        inventory.AddItem(ItemIdCode.Ruby);
        inventory.AddItem(ItemIdCode.Ruby);
        inventory.AddItem(ItemIdCode.Ruby);
        inventory.AddItem(ItemIdCode.Emerald);
        inventory.AddItem(ItemIdCode.Emerald);
        inventory.AddItem(ItemIdCode.Diamond);
        inventory.PrintInventory();

        inventory.AddItem(ItemIdCode.Ruby, 5);
        inventory.AddItem(ItemIdCode.Ruby, 5);
        inventory.AddItem(ItemIdCode.Ruby, 5);
        inventory.AddItem(ItemIdCode.Ruby, 5);
        inventory.PrintInventory();
    }

    private void Test_AddItem()
    {
        inventory.AddItem(ItemIdCode.Ruby);
        inventory.AddItem(ItemIdCode.Emerald);
        inventory.AddItem(ItemIdCode.Diamond);
        inventory.AddItem(ItemIdCode.Emerald);
        inventory.AddItem(ItemIdCode.Ruby);
        inventory.PrintInventory();
    }

    private void MoveItem()
    {
        inventory.MoveItem(0, 9);       // 1.에메(2) 2.다야(1) 9.루비(2)
        inventory.PrintInventory();
        inventory.MoveItem(9, 15);
        inventory.PrintInventory();
        inventory.MoveItem(1, 2);       // 1.다야(1) 2.에메(2) 9.루비(2)
        inventory.PrintInventory();
        inventory.MoveItem(5, 6);
        inventory.PrintInventory();
        inventory.MoveItem(5, 1);
        inventory.PrintInventory();

        inventory.AddItem(ItemIdCode.Diamond, 0);   // 0.다야(1) 1.다야(1) 2.에메(2) 9.루비(2)
        inventory.PrintInventory();
        inventory.MoveItem(0, 1);       // 1.다야(2) 2.에메(2) 9.루비(2)
        inventory.PrintInventory();
    }

    private void Test_ItemStack()
    {
        //inventory.RemoveItem(1);
        //inventory.RemoveItem(0, 3);

        inventory.AddItem(ItemIdCode.Ruby);
        inventory.AddItem(ItemIdCode.Ruby);
        inventory.AddItem(ItemIdCode.Ruby);
        inventory.AddItem(ItemIdCode.Ruby);
        inventory.AddItem(ItemIdCode.Emerald);
        inventory.AddItem(ItemIdCode.Emerald);
        inventory.AddItem(ItemIdCode.Diamond);
        inventory.PrintInventory();
        inventory.AddItem(ItemIdCode.Ruby, 5);
        inventory.AddItem(ItemIdCode.Ruby, 5);
        inventory.PrintInventory();
        inventory.MoveItem(0, 5);
        inventory.PrintInventory();
    }
}
