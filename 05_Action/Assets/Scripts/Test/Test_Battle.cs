using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Battle : TestBase
{
    Player player;

    private void Start()
    {
        player = GameManager.Inst.Player;
    }

    protected override void Test1(InputAction.CallbackContext _)
    {
        player.Defense(60);
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        player.HP = 100;
    }

    protected override void Test3(InputAction.CallbackContext _)
    {
        //GameManager.Inst.ItemData[0];     
        //GameManager.Inst.ItemData[ItemIdCode.Diamond];
        ItemFactory.MakeItem(ItemIdCode.Ruby);
        ItemFactory.MakeItem(ItemIdCode.Emerald, 8);
    }
}
