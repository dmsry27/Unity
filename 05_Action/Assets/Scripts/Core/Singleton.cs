using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 싱글톤
// 1. 디자인 패턴 중 하나
// 2. 클래스의 객체를 무조건 하나만 생성하는 디자인 패턴
// 3. 데이터를 확신할 수 있다.
// 4. static 맴버를 이용해서 객체에 쉽게 접근 할 수 있도록 해준다.

// Singleton 클래스는 제네릭 타입의 클래스이다. (만들때 타입을 하나 받아야 한다.)
// where 이하의 있는 조건을 만족시켜야 한다. (T는 컴포넌트를 상속받는 타입이어야 한다.)
public class Singleton<T> : MonoBehaviour where T : Component
{
    private static bool isShutDown = false;
    private static T _instance = null;
    public static T Inst
    {
        get
        {
            if(isShutDown)
            {
                Debug.LogWarning($"{typeof(T)} 싱글톤은 이미 삭제되었음.");
                return null;
            }

            if (_instance == null)
            // 한번도 사용된 적이 없다.
            {
                var obj = FindObjectOfType<T>();
                if(obj != null)
                {
                    _instance = obj;        // 이미 다른 객체가 있으니까 있는 객체를 사용한다.
                }
                else
                {
                    // 다른 객체가 없다.
                    GameObject gameObject = new GameObject();
                    gameObject.name = $"{typeof(T).Name}";
                    _instance = gameObject.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            Initialize(); 
            DontDestroyOnLoad(this.gameObject);     // 씬이 사라지더라도 게임 오브젝트를 삭제하지 않게 하는 코드
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            if (_instance != this)
            {
                Destroy(this.gameObject);
            }
                
           
        }
    }

    //private void OnDestroy()
    //{
    //    isShutDown = true;
    //}

    private void OnApplicationQuit()
    {
        isShutDown = true;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Initialize();
    }

    protected virtual void Initialize()
    {

    }
}

// static 키워드
// 실행 시점에서 이미 메모리의 위치가 고정되게 하는 한정자 키워드

// new 키워드
// 동적 할당

// as 키워드
// a as b;      // a를 b타입으로 캐스팅을 시도한 후 실패하면 null 아니면 b타입으로 변경해서 처리
