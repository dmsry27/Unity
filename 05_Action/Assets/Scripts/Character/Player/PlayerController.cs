using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float turnSpeed = 10.0f;
    public float walkSpeed = 3.0f;
    public float runSpeed = 5.0f;
    float currentSpeed = 3.0f;

    PlayerInputActions inputActions;
    CharacterController cc;
    Animator ani;
    Player player;

    Vector3 inputDir = Vector3.zero;
    Quaternion targetRotation = Quaternion.identity;
    MoveMode moveMode = MoveMode.Walk;  // 현재 이동 상태

    enum MoveMode       // 이동 상태를 나타내는 enum
    {
        Walk = 0,
        Run
    }

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        ani = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += MoveInput;
        inputActions.Player.Move.canceled += MoveInput;
        inputActions.Player.MoveModeChange.performed += OnMoveModeChange;
        inputActions.Player.Attack.performed += OnAttack;
        inputActions.Player.PickUp.performed += OnPickUp;
    }

    private void OnDisable()
    {
        inputActions.Player.PickUp.performed -= OnPickUp;
        inputActions.Player.Attack.performed -= OnAttack;
        inputActions.Player.MoveModeChange.performed -= OnMoveModeChange;
        inputActions.Player.Move.canceled -= MoveInput;
        inputActions.Player.Move.performed -= MoveInput;
        inputActions.Player.Disable();
    }

    private void Update()
    {
        if (player.IsAlive)
        {
            //transform.Translate(currentSpeed * Time.deltaTime * inputDir, Space.World);         // y축 회전이 이루어지면서 local좌표계가 바뀜.
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);    // 매 프레임마다 0.1의 비율씩 움직임

            cc.Move(currentSpeed * Time.deltaTime * inputDir);
            //cc.SimpleMove(currentSpeed * inputDir);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            // 컴퓨터마다 1초의 프레임수가 다를 수 있지만 1초에 Update함수가 호출되는 횟수에 Time.deltaTime(1/횟수)을 곱함으로써 초당 변수의 변화량으로 생각할 수 있다.
            // 위치 1 = Vector3.Lerp(위치1, 위치2, Time.deltaTime)
            // 기본적으로 Update함수는 한 프레임에 한 번 호출. 3번째 인자에 Time.deltaTime을 넣어주게 되면
            // 기존의 위치 1은 위치 2의 방향으로 둘 사이의 거리 * (전 프레임에 걸린 시간) 만큼 이동하게 되는 것.
            // ex) 위치 1 = 0, 위치 2 = 10, 전 프레임에 걸린 시간 = 0.1f (10fps)라 가정
            // Update0 -> 위치1 = 0
            // Update1 -> 위치1 = 1 // 전 프레임의 위치 +(10 * 0.1)
            // Update2 -> 위치1 = 1.9 // 전 프레임의 위치 +(9 * 0.1)
            // Update3 -> 위치1 = 2.71 // 전 프레임의 위치 +(8.1 * 0.1)
            // Update4 -> 위치1 = 3.439  // 전 프레임의 위치 +(7.29 * 0.1)
            // ...
            // Update? -> 위치1 = 위치2
        }
    }

    private void OnMoveModeChange(InputAction.CallbackContext _)
    {
        if(moveMode == MoveMode.Walk)
        {
            moveMode = MoveMode.Run;
            currentSpeed = runSpeed;
            if (inputDir != Vector3.zero)
            {
                ani.SetFloat("Speed", 1.0f);
            }
        }
        else
        {
            moveMode = MoveMode.Walk;
            currentSpeed = walkSpeed;
            if (inputDir != Vector3.zero)
            {
                ani.SetFloat("Speed", 0.3f); 
            }
        }
    }

    private void MoveInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        inputDir.x = input.x;
        inputDir.y = 0;
        inputDir.z = input.y;
        
        if (!context.canceled && player.IsAlive)
        {
            Quaternion cameraYRotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
            inputDir = cameraYRotation * inputDir;      // 입력을 y축으로 180도 회전

            targetRotation = Quaternion.LookRotation(inputDir);
            // 이동중 시프트 클릭이 적용안됨. 함수 호출 타이밍 
            if (moveMode == MoveMode.Walk)
            {
                ani.SetFloat("Speed", 0.3f);
            }
            else
            {
                ani.SetFloat("Speed", 1.0f);
            }
        }
        else
        {
            inputDir = Vector3.zero;
            ani.SetFloat("Speed", 0f);
        }

        //// 사원수와 벡터
        //Quaternion from = Quaternion.Euler(0f, 360f, 0f); 
        //Quaternion to = Quaternion.Euler(0f, 180f, 0f);

        //Quaternion q1 = Quaternion.Slerp(from, to, 0.5f);       // Slerp : 구형보간 (3번째 인자값은 보간 비율)

        //// 사원수와 벡터의 곱셈
        //Vector3 q2 = q1 * Vector3.forward;      // 결과는 벡터값. q1는 eulerAngles의 y값만 가지고있기에 (0,0,1) y축 90도 회전하면 (1,0,0)이 된다.
        ////Debug.Log(Vector3.Angle(q2, Vector3.forward));

        //// 사원수의 곱셈
        //Quaternion q3 = q1 * q1;        // 회전변환을 서로 결합하는 효과가 있다. 즉 더해진다. (단 동일축을 대상으로 회전변환하는 경우에)
    }

    private void OnAttack(InputAction.CallbackContext _)
    {
        //Debug.Log(ani.GetCurrentAnimatorStateInfo(0).normalizedTime);     // 현재 재생중인 애니메이션의 진행 상태(애니메이터의 게이지)를 알려줌 (0 ~ 1 : normalizedTime)
        if (player.IsAlive)
        {
            int comboState = ani.GetInteger("ComboState");
            comboState++;
            ani.SetInteger("ComboState", comboState);
            ani.SetTrigger("Attack");
        }
    }

    private void OnPickUp(InputAction.CallbackContext _)
    {
        player.ItemPickUp();
    }
}
