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

    // overloading : 이름은 같은데 파라메터는 다른 함수를 만드는 것
    // overriding  : 이름, 파라메터, 리턴값이 같은 함수를 만드는 것

    /// <summary>
    /// 아이템 코드를 이용해 아이템을 생성하는 함수
    /// </summary>
    /// <param name="code">생성할 아이템 코드</param>
    /// <returns>생성된 아이템</returns>
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

    /// <summary>
    /// 아이템 코드를 이용해 특정 위치에 아이템을 생성하는 함수
    /// </summary>
    /// <param name="code">생성할 아이템 코드</param>
    /// <param name="position">생성할 위치</param>
    /// <param name="randomNoise">위치에 랜덤성을 더할지 여부</param>
    /// <returns>생성된 아이템</returns>
    public static GameObject MakeItem(ItemIdCode code, Vector3 position, bool randomNoise = false)      // 디폴트 파라메터 맨뒤
    {
        GameObject obj = MakeItem(code);
        if (randomNoise)
        {
            Vector2 noise = Random.insideUnitCircle * 0.5f;
            position.x += noise.x;
            position.z += noise.y;
        }
        obj.transform.position = position;
        return obj;
    }

    /// <summary>
    /// 아이템 코드를 이용해 아이템을 한번에 여러개 생성하는 함수
    /// </summary>
    /// <param name="code">생성할 아이템 코드</param>
    /// <param name="count">생성할 아이템 개수</param>
    /// <returns>생성된 아이템들이 담긴 배열</returns>
    public static GameObject[] MakeItem(ItemIdCode code, int count)
    {
        GameObject[] objs = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            objs[i] = MakeItem(code);
        }
        return objs;
    }

    /// <summary>
    /// 특정 위치에 아이템을 한번에 여러개 생성하는 함수
    /// </summary>
    /// <param name="code">생성할 아이템 코드</param>
    /// <param name="count">생성할 아이템 개수</param>
    /// <param name="position">생성할 위치</param>
    /// <param name="randomNoise">위치에 랜덤성을 더할지 여부</param>
    /// <returns></returns>
    public static GameObject[] MakeItem(ItemIdCode code, int count, Vector3 position, bool randomNoise = false)
    {
        GameObject[] objs = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            objs[i] = MakeItem(code, position, randomNoise);
        }
        return objs;
    }

    /// <summary>
    /// 아이템 ID로 아이템 생성
    /// </summary>
    /// <param name="id">생성할 아이템 ID</param>
    /// <returns>생성된 아이템</returns>
    public static GameObject MakeItem(int id)
    {
        if(id < 0)
        {
            return null;
        }
        return MakeItem((ItemIdCode)id);
    }

    /// <summary>
    /// 아이템 ID를 이용해 특정 위치에 아이템을 생성하는 함수
    /// </summary>
    /// <param name="id">생성할 아이템 ID</param>
    /// <param name="position">생성할 위치</param>
    /// <param name="randomNoise">위치에 랜덤성을 더할지 여부</param>
    /// <returns>생성된 아이템</returns>
    public static GameObject MakeItem(int id, Vector3 position, bool randomNoise = false)
    {
        GameObject obj = MakeItem(id);
        if (randomNoise)
        {
            Vector2 noise = Random.insideUnitCircle * 0.5f;
            position.x += noise.x;
            position.z += noise.y;
        }
        obj.transform.position = position;
        return obj;
    }

    public static GameObject[] MakeItem(int id, int count)
    {
        GameObject[] objs = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            objs[i] = MakeItem(id);
        }
        return objs;
    }

    public static GameObject[] MakeItem(int id, int count, Vector3 position, bool randomNoise = false)
    {
        GameObject[] objs = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            objs[i] = MakeItem(id, position, randomNoise);
        }
        return objs;
    }
}
