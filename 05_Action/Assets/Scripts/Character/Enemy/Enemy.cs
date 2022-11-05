using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;   // UNITY_EDITOR라는 전처리기가 설정되어있을 때만 실행버전에 넣어라 (빌드에서 빠짐)   
#endif

[RequireComponent(typeof(Rigidbody))]   // 필수적으로 필요한 컴포넌트가 있을 때 자동으로 넣어주는 유니티 속성(Attribute)
[RequireComponent(typeof(Animator))]   
public class Enemy : MonoBehaviour, IBattle, IHealth
{
    public Waypoints waypoints;
    public float moveSpeed = 3.0f;
    public float waitTime = 1.0f;           // 기다리는 시간
    public float sightRange = 10.0f;
    public float sightHalfAngle = 50.0f;

    Transform waypointTarget;
    Transform chaseTarget;
    Animator ani;       // 캐싱용 변수
    NavMeshAgent agent;
    ParticleSystem dieEffect;
    SphereCollider bodyCollider;
    Rigidbody rigid;

    float waitTimer;    // 남아있는 기다려야하는 시간

    public ItemDropInfo[] dropItems;

    [System.Serializable]       // public 구조체를 유니티상에서 보기 위해 필요
    public struct ItemDropInfo
    {
        public ItemIdCode id;
        [Range(0.0f, 1.0f)]
        public float dropPercentage;
    }

    EnemyState state = EnemyState.Patrol;   // 현재 적의 상태
    protected enum EnemyState
    {
        Wait = 0,
        Patrol,
        Chase,
        Dead
    }

    Action stateUpdate;

