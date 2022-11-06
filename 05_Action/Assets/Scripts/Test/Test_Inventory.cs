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
        inventory.PrintInventory();
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        inventory.AddItem(ItemIdCode.Ruby);
        inventory.AddItem(ItemIdCode.Emerald);
        inventory.AddItem(ItemIdCode.Diamond);
        inventory.AddItem(ItemIdCode.Emerald);
        inventory.AddItem(ItemIdCode.Ruby);
        inventory.PrintInventory();
    }

    protected override void Test3(InputAction.CallbackContext _)
    {
        Test2(_);

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

    protected override void Test4(InputAction.CallbackContext _)
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

    protected override void Test5(InputAction.CallbackContext _)
    {
        inventory.AddItem(ItemIdCode.Ruby, 9);
        inventory.AddItem(ItemIdCode.Emerald, 8);
        inventory.AddItem(ItemIdCode.Emerald, 20);
    }
}
