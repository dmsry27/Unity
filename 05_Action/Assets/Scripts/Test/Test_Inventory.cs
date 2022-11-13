using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Inventory : TestBase
{
    [Range(1, 30)]
    public int invenSize = 10;
    public InventoryUI inventoryUI;

    Inventory inven;

    private void Start()
    {
        inven = new Inventory(invenSize);
        inventoryUI.InitializeInventory(inven);
    }

    protected override void Test1(InputAction.CallbackContext _)
    {
        inven.PrintInventory();
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        Test_AddItemForUI();
        //inventoryUI.InitializeInventory(inven);
    }

    protected override void Test3(InputAction.CallbackContext _)
    {
        inven.MoveItem(0, 3);
    }

    protected override void Test4(InputAction.CallbackContext _)
    {
        inven.MoveItem(3, 0);
    }

    protected override void Test5(InputAction.CallbackContext _)
    {
    }

    void Test_AddItemForUI()
    {
        inven.ClearInventory();

        inven.AddItem(ItemIdCode.Ruby);
        inven.AddItem(ItemIdCode.Ruby);
        inven.AddItem(ItemIdCode.Ruby);
        inven.AddItem(ItemIdCode.Ruby);
        inven.AddItem(ItemIdCode.Emerald);
        inven.AddItem(ItemIdCode.Emerald);
        inven.AddItem(ItemIdCode.Diamond);
        inven.PrintInventory();

        inven.AddItem(ItemIdCode.Ruby, 5);
        inven.AddItem(ItemIdCode.Ruby, 5);
        inven.AddItem(ItemIdCode.Ruby, 5);
        inven.AddItem(ItemIdCode.Ruby, 5);
        inven.PrintInventory();
    }

    private void Test_AddItem()
    {
        inven.AddItem(ItemIdCode.Ruby);
        inven.AddItem(ItemIdCode.Emerald);
        inven.AddItem(ItemIdCode.Diamond);
        inven.AddItem(ItemIdCode.Emerald);
        inven.AddItem(ItemIdCode.Ruby);
        inven.PrintInventory();
    }

    private void Test_MoveItem()
    {
        inven.MoveItem(0, 9);       // 1.에메(2) 2.다야(1) 9.루비(2)
        inven.PrintInventory();
        inven.MoveItem(9, 15);
        inven.PrintInventory();
        inven.MoveItem(1, 2);       // 1.다야(1) 2.에메(2) 9.루비(2)
        inven.PrintInventory();
        inven.MoveItem(5, 6);
        inven.PrintInventory();
        inven.MoveItem(5, 1);
        inven.PrintInventory();

        inven.AddItem(ItemIdCode.Diamond, 0);   // 0.다야(1) 1.다야(1) 2.에메(2) 9.루비(2)
        inven.PrintInventory();
        inven.MoveItem(0, 1);       // 1.다야(2) 2.에메(2) 9.루비(2)
        inven.PrintInventory();
    }

    private void Test_ItemStack()
    {
        //inven.RemoveItem(1);
        //inven.RemoveItem(0, 3);

        inven.AddItem(ItemIdCode.Ruby);
        inven.AddItem(ItemIdCode.Ruby);
        inven.AddItem(ItemIdCode.Ruby);
        inven.AddItem(ItemIdCode.Ruby);
        inven.AddItem(ItemIdCode.Emerald);
        inven.AddItem(ItemIdCode.Emerald);
        inven.AddItem(ItemIdCode.Diamond);
        inven.PrintInventory();
        inven.AddItem(ItemIdCode.Ruby, 5);
        inven.AddItem(ItemIdCode.Ruby, 5);
        inven.PrintInventory();
        inven.MoveItem(0, 5);
        inven.PrintInventory();
    }
}
