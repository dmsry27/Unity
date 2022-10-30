using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Player : MonoBehaviour, IBattle, IHealth
{
    ParticleSystem weaponPS;
    Transform weapon_r;
    Transform weapon_l;
    Collider weaponBlade;
    Animator ani;

    public float attackPower = 10.0f;
    public float defencePower = 3.0f;
    public float maxHP = 100.0f;
    float hp = 100.0f;
    float itemPickUpRange = 2.0f;
    bool isAlive = true;

    public bool IsAlive => isAlive;

    public float AttackPower => attackPower;

    public float DefencePower => defencePower;

    public float HP 
    { 
        get => hp;
        set
        {
            if (isAlive && hp != value)     // && 순서 생각
            { 
                hp = value;

                if (hp < 0)
                {
                    Die();
                }
                hp = Mathf.Clamp(hp, 0.0f, maxHP);
                onHealthChange?.Invoke(hp/maxHP);
            }
        } 
    }

    public float MaxHP => maxHP;

    public Action<float> onHealthChange { get; set; }

    public Action onDie { get; set; }

    private void Awake()
    {
        //weapon_r = GameObject.Find("weapon_r").transform;      // 씬에서 모든 오브젝트 검색해서 찾음
        //weapon_r = transform.Find("weapon_r");                 // 자기 자식만 찾음
        weapon_r = GetComponentInChildren<WeaponPosition>().transform;
        weapon_l = GetComponentInChildren<ShieldPosition>().transform;
        ani = GetComponent<Animator>();

        // 장비 교체가 일어나면 새로 설정해야 한다.
        weaponPS = weapon_r.GetComponentInChildren<ParticleSystem>();
        weaponBlade = weapon_r.GetComponentInChildren<Collider>();
    }

    private void Start()
    {
        hp = maxHP;
        isAlive = true;
    }

    public void WeaponEffectSwitch(bool on)
    {
        if (weaponPS != null)
        {
            if(on)
            {
                weaponPS.Play();
            }
            else
            {
                weaponPS.Stop();
            }
        }
    }

    public void WeaponBladeEnable()
    {
        if (weaponBlade != null)
        {
            weaponBlade.enabled = true;
        }
    }

    public void WeaponBladeDisable()
    {
        if (weaponBlade != null)
        {
            weaponBlade.enabled = false;
        }
    }

    public void ShowWeaponAndShield(bool isShow)
    {
        weapon_r.gameObject.SetActive(isShow);
        weapon_l.gameObject.SetActive(isShow);
    }

    public void Attack(IBattle target)
    {
        target.Defense(AttackPower);
    }

    public void Defense(float damage)
    {
        if (isAlive)
        {
            HP -= (damage - DefencePower);
            ani.SetTrigger("Hit");
        }
    }

    public void Die()
    {
        isAlive = false;
        ShowWeaponAndShield(true);
        ani.SetLayerWeight(1, 0.0f);        // layerIndex 확인
        ani.SetBool("IsAlive", isAlive);
        onDie?.Invoke();
    }

    public void ItemPickUp()
    {
        Collider[] items = Physics.OverlapSphere(transform.position, itemPickUpRange, LayerMask.GetMask("Item"));

        foreach(var item in items)
        {
            Destroy(item.gameObject);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position, transform.up, itemPickUpRange, 4.0f);
    }
#endif
}
