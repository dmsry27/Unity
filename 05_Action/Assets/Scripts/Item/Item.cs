using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 아이템 1개를 표현할 클래스
/// </summary>
public class Item : MonoBehaviour
{
    public ItemData data;

    private void Start()
    {
        Instantiate(data.modelPrefab, transform);
    }
}
