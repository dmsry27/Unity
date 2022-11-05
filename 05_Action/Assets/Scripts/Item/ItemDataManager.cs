using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataManager : MonoBehaviour
{
    /// <summary>
    /// 모든 아이템 데이터 (종류별)
    /// </summary>
    public ItemData[] itemDatas;

    /// <summary>
    /// 아이템데이터 확인용 인덱서(Indexer). 배열처럼 사용할 수 있는 프로퍼티의 변종.
    /// </summary>
    /// <param name="id">itemDatas의 인덱스로 사용할 변수</param>
    /// <returns>itemDatas의 id번째 아이템데이터</returns>
    public ItemData this[uint id] => itemDatas[id];
    // Indexer : 프로퍼티가 이름을 통해 객체내의 데이터의 접근하게 해준다면, 인덱서는 인덱스를 통해 접근하게 해준다.
    // 프로퍼티는 객체내의 데이터에 접근할 수 있는 통로다. 인덱서 또한 같은 역할을 하지만 접근 도구가 다르다.
    // foreach문에 사용할 수 없다.
    // (이 스크립트 가리키는 변수)[id or code]

    /// <summary>
    /// 아이템데이터 확인용 인덱서
    /// </summary>
    /// <param name="code">확인할 아이템의 Enum 코드</param>
    /// <returns>code가 가르키는 아이템</returns>
    public ItemData this[ItemIdCode code] => itemDatas[(int)code];

    /// <summary>
    /// 전체 아이템 가지 수
    /// </summary>
    public int Length => itemDatas.Length;
}
