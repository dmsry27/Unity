using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 이 스크립트를 가진 오브젝트는 y축을 기준으로 계속 회전(시계 방향)하면서 위아래로 올라갔다 내려갔다 한다(삼각함수 활용).
/// </summary>
public class ItemRotator : MonoBehaviour
{
    public float rotateSpeed = 360.0f;
    public float minHeight = 0.5f;
    public float maxHeight = 1.5f;
    float timeElapsed = 0.0f;

    Vector3 newPosition  = Vector3.zero;
    float trans = 0.0f;

    private void Start()
    {
        newPosition = transform.position;
        newPosition.y = minHeight;
        transform.position = newPosition;

        trans = 0.5f * (maxHeight - minHeight);
        timeElapsed = 0.0f;
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        newPosition.y = minHeight + (1 - Mathf.Cos(timeElapsed)) * trans;

        transform.position = newPosition;
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }
}
