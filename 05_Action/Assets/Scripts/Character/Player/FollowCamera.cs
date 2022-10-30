using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    public float dieSpeed = 0.3f;

    Transform target;
    Vector3 offset = Vector3.zero;
    Vector3 diePosition = Vector3.zero;
    Quaternion dieRotation = Quaternion.identity;
    bool isTargetAlive;

    private void Start()
    {
        Player player = GameManager.Inst.Player;
        target = player.transform;
        isTargetAlive = player.IsAlive;
        player.onDie += OnTargetDie;
        offset = transform.position - target.position;
        // 원점으로 부터 시작하는 벡터를 위치벡터라 한다. a - b = c 에서 c의 시점을 원점으로 바꾸면 이것도 특정 위치를 가리키는 위치벡터라 할 수 있다.
        // 벡터의 차로 생긴 방향벡터를 정규화하지 않았기에 이 벡터는 크기와 방향을 모두 가지고 있다.
        // 그래서 offset(간격)이라고 표현하고 나태낼 수 있다.
    }

    private void LateUpdate()
    {
        if (isTargetAlive)
        {
            transform.position = Vector3.Lerp(transform.position, target.position + offset, moveSpeed * Time.deltaTime);
            // 선형 : 결과가 어느정도 예상이 된다.
            // 카메라의 위치 -> 목표치(대상의 위치 + 간격)로 보간하며 변경. 1/moveSpeed 초에 걸쳐 목표치까지 변경
            // target.position + -> 그 위치에서의 간격(위치벡터)
        }
        else
        {
            float delta = dieSpeed * Time.deltaTime;
            transform.position = Vector3.Slerp(transform.position, diePosition, delta);
            transform.rotation = Quaternion.Slerp(transform.rotation, dieRotation, delta);
        }
    }

    void OnTargetDie()
    {
        isTargetAlive = false;
        diePosition = target.position + target.up * 10.0f;
        dieRotation = Quaternion.LookRotation(-target.up, -target.forward);
    }
}
