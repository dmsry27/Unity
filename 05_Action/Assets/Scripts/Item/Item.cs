using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 1개를 표현할 클래스
/// </summary>
public class Item : MonoBehaviour
{
    // 몬스터가 죽으면 아이템을 드랍한다
    public GameObject prefab;
    public GameObject prefab1;
    public GameObject prefab2;

    // 플레이어가 아이템 근처에서 획득 버튼을 누르면 플레이어가 아이템을 획득한다

    private void Start()
    {
        float select = Random.Range(0.0f, 1.0f);
        if (select < 0.7f)
        {
            Instantiate(prefab, transform.position, transform.rotation, transform);
        }
        else if (select < 0.9f)
        {
            Instantiate(prefab1, transform.position, transform.rotation, transform);
        }
        else
        {
            Instantiate(prefab2, transform.position, transform.rotation, transform);
        }
    }
}
