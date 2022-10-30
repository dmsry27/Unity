using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Player player;

    public Player Player => player;

    /// <summary>
    /// GameManager가 새로 만들어지거나 Scene이 로드되었을 때 실행될 초기화 함수
    /// </summary>
    protected override void Initialize()
    {
        player = FindObjectOfType<Player>();
    }
}
