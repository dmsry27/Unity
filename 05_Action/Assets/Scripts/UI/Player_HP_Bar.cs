using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Player_HP_Bar : MonoBehaviour
{
    Slider slider;
    TextMeshProUGUI HP_Text;
    string maxHP_Text;
    float maxHP;
    
    private void Awake()
    {
        slider = GetComponent<Slider>();
        HP_Text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        Player player = GameManager.Inst.Player;
        maxHP = player.MaxHP;
        maxHP_Text = $"/{maxHP:f0}";
        slider.value = 1;

        HP_Text.text = $"{maxHP}/{maxHP}";
        player.onHealthChange += OnHealthChange;
    }

    private void OnHealthChange(float ratio)
    {
        ratio = Mathf.Clamp(ratio, 0, 1);
        slider.value = ratio;

        float hp = maxHP * ratio;
        HP_Text.text = $"{hp:f0}{maxHP_Text}";
    }
}
