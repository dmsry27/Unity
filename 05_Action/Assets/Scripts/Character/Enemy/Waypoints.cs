using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    Transform[] children;

    int index = 0;

    public Transform Current => children[index];

    private void Awake()
    {
        //children = GetComponentsInChildren<Transform>();        // GetComponentsInChildren : 자기 자신을 포함함
        children = new Transform[transform.childCount];
        for(int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i);
        }
    }

    /// <summary>
    /// 다음 웨이포인트 리턴
    /// </summary>
    /// <returns></returns>
    public Transform MoveNext()
    {
        index++;
        index %= children.Length;       // 계속 반복을 위해 %연산 사용 (나머지 1-2-0)
        
        return children[index];
    }
}