    protected EnemyState State
    {
        get => state;
        set
        {
            if (state != value)
            {
                //switch(state)       // 이전 상태 (상태를 나가면서 해야할 일 처리)
                //{
                //    case EnemyState.Wait:
                //        break;
                //    case EnemyState.Patrol:
                //        break;
                //    default:
                //        break;
                //}
                state = value;      // 새로운 상태로 변경
                switch (state)       // 새로운 상태 (새로운 상태로 들어가면서 해야할 일 처리)
                {
                    case EnemyState.Wait:
                        agent.isStopped = true;
                        waitTimer = waitTime;           // 타이머 초기화
                        ani.SetTrigger("Stop");
                        stateUpdate = Update_Wait;
                        break;
                    case EnemyState.Patrol:
                        agent.isStopped = false;
                        agent.SetDestination(WaypointTarget.position);      // isStopped 일때 실행되지 않음
                        ani.SetTrigger("Move");
                        stateUpdate = Update_Patrol;    // FixedUpdate에서 실행될 델리게이트 변경
                        break;
                    case EnemyState.Chase:
                        agent.isStopped = false;
                        ani.SetTrigger("Move");
                        stateUpdate = Update_Chase;
                        break;
                    case EnemyState.Dead:
                        agent.isStopped = true;
                        ani.SetTrigger("Die");
                        StartCoroutine(DeadRepresent());
                        stateUpdate = Update_Dead;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    protected float WaitTimer
    {
        get => waitTimer;
        set
        {
            waitTimer = value;
            if ( waypoints != null && waitTimer < 0)
            {
                State = EnemyState.Patrol;
            }
        }
    }

    protected Transform WaypointTarget
    {
        get => waypointTarget;
        set
        {
            waypointTarget = value;
        }
    }

    public float attackPower = 10.0f;
    public float defencePower = 3.0f;
    public float maxHP = 100.0f;
    float hp = 100.0f;

    public float HP 
    { 
        get => hp;
        set
        {
            if(hp != value)
            {
                hp = value;
                if(State != EnemyState.Dead && hp < 0)
                {
                    Die();
                }
                hp = Mathf.Clamp(hp, 0.0f, maxHP);
                onHealthChange?.Invoke(hp/maxHP);
            }
        } 
    }

    public float MaxHP => maxHP;

    public float AttackPower => attackPower;

    public float DefencePower => defencePower;

    public Action<float> onHealthChange { get; set; }
    public Action onDie { get; set; }

    private void Awake()
    {
        ani = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        dieEffect = GetComponentInChildren<ParticleSystem>();
        bodyCollider = GetComponent<SphereCollider>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        agent.speed = moveSpeed;    // 최대속도
        
        if (waypoints != null)
            WaypointTarget = waypoints.Current;
        else
            WaypointTarget = transform;

        // Start에서 초기값은 설정해주는게 좋다.

        //state = EnemyState.Wait;        
        //waitTimer = waitTime;
        State = EnemyState.Wait;
        ani.ResetTrigger("Stop");   // 트리거가 쌓이는 현상을 방지 (처음에 트리거가 눌려있음)
        hp = maxHP;

        onHealthChange += Test_HP_Change;
        onDie += Test_Die;
    }

    private void FixedUpdate()
    {
        if(State != EnemyState.Dead && SearchPlayer())
        {
            State = EnemyState.Chase;
        }
        stateUpdate();
    }

    void Update_Patrol()
    {
        // 도착 확인
        // agent.pathPending : 경로 계산이 진행중인지 확인. true면 아직 경로 계산 중
        // agent.stoppingDistance : 도착지점에 도착했다고 인정되는 거리
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            WaypointTarget = waypoints.MoveNext();
            State = EnemyState.Wait;
        }
    }

    void Update_Wait()
    {
        WaitTimer -= Time.fixedDeltaTime;
    }

    private void Update_Chase()
    {
        if(chaseTarget != null)
        {
            agent.SetDestination(chaseTarget.position);
        }
        else
        {
            State = EnemyState.Wait;
        }
    }

    void Update_Dead()
    {
    }

    bool SearchPlayer()
    {
        bool result = false;
        chaseTarget = null;

        Collider[] colliders = Physics.OverlapSphere(transform.position, sightRange, LayerMask.GetMask("Player"));
        if (colliders.Length > 0)
        {
            //Debug.Log("Player가 시야범위안에 들어왔다.");
            Vector3 playerPos = colliders[0].transform.position;
            Vector3 toPlayerDir = playerPos - transform.position;

            if(IsInSightAngle(toPlayerDir))
            {
                //Debug.Log("Player가 시야각안에 들어왔다.");

                // 체크는 여러번해야 정확도를 높힐 수 있다.
                // 가벼운 연산부터 무거운 쪽으로 (Raycast는 무겁다)
                if(!IsSightBlocked(toPlayerDir))
                {
                    chaseTarget = colliders[0].transform;
                    result = true;
                }

            }
        }
        //LayerMask.GetMask("Player","Water","UI"); // 리턴 2^6 + 2^4 + 2^5 = 64+16+32 = 112
        //LayerMask.NameToLayer();                  // 리턴 6

        return result; 
    }

    bool IsInSightAngle(Vector3 toTargetDir)
    {
        float angle = Vector3.Angle(transform.forward, toTargetDir);
        return (sightHalfAngle > angle);
    }

    bool IsSightBlocked(Vector3 toTargetDir)
    {
        bool result = true;
        
        // ray의 높이를 조절해줘야됨. 타겟의 방향으로 레이를 생성
        Ray ray = new(transform.position + transform.up * 0.5f, toTargetDir);
        
        // out : 매개변수 한정자 (충돌지점에 대한 정보를 구조체 변수(hit)에 담아준다.)
        // layer는 int. 즉, 4byte로 32개의 상태를 표현함. 32개의 0과 1을 이용해 0이면 감지하지 않고 1인 레이어만 감지.
        // 마치 크기가 32개인 bool 배열로 생각할 수 있다. bool은 1byte. 32개를 표현하기 위해선 32byte. 8배의 차이가 난다.
        // 그래서 layer엔 비트연산자 <<, >> 등을 쓴다.

        if (Physics.Raycast(ray, out RaycastHit hit, sightRange))
        {
            if (hit.collider.CompareTag("Player"))
            {
                result = false;     // 시야가 막히지 않았다.
            }
        }
        return result;
    }

    public void Die()
    {
        State = EnemyState.Dead;
        onDie?.Invoke();
        MakeDropItem();
    }

    void MakeDropItem()
    {
        float percentage = UnityEngine.Random.Range(0.0f, 1.0f);
        // 0.4f로 집에서 해보기
        int index = 0;
        float max = 0.0f;
        for (int i = 0; i < dropItems.Length; i++)
        {
            if(max < dropItems[i].dropPercentage)
            {
                max = dropItems[i].dropPercentage;
                index = i;      // index의 디폴트는 가장 확률이 높은 아이템으로 설정
            }
        }
        float checkPercentage = 0.0f;
        for (int i = 0; i < dropItems.Length; i++)
        {
            checkPercentage += dropItems[i].dropPercentage;
            if (percentage <= checkPercentage)
            {
                index = i;
                break;
            }
        }
        GameObject obj = ItemFactory.MakeItem(dropItems[index].id, transform.position, true);
    }

    public void Attack(IBattle target)
    {
        target.Defense(AttackPower);
    }

    public void Defense(float damage)
    {
        if (State != EnemyState.Dead)
        {
            ani.SetTrigger("Hit");
            HP -= (damage - DefencePower);
        }
    }

    IEnumerator DeadRepresent()
    {
        dieEffect.Play();
        dieEffect.transform.parent = null;
        Enemy_HP_Bar hpBar = GetComponentInChildren<Enemy_HP_Bar>();

        yield return new WaitForSeconds(1.5f);

        Destroy(hpBar.gameObject);
        agent.enabled = false;
        bodyCollider.enabled = false;
        rigid.isKinematic = false;
        rigid.drag = 10.0f;

        yield return new WaitForSeconds(1.5f);

        Destroy(dieEffect.gameObject);
        Destroy(this.gameObject);
    }

    public void Test()
    {
        SearchPlayer();
    }

    void Test_HP_Change(float ratio)
    {
        Debug.Log($"{gameObject.name}의 HP가 {HP}로 변경되었습니다.");
    }

    void Test_Die()
    {
        Debug.Log($"{gameObject.name}이 죽었습니다.");
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, transform.up, sightRange);

        if (SearchPlayer())
        {
            Handles.color = Color.red;
        }

        Vector3 forward = transform.forward * sightRange;
        Handles.DrawDottedLine(transform.position, transform.position + forward, 2.0f);
        
        Quaternion q1 = Quaternion.AngleAxis(-sightHalfAngle, transform.up);
        Quaternion q2 = Quaternion.AngleAxis(sightHalfAngle, transform.up);

        Handles.DrawLine(transform.position, transform.position + q1 * forward);
        Handles.DrawLine(transform.position, transform.position + q2 * forward);
        Handles.DrawWireArc(transform.position, transform.up, q1 * forward, sightHalfAngle * 2, sightRange, 5.0f);      // 항상 정방향으로 생각
    }

    /// <summary>
    /// Editor-only function that Unity calls when the script is loaded or a value changes in the Inspector.
    /// Unsupported type 에러 발생시 컴포넌트를 지운후 다시 추가 (Serializable때문에 발생)
    /// </summary>
    private void OnValidate()
    {
        if (State != EnemyState.Dead)
        {
            // 드랍 아이템의 드랍 확률 합을 1로 만들기
            float total = 0.0f;
            foreach(var item in dropItems)
            {
                total += item.dropPercentage;
            }
            // 값 입력할 때와 Enter입력시 둘 다 실행됨
            // (0, 0, 1) -> 0.3입력시 (0.3, 0, 1 / 1.3)으로 처리됨
            // (0.3, 0, 0.7)이 나오려면 어떻게 해야 할까?
            for (int i = 0; i < dropItems.Length; i++)
            {
                dropItems[i].dropPercentage /= total;
            }
        }
    }
#endif
}
