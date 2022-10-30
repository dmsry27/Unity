using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

/// <summary>
/// 아이템을 생성만하는 클래스. 팩토리 디자인 패턴
/// </summary>
public class ItemFactory
{
    static int itemCount = 0;       // 생성된 아이템 총 갯수. 아이템 생성 아이디 역할도 함

    public static GameObject MakeItem(ItemIdCode code)
    {
        GameObject obj = new GameObject();      // 빈 오브젝트 생성
        Item item = obj.AddComponent<Item>();
        item.data = GameManager.Inst.ItemData[code];
        
        string[] itemName = item.data.name.Split("_");
        obj.name = $"{itemName[1]}_{itemCount++}";
        obj.layer = LayerMask.NameToLayer("Item");

        SphereCollider sc = obj.AddComponent<SphereCollider>();
        sc.isTrigger = true;
        sc.radius = 0.5f;
        sc.center = Vector3.up;

        return obj;
    }

    public static GameObject MakeItem(uint id)
    {
        return MakeItem((ItemIdCode)id);
    }

    public static GameObject MakeItem(ItemIdCode code, Vector3 position)
    {
        GameObject obj = new GameObject();      // 같은 내용 또 쓰기 싫은데
        Item item = obj.AddComponent<Item>();
        item.data = GameManager.Inst.ItemData[code];

        string[] itemName = item.data.name.Split("_");
        obj.name = $"{itemName[1]}_{itemCount++}";
        obj.layer = LayerMask.NameToLayer("Item");
        obj.transform.position = position;

        SphereCollider sc = obj.AddComponent<SphereCollider>();
        sc.isTrigger = true;
        sc.radius = 0.5f;
        sc.center = Vector3.up;

        return obj;
    }

    public static GameObject MakeItems(ItemIdCode code, int count)
    {
        GameObject[] objs = new GameObject[count];
        foreach(var obj in objs)
        { 
            Item item = obj.AddComponent<Item>();
            item.data = GameManager.Inst.ItemData[code];

            string[] itemName = item.data.name.Split("_");
            obj.name = $"{itemName[1]}_{itemCount++}";
            obj.layer = LayerMask.NameToLayer("Item");

            SphereCollider sc = obj.AddComponent<SphereCollider>();
            sc.isTrigger = true;
            sc.radius = 0.5f;
            sc.center = Vector3.up;
        }

        return objs[count];
    }
}
