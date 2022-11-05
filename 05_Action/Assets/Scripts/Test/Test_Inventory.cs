using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Inventory : TestBase
{
    Inventory inventory;

    private void Start()
    {
        inventory = new Inventory(10);
    }

    protected override void Test1(InputAction.CallbackContext _)
    {
        inventory.AddItem(ItemIdCode.Ruby);
        inventory.AddItem(ItemIdCode.Emerald);
        inventory.AddItem(ItemIdCode.Diamond);
        inventory.AddItem(ItemIdCode.Emerald);
        inventory.AddItem(ItemIdCode.Ruby);
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        inventory.PrintInventory();
    }

    protected override void Test3(InputAction.CallbackContext _)
    {
        inventory.ClearItem(15);
        inventory.ClearItem(1);
        inventory.ClearItem(3);
    }

    protected override void Test4(InputAction.CallbackContext _)
    {
        inventory.RemoveItem(1);
        inventory.RemoveItem(0, 3);
    }
}
